using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.Client.Models
{
    public class AddUserFood
    {
        public int UserID { get; set; }
        public DateTime SelectedDate { get; set; }
        public string? SelectedFood { get; set; }
    }
}
