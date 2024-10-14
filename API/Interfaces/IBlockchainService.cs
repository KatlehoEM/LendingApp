using API.BlockchainStructure;
using API.DTOs;

namespace API.Interfaces;

public interface IBlockchainService
{
    Task<string> GetUserWallet(int userId);
    Task UpdateReputationScoreAsync(int userId);
    Task CreateReputationScoreAsync(int userId);

    Task<decimal> GetLatestReputationScoreAsync(int userId);
    Task<Wallet> CreateWalletAsync(int userId);
    int Count();
}
