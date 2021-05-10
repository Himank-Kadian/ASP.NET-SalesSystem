using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.ComponentModel;
using ERacingSystem.DAL;
using ERacingSystem.VIEWMODELS;
using ERacingSystem.ENTITIES;
#endregion

namespace ERacingSystem.BLL
{
    [DataObject]
    public class SalesController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CategoryDDViewModel> Category_List()
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.Categories
                              select new CategoryDDViewModel
                              {
                                  CategoryId = x.CategoryID,
                                  Description = x.Description
                              };
                return results.OrderBy(x => x.Description).ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ProductViewModel> Product_List(int categoryId)
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.Products
                              where x.CategoryID == categoryId
                              select new ProductViewModel
                              {
                                  ProductID = x.ProductID,
                                  ItemName = x.ItemName
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<InvoiceDetailsViewModel> Product_Selected(int productId)
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.Products
                              where x.ProductID == productId
                              select new InvoiceDetailsViewModel
                              {
                                  ProductID = x.ProductID,
                                  ItemName = x.ItemName,
                                  Price = x.ItemPrice,
                                  RestockCharge = x.ReStockCharge,
                                  QuantityOnHand = x.QuantityOnHand
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<InvoiceDetailsViewModel> Invoice_List(int invoiceId)
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from y in context.InvoiceDetails
                              where y.InvoiceID == invoiceId
                              select new InvoiceDetailsViewModel
                              {
                                  Category = y.Product.Category.Description,
                                  ProductID = y.ProductID,
                                  ItemName = y.Product.ItemName,
                                  Quantity = y.Quantity,
                                  Price = y.Price,
                                  Amount = (decimal)(y.Quantity * y.Price),
                                  RestockCharge = y.Product.ReStockCharge
                              };
                return results.ToList();
            }
        }
        public int Purchase(InvoiceViewModel currentInvoice, string employeeName)
        {
            using (var context = new ERaceingSystemContext())
            {
                int? employeeID = (from x in context.AspNetUsers
                                   where x.UserName == employeeName
                                   select x.EmployeeId).FirstOrDefault();

                ICollection<InvoiceDetail> list = new List<InvoiceDetail>();

                foreach (var item in currentInvoice.Products)
                {
                    var result = (from x in context.Products
                                  where x.ProductID == item.ProductID
                                  select x).First();

                    result.QuantityOnHand = result.QuantityOnHand - item.Quantity;

                    var invoiceProducts = new InvoiceDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    list.Add(invoiceProducts);
                }

                Invoice invoice = new Invoice
                {
                    InvoiceDate = currentInvoice.InvoiceDate,
                    EmployeeID = (int)employeeID,
                    SubTotal = currentInvoice.Subtotal,
                    GST = currentInvoice.GST,
                    Total = currentInvoice.Total,
                    InvoiceDetails = list
                };
                context.Invoices.Add(invoice);
                context.SaveChanges();
                var invoiceId = invoice.InvoiceID;
                return invoiceId;
            }
        }
        public int Refund(InvoiceViewModel currentInvoice, string employeeName)
        {
            using (var context = new ERaceingSystemContext())
            {
                ICollection<InvoiceDetail> list = new List<InvoiceDetail>();

                int? employeeID = (from x in context.AspNetUsers
                                   where x.UserName == employeeName
                                   select x.EmployeeId).FirstOrDefault();

                var originalInvoice = (from x in context.Invoices
                                       where x.InvoiceID == currentInvoice.InvoiceID
                                       select x).First();
                
                foreach (var item in currentInvoice.Products)
                {
                    var exist = (from x in context.StoreRefunds
                                 where x.OriginalInvoiceID == currentInvoice.InvoiceID
                                 && x.ProductID == item.ProductID
                                 select x);

                    if (exist.Count() > 0)
                        throw new Exception("The item(s) were previously refunded. You can not refund an item again.");

                    var result = (from x in context.Products
                                  where x.ProductID == item.ProductID
                                  select x).First();

                    result.QuantityOnHand = result.QuantityOnHand + item.Quantity;

                    var invoiceProducts = new InvoiceDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    list.Add(invoiceProducts);
                }

                Invoice refundInvoice = new Invoice
                {
                    InvoiceDate = currentInvoice.InvoiceDate,
                    EmployeeID = (int)employeeID,
                    SubTotal = currentInvoice.Subtotal,
                    GST = currentInvoice.GST,
                    Total = currentInvoice.Total,
                    InvoiceDetails = list
                };

                foreach (var item in currentInvoice.Products)
                {
                    StoreRefund refund = new StoreRefund
                    {
                        ProductID = item.ProductID,
                        OriginalInvoiceID = item.InvoiceID,
                        Reason = item.RefundReason,
                        OriginalInvoice = originalInvoice,
                        RefundInvoice = refundInvoice
                    };
                    context.StoreRefunds.Add(refund);
                };
                context.SaveChanges();
                return refundInvoice.InvoiceID;
            }
            //throw new NotImplementedException("Refund not implemented.");
        }
    }
}
