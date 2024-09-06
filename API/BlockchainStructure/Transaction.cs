namespace API.BlockchainStructure;

public class Transaction
{
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public decimal Amount { get; set; }
    public DateTime TimeStamp { get; set; }

    public Transaction(string fromAddress, string toAddress, decimal amount)
    {
        FromAddress = fromAddress;
        ToAddress = toAddress;
        Amount = amount;
        TimeStamp = DateTime.UtcNow;
    }
    public Transaction(){}
}
