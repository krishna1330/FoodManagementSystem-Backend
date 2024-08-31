using FMS.Business.Client.Models;
using FMS.Business.DatabaseObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Data
{
    public class UsersDAC
    {
        private readonly FMS_DbContext _dbcontext;

        public UsersDAC(FMS_DbContext context)
        {
            _dbcontext = context;
        }

        public async Task<List<UserDetails>> GetAllAdmins()
        {
            try
            {
                var admins = await GetUserDetailsByUserTypeID(2);
                return admins;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all admins.", ex);
            }
        }

        public async Task<string> DeleteUserByID(int userID)
        {
            try
            {
                var user = await _dbcontext.Users
                    .FirstOrDefaultAsync(u => u.UserID == userID)
                    .ConfigureAwait(false);

                if (user == null)
                {
                    return "User not found.";
                }

                if (user.UserTypeID == 1)
                {
                    return "You cannot delete Super Admin.";
                }

                var userType = await _dbcontext.UserType
                    .Where(ut => ut.UserTypeID == user.UserTypeID)
                    .Select(ut => ut.Usertype)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                if (user.IsDeleted)
                {
                    return $"{userType} is already deleted.";
                }

                user.IsDeleted = true;
                user.DeletedBy = 1;

                await _dbcontext.SaveChangesAsync().ConfigureAwait(false);

                return $"{userType} deleted successfully.";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting user.", ex);
            }
        }

        public async Task<UserDetails> AddUserAsync(AddUser user)
        {
            try
            {
                Users newUser = new Users
                {
                    UserTypeID = user.UserTypeID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID,
                    Phone = user.Phone,
                    Password = "Created@123",
                    CreatedBy = user.CreatedBy,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                };

                await _dbcontext.Users.AddAsync(newUser);
                await _dbcontext.SaveChangesAsync();

                var addedUser = await _dbcontext.Users
                    .Where(user => user.UserID == newUser.UserID && !user.IsDeleted)
                    .Select(user => new UserDetails
                    {
                        UserID = user.UserID,
                        UserTypeID = user.UserTypeID,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailID = user.EmailID,
                        Phone = user.Phone,
                        IsActive = user.IsActive,
                    })
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                return addedUser;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting user.", ex);
            }
        }

        public async Task<List<UserDetails>> GetAllEmployees()
        {
            try
            {
                var employees = await GetUserDetailsByUserTypeID(3);
                return employees;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all employees.", ex);
            }
        }

        private async Task<List<UserDetails>> GetUserDetailsByUserTypeID(int userTypeID)
        {
            var admins = await _dbcontext.Users
                .Where(user => user.UserTypeID == userTypeID && !user.IsDeleted)
                .Select(user => new UserDetails
                {
                    UserID = user.UserID,
                    UserTypeID = user.UserTypeID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID,
                    Phone = user.Phone,
                    IsActive = user.IsActive,
                })
                .ToListAsync();

            return admins;
        }

    }
}
