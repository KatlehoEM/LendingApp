using System.Security.Cryptography;
using System.Text;

namespace API.BlockchainStructure;

public class Block
{
    public int Index { get; set; }
    public DateTime Timestamp { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }
    public List<Transaction> Transactions { get; set; }
    public int Nonce { get; set; }
    

    public Block(int index, DateTime timestamp, string previousHash, List<Transaction> transactions)
    {
        Index = index;
        Timestamp = timestamp;
        PreviousHash = previousHash;
        Transactions = transactions;
        Nonce = 0;
        Hash = CalculateHash();
    }

    public Block(){}

    public string CalculateHash()
    {
        string blockData = $"{Index}{Timestamp.ToString("s")}{PreviousHash}{string.Join("", Transactions)}{Nonce}";
        byte[] blockBytes = Encoding.UTF8.GetBytes(blockData);
        byte[] hashBytes = SHA256.Create().ComputeHash(blockBytes);
        return BitConverter.ToString(hashBytes).Replace("-", "");
    }

    public void Mine(int difficulty, ILogger<Blockchain> _logger)
    {
        string leadingZeros = new string('0', difficulty);
        _logger.LogInformation($"\n\n----------------- Mining block {Index}  ----------------\n");
        int attempts = 0;
        while (!Hash.StartsWith(leadingZeros))
        {
            Nonce++;
            Hash = CalculateHash();
            attempts++;
            
            _logger.LogInformation($"\tMining attempt {attempts}:  Hash = {Hash}");
            

            if (attempts % 100000 == 0)
            {
               _logger.LogInformation($"\nMining in progress: {attempts} attempts made");
            }
        }
    }
}
