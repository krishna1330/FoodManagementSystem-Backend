using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.Client.Models
{
    public class FoodAvailabilityData
    {
        public int FoodAvailabilityID { get; set; }
        public int CreatedAdminID { get; set; }
        public int MonthNumber { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<string>? Menu { get; set; }
    }
}
