using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERacingSystem.VIEWMODELS
{
    public class InventoryByVendorByCatagoryViewModel
    {
        public string Description { get; set; }
        public IEnumerable<InventoryViewModel> Items { get; set; }
    }
}
