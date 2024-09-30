using Assetmanagementsystem.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;

namespace Assetmanagementsystem.Repository
{
    public class LoginRepository : ILoginRepository
    {
        // VirtualDatabase private variable
        private readonly AssetManagementSystemContext _context;
        
        // Dependency Injection - DI
        public LoginRepository(AssetManagementSystemContext context)
        {
            _context = context; 
        }

        public async Task<IEnumerable<AssetMaster>> GetAllAssets()
        {
            try
            {
                //return await _context.AssetMasters.ToListAsync();
                if (_context != null)
                {
                    var asset = await _context.AssetMasters.Include(a => a.AmAd).Include(a => a.AmAtype).Include(a => a.AmMake).ToListAsync();
                    return asset;
                }
                return new List<AssetMaster>();
            }
            catch (Exception ex)
            {
                //return StatusCode(500, $"Internal server error: {ex.Message}");
                throw;
            }

        }

        public async Task<AssetMaster> GetAssetByIdAsync(int assetId)
        {
            if (_context != null)
            {
                return await _context.AssetMasters
                    .Include(a => a.AmAtype)
                    .Include(a => a.AmMake)
                    .Include(a => a.AmAd)
                    .FirstOrDefaultAsync(a => a.AmId == assetId);
            }
            return null;
        }

        // Get all assets
        public async Task<IEnumerable<AssetMaster>> GetAllAssetsAsync()
        {
            if (_context != null)
            {
                return await _context.AssetMasters
                    .Include(a => a.AmAtype)
                    .Include(a => a.AmMake)
                    .Include(a => a.AmAd)
                    .ToListAsync();
            }
            return null;

        }

        public async Task<UserRegistration> GetUserByIdAsync(int id)
        {
            try
            {
                return await _context.UserRegistrations.Include(u => u.Login).SingleOrDefaultAsync(u => u.UId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving user information.", ex);
            }

        }

        public async Task<UserRegistration> RegisterUserAsync(UserRegistration user)
        {
            try
            {
                
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "Employee data is null");
                }
              
                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized.");
                }
                await _context.UserRegistrations.AddAsync(user);    
                await _context.SaveChangesAsync();  
                var userWithLoginId = await _context.UserRegistrations
                    .Include(u=>u.Login).FirstOrDefaultAsync(u=>u.LoginId == user.LoginId);
                return userWithLoginId;
                
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the user.", ex);
            }

        }

        public async Task<UserRegistration> UpdateUserAsync(int uId, UserRegistration user)
        {/*
            var existingUser = await _context.UserRegistrations.FindAsync(uId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Update only the fields that are provided
            if (!string.IsNullOrEmpty(user.FirstName))
                existingUser.FirstName = user.FirstName;

            if (!string.IsNullOrEmpty(user.LastName))
                existingUser.LastName = user.LastName;

            if (user.Age.HasValue)
                existingUser.Age = user.Age;

            if (!string.IsNullOrEmpty(user.Gender))
                existingUser.Gender = user.Gender;

            if (!string.IsNullOrEmpty(user.Address))
                existingUser.Address = user.Address;

            if (!string.IsNullOrEmpty(user.PhoneNumber))
                existingUser.PhoneNumber = user.PhoneNumber;

            if (user.LoginId.HasValue)
                existingUser.LoginId = user.LoginId;

            await _context.SaveChangesAsync();
            return existingUser; // Return the updated user*/
            var existingUser = await _context.UserRegistrations.FindAsync(uId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Update only the fields that are provided
            existingUser.FirstName = user.FirstName ?? existingUser.FirstName;
            existingUser.LastName = user.LastName ?? existingUser.LastName;
            existingUser.Age = user.Age; // Already validated in controller
            existingUser.Gender = user.Gender ?? existingUser.Gender;
            existingUser.Address = user.Address ?? existingUser.Address;
            existingUser.PhoneNumber = user.PhoneNumber ?? existingUser.PhoneNumber;
            existingUser.LoginId = user.LoginId ?? existingUser.LoginId;

            await _context.SaveChangesAsync();
            return existingUser; // Return the updated user
        }

        public async Task<Login> ValidateUser(string username, string password)
        {
            try
            {
                if (_context != null)
                {
                    //TblUser dbUser =await _context.TblUsers.FirstOrDefaultAsync(u => u.UserName == username && u.UserPassword == password);
                    Login dbUser = await _context.Logins.FirstOrDefaultAsync(u=> u.Username == username && u.Password == password);
                    if (dbUser != null)
                    {
                        return dbUser;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

     

        public async Task<bool> UpdateAssetAsync(AssetMaster asset)
        {
            if (_context != null)
            {
                var existingAsset = await _context.AssetMasters.FirstOrDefaultAsync(a => a.AmId == asset.AmId);
                if (existingAsset != null)
                {
                    existingAsset.AmModel = asset.AmModel;
                    existingAsset.AmSnnumber = asset.AmSnnumber;
                    existingAsset.AmMyyear = asset.AmMyyear;
                    existingAsset.AmPdate = asset.AmPdate;
                    existingAsset.AmWarranty = asset.AmWarranty;
                    existingAsset.AmFromDate = asset.AmFromDate;
                    existingAsset.AmToDate = asset.AmToDate;
                    existingAsset.AmAtypeid = asset.AmAtypeid;
                    existingAsset.AmMakeid = asset.AmMakeid;
                    existingAsset.AmAdId = asset.AmAdId;

                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;

        }

        public async Task<IEnumerable<AssetMaster>> SearchAssetsAsync(string searchTerm)
        {
            if (_context != null)
            {
                return await _context.AssetMasters
                    .Where(a => a.AmModel.Contains(searchTerm) || a.AmSnnumber.Contains(searchTerm))
                    .ToListAsync();
            }
            return null;

        }

        public async Task<AssetMaster> InsertAssetReturnRecord(AssetMaster asset)
        {
            try
            {
                //check if user ibject is not null
                if (asset == null)
                {
                    throw new ArgumentNullException(nameof(asset), "asset Data is null");
                }
                // ensure the context is not null
                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized");
                }
                //Add the asset record to the dbcontext
                await _context.AssetMasters.AddAsync(asset);
                //pass it to realrime DB
                //save changes to the database
                await _context.SaveChangesAsync();
                //Retrieve the user with the related Department
                var Asset = await _context.AssetMasters
                    .Include(a => a.AmAd).Include(a => a.AmAtype).Include(a => a.AmMake)
                    .FirstOrDefaultAsync(a => a.AmId == asset.AmId);
                //return the added asset record
                return Asset;

            }
            catch (Exception ex)
            {
                //return StatusCode(500, $"Internal server error: {ex.Message}");
                throw new Exception($"An Error Accoured While Registering the asset Record:{ex.Message}", ex);
            }

        }

        public Task<AssetMaster> UpdateAssetReturnRecord(int id, AssetMaster asset)
        {
            throw new NotImplementedException();
        }
    }
}
