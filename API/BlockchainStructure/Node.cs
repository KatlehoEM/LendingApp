namespace API.BlockchainStructure;

public class Node
{
    public string Id { get; private set; }
    public Blockchain Blockchain { get; private set; }
   // public List<Transaction> PendingTransactions { get; private set; }
    public List<string> Peers { get; private set; }

    public Node()
    {
        Id = Guid.NewGuid().ToString();
        Blockchain = new Blockchain();
        //PendingTransactions = new List<Transaction>();
        Peers = new List<string>();
    }

    public void AddTransaction(Transaction transaction)
    {
        Blockchain.AddTransaction(transaction);
    }

    public void Mine()
    {
        if(Blockchain.MineNewBlock())
        {
            Blockchain.ProcessPendingTransactions(Id);
        }
        
        //Block block = new Block(Blockchain.GetLatestBlock().Index + 1, DateTime.UtcNow, Blockchain.GetLatestBlock().Hash, PendingTransactions);
      //  Blockchain.AddBlock(block);
       // PendingTransactions.Clear();
    }

    public void AddPeer(string peerId)
    {
        if (!Peers.Contains(peerId))
        {
            Peers.Add(peerId);
        }
    }

    public void ReceiveBlock(Block block)
    {
        if (block.Index > Blockchain.GetLatestBlock().Index + 1)
        {
            // We're behind, request the full chain
            RequestFullChain();
        }
        else if (block.Index == Blockchain.GetLatestBlock().Index + 1 && block.PreviousHash == Blockchain.GetLatestBlock().Hash)
        {
            Blockchain.AddBlock(block);
            // Remove transactions in the new block from pending transactions
            //PendingTransactions.RemoveAll(t => block.Transactions.Contains(t));
        }
    }

    public void ReceiveChain(List<Block> chain)
    {
        Blockchain.ReplaceChain(chain);
    }

    public void RequestFullChain()
    {
        // In a real implementation, this would send a network request to peers
        // For simulation, we'll handle this in the Network class
    }
}
