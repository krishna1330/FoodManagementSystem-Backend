using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.Client.Models
{
    public class UserSelectedFood
    {
        public int UserID { get; set; }
        public int UserFoodID { get; set; }
        public int MenuID { get; set; }
        public string? SelectedFood { get; set; }
        public DateTime SelectedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
