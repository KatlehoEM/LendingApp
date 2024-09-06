

namespace API.BlockchainStructure;

public class Miner
{
    public string Address { get; private set; }

    public Miner()
    {
        Address = Guid.NewGuid().ToString(); // Simplified address generation
    }

    public void Mine(Blockchain blockchain)
    {
        blockchain.ProcessPendingTransactions(Address);
    }
}
