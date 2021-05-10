using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERacingSystem.VIEWMODELS
{
    #region dropdowns

    public class CategoryDDViewModel
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
    }
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public int QuantityOnOrder { get; set; }
        public int QuantityOnHand { get; set; }
        public decimal? RestockCharge { get; set; }
    }
    #endregion

    public class InvoiceViewModel
    {
        public int InvoiceID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int EmployeeID { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GST { get; set; }
        public decimal? Total { get; set; }
        public List<InvoiceDetailsViewModel> Products { get; set; }
    }
    public class InvoiceDetailsViewModel
    {
        public String Category { get; set; }
        public int InvoiceID { get; set; }
        public int ProductID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal Amount { get; set; }
        public int QuantityOnHand { get; set; }
        public decimal RestockCharge { get; set; }
        public string RefundReason { get; set; }
        public InvoiceViewModel OriginalInvoice { get; set; }
    }

    public class StoreRefundViewModel
    {
        public int RefundID { get; set; }
        public int InvoiceID { get; set; }
        public int ProductID { get; set; }
        public int OriginalInvoiceID { get; set; }
        public string Reason { get; set; }
        public InvoiceViewModel RefundInvoice { get; set; }
        public InvoiceViewModel OriginalInvoice { get; set; }
    }
}
