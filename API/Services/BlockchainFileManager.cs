namespace API.Services;

using System.Text.Json;
using System.Text.Json.Serialization;
using API.BlockchainStructure;

public class BlockchainFileManager
{
    private const string BlockchainFileName = "blockchain_data.json";
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<Blockchain> _blockchainLogger;

    public BlockchainFileManager(ILogger<Blockchain> blockchainLogger)
    {
         _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never
        };
        _blockchainLogger = blockchainLogger;
    }

    public void SaveBlockchainData(Blockchain blockchain)
    {
        var chainData = blockchain.GetChainData();
        string jsonString = JsonSerializer.Serialize(chainData, _jsonOptions);

        File.WriteAllText(BlockchainFileName, jsonString);
    }

    public Blockchain LoadBlockchainData()
    {
        if (File.Exists(BlockchainFileName))
        {
            string jsonString = File.ReadAllText(BlockchainFileName);
            var chainData = JsonSerializer.Deserialize<ChainData>(jsonString,_jsonOptions);

            var blockchain = new Blockchain(_blockchainLogger)
            {
                Chain = chainData.Chain
            };
            return blockchain;
        }

        return new Blockchain(_blockchainLogger);
    }
}

public class ChainData
{
    public List<Block> Chain { get; set; }
}