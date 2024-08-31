using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.Client.Models
{
    public class UserDetails
    {
        public int UserID { get; set; }
        public int UserTypeID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailID { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
    }
}
