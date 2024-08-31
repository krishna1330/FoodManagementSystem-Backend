using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.Client.Models
{
    public class FoodCount
    {
        public DateTime SelectedDate { get; set; }
        public Dictionary<string, int> Food_Count { get; set; } = new Dictionary<string, int>();
    }
}

