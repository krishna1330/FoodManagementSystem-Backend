using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.Client.Models
{
    public class AuthorizedUser
    {
        public int UserID { get; set; }
        public int UserTypeID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailID { get; set; }
        public string? Phone { get; set; }
        public string? Token { get; set; }
        public string? ResponseMessage { get; set; }
    }
}
