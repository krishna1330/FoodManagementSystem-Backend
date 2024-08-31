using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.DatabaseObjects
{
    public class UserType
    {
        [Key]
        public int UserTypeID { get; set; }
        public string? Usertype { get; set; }
    }
}
