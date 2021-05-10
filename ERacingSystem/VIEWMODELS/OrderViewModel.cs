using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERacingSystem.VIEWMODELS
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }
        public int? OrderNumber { get; set; }
        public string Comment { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
