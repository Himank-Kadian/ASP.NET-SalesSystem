using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERacingSystem.VIEWMODELS
{
    public class OrderDetailsViewModel
    {
        public int OrderDetailID { get; set; }
        public int ProductId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int OrderUnitSize { get; set; }
        public decimal UnitCost { get; set; }
        public decimal ItemCost { get; set; }
        public decimal ExtendedCost { get; set; }
    }
}
