namespace API.BlockchainStructure;

public class Wallet
{
    public string Address { get; private set; }
    public decimal Balance { get; private set; }

    public Wallet()
    {
        Address = Guid.NewGuid().ToString(); // Simplified address generation
        Balance = 0;
    }

    public void UpdateBalance(decimal amount)
    {
        Balance += amount;
    }
}
