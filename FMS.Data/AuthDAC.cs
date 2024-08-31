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
    public class AuthDAC
    {
        private readonly FMS_DbContext _dbcontext;

        public AuthDAC(FMS_DbContext context)
        {
            _dbcontext = context;
        }

        public async Task<AuthorizedUser?> IsAuthorizedUser(string emailID, string password)
        {
            try
            {
                var user = await this._dbcontext.Users.FirstOrDefaultAsync(u => u.EmailID == emailID && u.Password == password).ConfigureAwait(false);

                if (user == null)
                {
                    return new AuthorizedUser
                    {
                        ResponseMessage = "Invalid credentials."
                    };
                }

                if (!user.IsActive || user.IsDeleted)
                {
                    return new AuthorizedUser
                    {
                        ResponseMessage = "Your account is inactive or deleted."
                    };
                }

                return new AuthorizedUser
                {
                    UserID = user.UserID,
                    UserTypeID = user.UserTypeID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID,
                    Phone = user.Phone,
                    ResponseMessage = "Login successful."
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }

    }
}
