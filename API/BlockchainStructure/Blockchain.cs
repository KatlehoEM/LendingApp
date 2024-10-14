namespace API.BlockchainStructure;

public class Blockchain
{

    public List<Block> Chain { get; set; }
    public int Difficulty { get; set; } = 2;
    public List<Transaction> PendingTransactions { get; private set; }
    private DateTime LastMinedDay {get;set;}
    private readonly ILogger<Blockchain> _blockchainlogger;

    public Blockchain(ILogger<Blockchain> blockchainLogger)
    {
        _blockchainlogger = blockchainLogger;
        Chain = new List<Block> { CreateGenesisBlock() };
        PendingTransactions = new List<Transaction>();
        LastMinedDay = DateTime.UtcNow.Date;
    }

    private Block CreateGenesisBlock()
    {
        return new Block(0, DateTime.UtcNow, "0", new List<Transaction>());
    }

    public Block GetLatestBlock()
    {
        return Chain[Chain.Count - 1];
    }

    public object GetChainData()
    {
        return new { Chain = this.Chain};
    }

    public void AddBlock(Block block)
    {
        block.PreviousHash = GetLatestBlock().Hash;
        //_blockchainlogger.LogInformation($"\nMining block {block.Index}");
        block.Mine(Difficulty, _blockchainlogger);
        Chain.Add(block);
       // _blockchainlogger.LogInformation($"\nBlock {block.Index} added to the chain");
    }

    public bool MineNewBlock()
    {
        return  PendingTransactions.Count > 0; //PendingTransactions.Count >= 2 || DateTime.UtcNow.Date > LastMinedDay;
    }

    public bool IsValid()
    {
        for (int i = 1; i < Chain.Count; i++)
        {
            Block currentBlock = Chain[i];
            Block previousBlock = Chain[i - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash())
                return false;

            if (currentBlock.PreviousHash != previousBlock.Hash)
                return false;
        }
        return true;
    }

    public void AddTransaction(Transaction transaction)
    {
        PendingTransactions.Add(transaction);
    }

    public void ProcessPendingTransactions(string minerAddress)
    {
        if(!MineNewBlock())
        {
            return;
        }
        var  transactionsToMine = PendingTransactions.Where(t => t.TimeStamp.Date <= DateTime.UtcNow.Date).Take(4).ToList();

        Block block = new Block(GetLatestBlock().Index + 1, DateTime.UtcNow, GetLatestBlock().Hash, transactionsToMine);
        AddBlock(block);

        // Remove mined transaction from pending
        foreach(var transaction in transactionsToMine)
        {
            PendingTransactions.Remove(transaction);
        }
        AddTransaction(new Transaction(minerAddress, 1m,0)); // Mining reward

        LastMinedDay = DateTime.UtcNow.Date;
    }

    public void ReplaceChain(List<Block> newChain)
    {
        if (newChain.Count > Chain.Count && IsValidChain(newChain))
        {
            Chain = newChain;
        }
    }

    private bool IsValidChain(List<Block> chainToValidate)
    {
        if (chainToValidate[0].Hash != CreateGenesisBlock().Hash)
            return false;

        for (int i = 1; i < chainToValidate.Count; i++)
        {
            Block currentBlock = chainToValidate[i];
            Block previousBlock = chainToValidate[i - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash())
                return false;

            if (currentBlock.PreviousHash != previousBlock.Hash)
                return false;
        }
        return true;
    }
}
