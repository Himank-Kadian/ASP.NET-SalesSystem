using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERacingSystem.VIEWMODELS
{
    public class InventoryViewModel
    {
        public int ProductID { get; set; }
        public string ItemName { get; set; }
        public int ReOrderLevel { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityOnOrder { get; set; }
        public string OrderUnitType { get; set; }
        public int OrderUnitSize { get; set; }

    }
}
