namespace API.BlockchainStructure;

public class Network
{
   public Dictionary<string, Node> Nodes { get; set; }
   private readonly ILogger<Network> _networkLogger;
   private readonly ILogger<Blockchain> _blockchainLogger;
   private readonly ILogger<Node> _nodeLogger;

     public Network(ILogger<Network> networkLogger, ILogger<Node> nodeLogger, ILogger<Blockchain> blockchainLogger)
    {
        _networkLogger = networkLogger;
        _nodeLogger = nodeLogger;
        _blockchainLogger = blockchainLogger;
        Nodes = new Dictionary<string, Node>();
    }
    public void AddNode(Node node)
    {
        if (!Nodes.ContainsKey(node.Id))
        {
            Nodes.Add(node.Id, node);
            foreach (var existingNode in Nodes.Values)
            {
                if (existingNode.Id != node.Id)
                {
                    node.AddPeer(existingNode.Id);
                    existingNode.AddPeer(node.Id);
                }
            }
        }
    }

    public void BroadcastTransaction(Transaction transaction)
    {
         if (Nodes.Count == 0)
        {
            _networkLogger.LogWarning("No nodes in the network to broadcast transaction to.");
            return;
        }

       // _networkLogger.LogInformation($"Broadcasting transaction to {Nodes.Count} nodes");
        foreach (var node in Nodes.Values)
        {
            try
            {
                node.AddTransaction(transaction);
                //_networkLogger.LogInformation($"Transaction broadcasted to node {node.Id.Substring(0,4)}");
            }
            catch (Exception ex)
            {
                _networkLogger.LogError($"Failed to broadcast transaction to node {node.Id.Substring(0,4)}: {ex.Message}");
            }
        }
    }

    public void SimulateMining()
    {
        // _networkLogger.LogInformation($"\nSimulating mining. Total nodes: {Nodes.Count}");
        if (Nodes.Count == 0)
        {
            _networkLogger.LogWarning("No nodes in the network to perform mining.");
            return;
        }

        // Randomly select a node to mine the next block
        var miner = Nodes.Values.ElementAt(new Random().Next(Nodes.Count));
        
        if(miner.Blockchain.MineNewBlock())
        {
            //_networkLogger.LogInformation($"Node {miner.Id.Substring(0,4)} selected for mining");
            miner.Mine();

            // Broadcast the new block to all other nodes
            BroadcastLatestBlock(miner);

            // Reach consensus
            ReachConsensus();
        }
        else
        {
            _networkLogger.LogInformation("No new block to mine at this time.");
        }
        
    }

    private void BroadcastLatestBlock(Node fromNode)
    {
        var latestBlock = fromNode.Blockchain.GetLatestBlock();
       // _networkLogger.LogInformation($"Broadcasting latest block {latestBlock.Index} from node {fromNode.Id.Substring(0,4)}");
        
        // foreach (var node in Nodes.Values)
        // {
        //     if (node.Id != fromNode.Id)
        //     {
        //         node.ReceiveBlock(latestBlock);
        //        // _networkLogger.LogInformation($"Block {latestBlock.Index} sent to node {node.Id.Substring(0,4)}");
        //     }
        // }

        var node = Nodes.Values.ElementAt(new Random().Next(Nodes.Count));
        node.ReceiveBlock(latestBlock);


    }

     public void ReachConsensus()
    {
        _networkLogger.LogInformation($"\n\n----------------- Reaching Consensus ----------------\n");

        var longestChain = Nodes.Values.OrderByDescending(n => n.Blockchain.Chain.Count).First().Blockchain.Chain;

        foreach (var node in Nodes.Values)
        {
            node.ReceiveChain(longestChain);
            _networkLogger.LogInformation("\tConfirming that Node: " + node.Id.Substring(0,8) + " has a valid blockchain");

        }
         _networkLogger.LogInformation("\t\tConsensus reached");

    }

    public bool IsNetworkValid()
    {
        return Nodes.Values.All(n => n.Blockchain.IsValid());
    }

    public void SimulateNodeFailure(string nodeId)
    {
        if (Nodes.ContainsKey(nodeId))
        {
            Nodes.Remove(nodeId);
            // Remove the failed node from all peers
            foreach (var node in Nodes.Values)
            {
                node.Peers.Remove(nodeId);
            }
        }
    }

    public void SimulateNodeRecovery(string nodeId)
    {
        if (!Nodes.ContainsKey(nodeId))
        {
            var recoveredNode = new Node(_blockchainLogger, _nodeLogger);
            AddNode(recoveredNode);
            // The recovered node will request the full chain from its peers
            recoveredNode.RequestFullChain();
        }
    }
}
