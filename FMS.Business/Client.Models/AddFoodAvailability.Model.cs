using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.Client.Models
{
    public class AddFoodAvailability
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<Int32>? menuIDs { get; set; }
        public int CreatedAdminID { get; set; }
    }
}
