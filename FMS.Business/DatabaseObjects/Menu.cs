using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Business.DatabaseObjects
{
    public class Menu
    {
        public int MenuID { get; set; }
        public string? Food { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
    }
}
