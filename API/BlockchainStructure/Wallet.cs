namespace API.BlockchainStructure;

public class Wallet
{
    public string Address { get; private set; }
    public Wallet()
    {
        Address = Guid.NewGuid().ToString(); 
    }
}
