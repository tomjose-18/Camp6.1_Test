using Assetmanagementsystem.Models;
using NuGet.ContentModel;

namespace Assetmanagementsystem.Repository
{
    public interface ILoginRepository
    {
      public Task<Login> ValidateUser(string username, string password);

      public Task<UserRegistration> RegisterUserAsync(UserRegistration user);

     public Task<UserRegistration> GetUserByIdAsync(int id);

     public Task<UserRegistration> UpdateUserAsync(int uId, UserRegistration user);

      public Task<IEnumerable<AssetMaster>> GetAllAssets();
        // Read (Get) an asset by its ID
      public Task<AssetMaster> GetAssetByIdAsync(int assetId);

        public Task<AssetMaster> InsertAssetReturnRecord(AssetMaster asset);

        // Update an existing asset
        public Task<AssetMaster> UpdateAssetReturnRecord(int id, AssetMaster asset);

        // Search for assets based on specific criteria (e.g., model name)
        public Task<IEnumerable<AssetMaster>> SearchAssetsAsync(string searchTerm);

    }
}
