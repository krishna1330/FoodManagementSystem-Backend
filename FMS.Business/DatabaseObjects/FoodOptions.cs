using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.DatabaseObjects
{
    public class FoodOptions
    {
        public int FoodOptionsID { get; set; }
        public int FoodAvailabilityID { get; set; }
        public int MenuID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
    }
}
