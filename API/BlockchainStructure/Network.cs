namespace API.BlockchainStructure;

public class Network
{
   public Dictionary<string, Node> Nodes { get; set; }


     public Network()
    {
        Nodes = new Dictionary<string, Node>();
    }
    public void AddNode(Node node)
    {
        if (!Nodes.ContainsKey(node.Id))
        {
            Nodes.Add(node.Id, node);
            // Connect the new node to existing nodes
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
        foreach (var node in Nodes.Values)
        {
            node.AddTransaction(transaction);
        }
    }

    public void SimulateMining()
    {
        // Randomly select a node to mine the next block
        var miner = Nodes.Values.ElementAt(new Random().Next(Nodes.Count));
        
        if(miner.Blockchain.MineNewBlock())
        {
            miner.Mine();

            // Broadcast the new block to all other nodes
            BroadcastLatestBlock(miner);

            // Reach consensus
            ReachConsensus();
        }
        
    }

    private void BroadcastLatestBlock(Node fromNode)
    {
        var latestBlock = fromNode.Blockchain.GetLatestBlock();
        foreach (var node in Nodes.Values)
        {
            if (node.Id != fromNode.Id)
            {
                node.ReceiveBlock(latestBlock);
            }
        }
    }

     public void ReachConsensus()
    {
        var longestChain = Nodes.Values.OrderByDescending(n => n.Blockchain.Chain.Count).First().Blockchain.Chain;

        foreach (var node in Nodes.Values)
        {
            node.ReceiveChain(longestChain);
        }
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
            var recoveredNode = new Node();
            AddNode(recoveredNode);
            // The recovered node will request the full chain from its peers
            recoveredNode.RequestFullChain();
        }
    }
}
