using API.BlockchainStructure;
using API.DTOs;
using API.Interfaces;

namespace API.Services;

public class BlockchainService : IBlockchainService
{
    private readonly Network _network;
    private readonly BlockchainFileManager _fileManager;
    private readonly IUserRepository _userRepository;

    // Constants for reputation score calculation
    private const double W1 = 0.15; // User Rating
    private const double W2 = 0.4;  // Repayment history
    private const double W3 = 0.25; // Consistency score
    private const double W4 = 0.2;  // Credit score

    public BlockchainService(IUserRepository userRepository)
    {
        _network = new Network();
        _fileManager = new BlockchainFileManager();
        _userRepository = userRepository;

        InitialiseBlockchain();
    }

    private void InitialiseBlockchain()
    {
        var loadedChain = _fileManager.LoadBlockchainData();
        if (loadedChain.Chain.Count > 0)
        {
            var node = new Node();
            node.Blockchain.Chain = loadedChain.Chain;
            _network.AddNode(node);
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                _network.AddNode(new Node());
            }
        }
    }

    public async Task<Wallet> CreateWalletAsync(int userId)
    {
        var wallet = new Wallet();
        // Calculate initial reputation score and store it in the blockchain
        await UpdateReputationScoreAsync(userId);
        
        SaveBlockchainData();
        return wallet;
    }

    public async Task<string> GetUserWallet(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found");
        }

        if (string.IsNullOrEmpty(user.WalletAddress))
        {
            // User doesn't have a wallet, so create one
            var wallet = await CreateWalletAsync(userId);
            user.WalletAddress = wallet.Address;
            await _userRepository.UpdateUserAsync(user);
        }

        return user.WalletAddress;
    }



    public async Task UpdateReputationScoreAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        string address = user.WalletAddress;
        decimal score = await CalculateReputationScoreAsync(userId);
        Transaction transaction = new Transaction(null, address, score);
        _network.BroadcastTransaction(transaction);

        
            await SimulateMiningAsync();
     

        SaveBlockchainData();
    }

    public int Count()
    {
       return _network.Nodes.Values.First().Blockchain.PendingTransactions.Count();

    }
    private Task SimulateMiningAsync()
    {
        _network.SimulateMining();
        SaveBlockchainData();
        return Task.CompletedTask;
    }

    public async Task<decimal> GetLatestReputationScoreAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        string userWalletAddress = user.WalletAddress;

        var blockchain = _network.Nodes.Values.First().Blockchain;

        // Iterate through the blockchain in reverse order to find the latest reputation score
        for (int i = blockchain.Chain.Count - 1; i >= 0; i--)
        {
            var block = blockchain.Chain[i];
            var reputationTransaction = block.Transactions
                .LastOrDefault(t => t.ToAddress == userWalletAddress && t.FromAddress == null);

            if (reputationTransaction != null)
            {
                return reputationTransaction.Amount;
            }
        }

        // If no reputation score is found, calculate a new one
        return await CalculateReputationScoreAsync(userId);
    }

    private async Task<decimal> CalculateReputationScoreAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        double userRating =  await CalculateUserRating(userId);
        double repaymentHistory = await CalculateRepaymentHistoryAsync(userId);
        double consistencyScore = await CalculateConsistencyScoreAsync(userId);
        double creditScore = user.CreditScore;

        double totalWeight = W1 + W2 + W3 + W4;
        double score = (W1 * userRating + W2 * repaymentHistory + W3 * consistencyScore + W4 * creditScore) / totalWeight;

        return (decimal)score;
    }

    private async Task<double> CalculateUserRating(int userId)
    {
        var ratings = await _userRepository.GetUserRatingsAsync(userId);
    
        if (ratings == null || !ratings.Any())
        {
            return 50.0; // Default rating for users with no ratings
        }

        double averageRating = ratings.Average(r => r.Score);
        
        // Convert the average rating (1-5) to a scale of 0-100
        double normalizedRating = (averageRating - 1) / 4 * 100;
        
        return normalizedRating;
        
    }

    private async Task<double> CalculateRepaymentHistoryAsync(int userId) // look more into this
    {
        var blockchain = _network.Nodes.Values.First().Blockchain;
        var loanTransactions = new List<Transaction>();

        string userWallet = await GetUserWallet(userId);

        // Fetch all loan transactions involving the user
        foreach (var block in blockchain.Chain)
        {
            loanTransactions.AddRange(block.Transactions.Where(t => t.ToAddress == userWallet || t.FromAddress == userWallet));
        }

        if (!loanTransactions.Any())
        {
            return 50.0; // Default repayment history score for users with no loan history
        }

        // Calculate the percentage of on-time payments
        int onTimePayments = loanTransactions.Count(t => t.Amount > 0);
        int totalPayments = loanTransactions.Count;
        double onTimePaymentRatio = (double)onTimePayments / totalPayments;

        // Convert the ratio to a score on a 0-100 scale
        return onTimePaymentRatio * 100;
    }

    private async Task<double> CalculateConsistencyScoreAsync(int userId)
    {
        var blockchain = _network.Nodes.Values.First().Blockchain;
        var userTransactions = new List<Transaction>();

        string userWallet = await GetUserWallet(userId);

        // Fetch all transactions involving the user
        foreach (var block in blockchain.Chain)
        {
            userTransactions.AddRange(block.Transactions.Where(t => t.ToAddress == userWallet || t.FromAddress == userWallet));
        }

        if (userTransactions.Count < 5) // Minimum 5 transactions to calculate consistency
        {
            return 50.0; // Default consistency score for users with limited transaction history
        }

        // Calculate the standard deviation of the time differences between transactions
        var transactionTimes = userTransactions.OrderBy(t => t.TimeStamp).Select(t => t.TimeStamp).ToList();
        double meanTimeDiff = transactionTimes.Zip(transactionTimes.Skip(1), (t1, t2) => (t2 - t1).TotalDays).Average();
        double varianceTimeDiff = transactionTimes.Zip(transactionTimes.Skip(1), (t1, t2) => Math.Pow((t2 - t1).TotalDays - meanTimeDiff, 2)).Average();
        double stdDevTimeDiff = Math.Sqrt(varianceTimeDiff);

        // Convert the standard deviation to a consistency score on a 0-100 scale
        double consistencyScore = 100 - (stdDevTimeDiff / 7 * 100); // Assume a standard deviation of 7 days is the lowest score of 0
        return Math.Max(0, Math.Min(100, consistencyScore));
    }

    public bool ValidateNetwork()
    {
        return _network.IsNetworkValid();
    }

    public void SimulateNodeFailure(string nodeId)
    {
        _network.SimulateNodeFailure(nodeId);
        SaveBlockchainData();
    }

    public void SimulateNodeRecovery(string nodeId)
    {
        _network.SimulateNodeRecovery(nodeId);
        SaveBlockchainData();
    }

    private void SaveBlockchainData()
    {
        var blockchain = _network.Nodes.Values.First().Blockchain;
        _fileManager.SaveBlockchainData(blockchain);
   
    }

}