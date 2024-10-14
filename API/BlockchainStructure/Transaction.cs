namespace API.BlockchainStructure;

public class Transaction
{
    public string BorrowerAddress { get; set; }
     public decimal ReputationScore { get; set; }
    public int CreditScore { get; set; }
    public DateTime TimeStamp { get; set; }

     public Transaction(string borrowerAddress, decimal reputationScore, int creditScore)
    {
        BorrowerAddress = borrowerAddress;
        ReputationScore = reputationScore;
        CreditScore = creditScore;
        TimeStamp = DateTime.Now;
    }
    public Transaction(){}
}
