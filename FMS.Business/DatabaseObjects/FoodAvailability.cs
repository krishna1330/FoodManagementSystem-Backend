using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.DatabaseObjects
{
    public class FoodAvailability
    {
        public int FoodAvailabilityID { get; set; }
        public int MonthNumber { get; set; }
        public int CreatedAdminID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
    }
}
