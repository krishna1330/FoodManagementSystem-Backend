using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.DatabaseObjects
{
    public class UserFood
    {
        public int UserFoodID { get; set; }
        public int UserID { get; set; }
        public int MenuID { get; set; }
        public DateTime SelectedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedBy { get; set; }

    }
}
