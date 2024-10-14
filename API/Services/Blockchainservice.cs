using API.BlockchainStructure;
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class BlockchainService : IBlockchainService
{
    private readonly Network _network;
    private readonly BlockchainFileManager _fileManager;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<BlockchainService> _serviceLogger;
    private readonly ILogger<Network> _networkLogger;
   private readonly ILogger<Blockchain> _blockchainLogger;
   private readonly ILogger<Node> _nodeLogger;
    private readonly DataContext _context;


    // Constants for reputation score calculation
    private const double W1 = 0.15; // User Rating
    private const double W2 = 0.4;  // Repayment history
    private const double W3 = 0.25; // Consistency score
    private const double W4 = 0.2;  // Credit score

    public BlockchainService(IUserRepository userRepository, ILogger<BlockchainService> serviceLogger, ILogger<Network> networkLogger, ILogger<Node> nodeLogger, ILogger<Blockchain> blockchainLogger, DataContext context)
    {
        _serviceLogger = serviceLogger;
        _blockchainLogger = blockchainLogger;
        _networkLogger = networkLogger;
        _nodeLogger = nodeLogger;
        _network = new Network(networkLogger, nodeLogger, blockchainLogger);
        _fileManager = new BlockchainFileManager(_blockchainLogger);
        _userRepository = userRepository;
        _context = context;

        InitialiseBlockchain();
    }

    private void InitialiseBlockchain()
    {
        const int DesiredNodeCount = 4;
        var loadedChain = _fileManager.LoadBlockchainData();

        if (loadedChain.Chain.Count > 0)
        {
            // Create a node with the loaded chain
            var node = new Node(_blockchainLogger, _nodeLogger);
            node.Blockchain.Chain = loadedChain.Chain;
            _network.AddNode(node);
        }

        // Add additional nodes until we reach the desired count
        int currentNodeCount = _network.Nodes.Count;
        for (int i = currentNodeCount; i < DesiredNodeCount; i++)
        {
            var newNode = new Node(_blockchainLogger, _nodeLogger);
            
            // If we have a loaded chain, copy it to the new nodes
            if (loadedChain.Chain.Count > 0)
            {
                newNode.Blockchain.Chain = new List<Block>(loadedChain.Chain);
            }
            
            _network.AddNode(newNode);
            //_serviceLogger.LogInformation($"Node {i + 1} added.");
        }

        //_serviceLogger.LogInformation($"Blockchain initialized with {DesiredNodeCount} nodes.");
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
        decimal reputationScore = await CalculateReputationScoreAsync(userId);
        Transaction transaction = new Transaction(address, reputationScore, user.CreditScore);
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
                .LastOrDefault(t => t.BorrowerAddress == userWalletAddress);

            if (reputationTransaction != null)
            {
                return reputationTransaction.ReputationScore;
            }
        }

        // If no reputation score is found, calculate a new one
        return await CalculateReputationScoreAsync(userId);
    }

    public async Task CreateReputationScoreAsync(int userId)
    {
        // Get user information from the repository
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        // Ensure the user has a valid credit score
        if (user.CreditScore < 300 || user.CreditScore > 850)
        {
            throw new ArgumentException("Invalid credit score range.");
        }

        // Normalize the credit score to a 1-100 scale for the initial reputation score
        double normalizedReputationScore = NormalizeCreditScore(user.CreditScore);

        string address = await GetUserWallet(userId);
        Transaction transaction = new Transaction(address, (decimal)normalizedReputationScore, user.CreditScore);
        _network.BroadcastTransaction(transaction);

        await SimulateMiningAsync();

        SaveBlockchainData();
    }



    private async Task<decimal> CalculateReputationScoreAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        double userRating = await CalculateUserRating(userId);
        double repaymentHistory = await CalculateRepaymentHistoryAsync(userId);
        double consistencyScore = await CalculateConsistencyScoreAsync(userId);
        double creditScore = NormalizeCreditScore(user.CreditScore);

        double totalWeight = W1 + W2 + W3 + W4;
        double score = (W1 * userRating + W2 * repaymentHistory + W3 * consistencyScore + W4 * creditScore) / totalWeight;

        return (decimal)Math.Round(Math.Max(1, Math.Min(100, score)));
    }

    private async Task<double> CalculateUserRating(int userId)
{
    var ratings = await _userRepository.GetUserRatingsAsync(userId);

    if (ratings == null || !ratings.Any())
    {
        return 0.0; // Default rating for users with no ratings
    }

    // Calculate the average rating directly
    double totalScore = 0;
    int ratingCount = ratings.Count;

    // Sum all the scores
    for (int i = 0; i < ratingCount; i++)
    {
        totalScore += ratings[i].Score;
    }

    // Calculate the average rating
    double averageRating = totalScore / ratingCount;

    // Convert the average rating (1-5) to a scale of 1-100
    double normalizedRating = (averageRating - 1) / 4 * 99 + 1;

    return normalizedRating;
}

    private async Task<double> CalculateRepaymentHistoryAsync(int userId)
    {
        // Fetch all loans associated with the borrower
        var userLoans = await _context.Loans
            .Include(l => l.Payments)  // Include related payments
            .Where(l => l.BorrowerId == userId && l.Status == LoanStatus.Active)
            .ToListAsync();

        if (!userLoans.Any())
        {
            return 50.0; // Default score for users with no loan history
        }

        var totalPaymentsMade = 0m;      // Total payments made by the borrower
        var totalPrincipal = 0m;         // Total principal amounts of all loans

        foreach (var loan in userLoans)
        {
            totalPrincipal += loan.PrincipalAmount;  // Sum of all principal amounts
            
            foreach (var payment in loan.Payments)
            {
                totalPaymentsMade += payment.Amount; // Sum of all payments made by the borrower
            }
        }

        // Calculate repayment history as a percentage (total payments made vs total principal)
        var repaymentHistory = totalPaymentsMade / totalPrincipal * 100;

        // Ensure the repayment history is within the 1-100 scale
        return Math.Max(1, Math.Min(100, (double)repaymentHistory));
    }


   
    private async Task<double> CalculateConsistencyScoreAsync(int userId)
    {
        var blockchain = _network.Nodes.Values.First().Blockchain;
        var userTransactions = new List<Transaction>();

        string userWallet = await GetUserWallet(userId);

        // Fetch all transactions involving the user
        foreach (var block in blockchain.Chain)
        {
            userTransactions.AddRange(block.Transactions.Where(t => t.BorrowerAddress == userWallet));
        }

        if (userTransactions.Count < 5) // Minimum 5 transactions to calculate consistency
        {
            return 0.0; // Default consistency score for users with limited transaction history
        }

        // Calculate the standard deviation of the time differences between transactions
        var transactionTimes = userTransactions.OrderBy(t => t.TimeStamp).Select(t => t.TimeStamp).ToList();
        double meanTimeDiff = transactionTimes.Zip(transactionTimes.Skip(1), (t1, t2) => (t2 - t1).TotalDays).Average();
        double varianceTimeDiff = transactionTimes.Zip(transactionTimes.Skip(1), (t1, t2) => Math.Pow((t2 - t1).TotalDays - meanTimeDiff, 2)).Average();
        double stdDevTimeDiff = Math.Sqrt(varianceTimeDiff);

        // Convert the standard deviation to a consistency score on a 1-100 scale
        // Lower standard deviation means higher consistency
        double consistencyScore = 100 - (stdDevTimeDiff / 30 * 99); // Assuming 30 days as maximum inconsistency
        return Math.Max(1, Math.Min(100, consistencyScore));
    }

    private double NormalizeCreditScore(int creditScore)
    {
        const int minCreditScore = 300;
        const int maxCreditScore = 850;

        // Clamp the credit score to the valid range
        creditScore = Math.Max(minCreditScore, Math.Min(maxCreditScore, creditScore));

        // Normalize the credit score to a 1-100 scale
        decimal normalizedScore = ((decimal)(creditScore - minCreditScore) / (maxCreditScore - minCreditScore)) * 100;

        return (double)Math.Round(normalizedScore);
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