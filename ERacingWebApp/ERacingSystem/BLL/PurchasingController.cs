using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using ERacingSystem.DAL;
using ERacingSystem.VIEWMODELS;
using ERacingSystem.ENTITIES;

namespace ERacingSystem.BLL
{
    [DataObject]
    public class PurchasingController
    {
        #region Queries
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<VendorViewModel> ListVendors()
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.Vendors
                              select new VendorViewModel
                              {
                                  VendorID = x.VendorID,
                                  VendorName = x.Name,
                                  Contact = x.Contact,
                                  Phone = x.Phone
                              };
                return results.ToList();
            }
        }
        public VendorViewModel GetVendorByID(int vendorID)
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.Vendors
                              where x.VendorID == vendorID
                              select new VendorViewModel

                              {
                                  VendorID = x.VendorID,
                                  VendorName = x.Name,
                                  Contact = x.Contact,
                                  Phone = x.Phone
                              };
                return results.FirstOrDefault();
            }
        }


        public List<OrderDetailsViewModel> GetOrderDetails(int vendorID)
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.OrderDetails
                              where x.Order.VendorID.Equals(vendorID) && x.Order.OrderNumber.Equals(null)
                              select new OrderDetailsViewModel
                              {
                                  OrderDetailID = x.OrderDetailID,
                                  ProductId = x.ProductID,
                                  ItemName = x.Product.ItemName,
                                  Quantity = x.Quantity,
                                  OrderUnitSize = x.OrderUnitSize,
                                  UnitCost = Decimal.Round(x.Cost,2),
                                  ItemCost = (x.Cost / x.OrderUnitSize),
                                  ExtendedCost = (x.Cost * x.Quantity)
                              };
                return results.ToList();
            }
        }
        public OrderViewModel GetActiveOrder(int vendorID)
        {
            var order = new OrderViewModel();
            if (CheckIfExistingActiveOrder(vendorID))
            {
                order = GetOrder(vendorID);
            } else
            {
                order = Create_Blank_Order();
            }
            return order;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<OrderDetailsViewModel> GetActiveOrderDetails(int vendorID)
        {
            var order = new OrderViewModel();
            var orderDetails = new List<OrderDetailsViewModel>();
            if (CheckIfExistingActiveOrder(vendorID))
            {
                orderDetails = GetOrderDetails(vendorID);
            }
            else
            {
                order = null;
            }
            return orderDetails;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public OrderViewModel GetOrder(int vendorID)
        {
            using (var context = new ERaceingSystemContext())
            {

                var results = (from x in context.Orders
                               where x.VendorID == vendorID && x.OrderNumber == null
                               orderby x.OrderID
                               select new OrderViewModel
                               {
                                   OrderID = x.OrderID,
                                   OrderNumber = x.OrderNumber,
                                   Comment = x.Comment,
                                   SubTotal = x.SubTotal,
                                   Tax = x.TaxGST,
                                   Total = x.TaxGST + x.SubTotal
                               }).FirstOrDefault();
                return results;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<InventoryByVendorByCatagoryViewModel> ListCategoryByVendor(int vendorID)
        {
            using (var context = new ERaceingSystemContext())
            {
                IEnumerable<InventoryByVendorByCatagoryViewModel> results = null;
                results = from x in context.VendorCatalogs
                          where x.VendorID.Equals(vendorID)
                          group x by new { x.Product.Category } into categorytemp
                          select new InventoryByVendorByCatagoryViewModel
                          {

                              Description = categorytemp.Key.Category.Description,
                              Items = from y in context.VendorCatalogs
                                      where y.Product.CategoryID.Equals(categorytemp.Key.Category.CategoryID) && y.VendorID.Equals(vendorID)

                                      select new InventoryViewModel
                                      {
                                          ProductID = y.ProductID,
                                          ItemName = y.Product.ItemName,
                                          ReOrderLevel = y.Product.ReOrderLevel,
                                          QuantityOnHand = y.Product.QuantityOnHand,
                                          QuantityOnOrder = y.Product.QuantityOnOrder,
                                          OrderUnitType = y.OrderUnitType,
                                          OrderUnitSize = y.OrderUnitSize

                                      }
                          };
                return results.ToList();
            }

        }
        public decimal GetProductUnitCost(int productId)
        {
            decimal results = 0;
            using (var context = new ERaceingSystemContext())
            {
                results = (from x in context.VendorCatalogs
                           where x.ProductID == productId
                           select x.OrderUnitCost).FirstOrDefault();
            
            }
            return results;
        }
        public decimal GetProductSellingPrice(int productId)
        {
            decimal results = 0;
            using (var context = new ERaceingSystemContext())
            {
                results = (from x in context.Products
                           where x.ProductID == productId
                           select x.ItemPrice).FirstOrDefault();

            }
            return results;
        }
        #endregion
            #region Commands
            public OrderViewModel Create_Blank_Order()
        {
            OrderViewModel order = new OrderViewModel();
            order.OrderNumber = null;
            order.Comment = "";
            order.SubTotal = 0;
            order.Tax = 0;
            order.Total = 0;
            return order;
        }
        public bool CheckIfExistingActiveOrder(int vendorID)
        {
            using (var context = new ERaceingSystemContext())
            {
                var active = false;
                IEnumerable<OrderViewModel> results = null; 
                results = from x in context.Orders
                          where x.VendorID == vendorID
                          orderby x.OrderID
                          select new OrderViewModel
                          {
                              OrderID = x.OrderID,
                              OrderNumber = x.OrderNumber,
                              Comment = x.Comment,
                              SubTotal = x.SubTotal,
                              Tax = x.TaxGST,
                              Total = x.TaxGST + x.SubTotal
                          };
                foreach(OrderViewModel item in results)
                {
                    if(item.OrderNumber == null)
                    {
                        active = true;
                    }
                }
                if (active)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public void SavePurchaseOrder(int vendorId, OrderViewModel order, List<OrderDetailsViewModel> orderDetails)
        {
            Order argOrder = null;
            OrderDetail argOrderDetail = null;
            int orderId = order.OrderID;
            using (var context = new ERaceingSystemContext())
            {
 
                List<OrderDetail> orderDetailsOld = (from x in context.OrderDetails
                                                     where x.OrderID == orderId
                                                     select x).ToList();
               
                Order exists = (from x in context.Orders
                                  where x.OrderID == orderId
                                  select x).FirstOrDefault();
                if (exists == null)
                {
                    argOrder = new Order();
                    argOrder.VendorID = vendorId;
                    argOrder.EmployeeID = 20;
                    argOrder.SubTotal = order.SubTotal;
                    argOrder.TaxGST = order.Tax;
                    context.Orders.Add(argOrder);
                    context.SaveChanges();
                    orderId = argOrder.OrderID;
                }
                else
                {
                    foreach (OrderDetail item in orderDetailsOld)
                    {
                        context.OrderDetails.Remove(item);
                    }
                    argOrder = exists;
                    argOrder.EmployeeID = 20;
                    argOrder.SubTotal = order.SubTotal;
                    argOrder.TaxGST = order.Tax;
                    
                    context.SaveChanges();
                }



                foreach (OrderDetailsViewModel item in orderDetails)
                {
                    argOrderDetail = new OrderDetail();
                    argOrderDetail.OrderID = orderId;
                    argOrderDetail.ProductID = item.ProductId;
                    argOrderDetail.Quantity = item.Quantity;
                    argOrderDetail.OrderUnitSize = item.OrderUnitSize;
                    argOrderDetail.Cost = item.UnitCost;
                    context.OrderDetails.Add(argOrderDetail);
                    
                }

                context.SaveChanges();

            }
        }
        public void PlacePurchaseOrder(int vendorId, OrderViewModel order, List<OrderDetailsViewModel> orderDetails)
        {
            
            Order argOrder = null;
            OrderDetail argOrderDetail = null;
            int orderId = order.OrderID;
            using (var context = new ERaceingSystemContext())
            {
                int? orderNumber = (from x in context.Orders
                                    where x.OrderNumber != null
                                    select x.OrderNumber).Max() + 1;
                List<OrderDetail> orderDetailsOld = (from x in context.OrderDetails
                                                     where x.OrderID == orderId
                                                     select x).ToList();

                Order exists = (from x in context.Orders
                                where x.OrderID == orderId
                                select x).FirstOrDefault();
                if (exists == null)
                {
                    argOrder = new Order();
                    argOrder.VendorID = vendorId;
                    argOrder.EmployeeID = 20;
                    argOrder.SubTotal = order.SubTotal;
                    argOrder.TaxGST = order.Tax;
                    argOrder.OrderDate = DateTime.Today;
                    argOrder.OrderNumber = orderNumber;
                    context.Orders.Add(argOrder);
                    context.SaveChanges();
                    orderId = argOrder.OrderID;
                }
                else
                {
                    foreach (OrderDetail item in orderDetailsOld)
                    {
                        context.OrderDetails.Remove(item);
                    }
                    argOrder = exists;
                    argOrder.EmployeeID = 20;
                    argOrder.SubTotal = order.SubTotal;
                    argOrder.TaxGST = order.Tax;
                    argOrder.OrderDate = DateTime.Today;
                    argOrder.OrderNumber = orderNumber;
                    context.SaveChanges();
                }



                foreach (OrderDetailsViewModel item in orderDetails)
                {
                    var result = (from x in context.Products
                                  where x.ProductID == item.ProductId
                                  select x).FirstOrDefault();
                    result.QuantityOnOrder = result.QuantityOnOrder + item.Quantity;
                    argOrderDetail = new OrderDetail();
                    argOrderDetail.OrderID = orderId;
                    argOrderDetail.ProductID = item.ProductId;
                    argOrderDetail.Quantity = item.Quantity;
                    argOrderDetail.OrderUnitSize = item.OrderUnitSize;
                    argOrderDetail.Cost = item.UnitCost;
                    context.OrderDetails.Add(argOrderDetail);

                }

                context.SaveChanges();

            }
            
        }
        public void DeletePurchaseOrder(int vendorId, OrderViewModel order, List<OrderDetailsViewModel> orderDetail)
        {
            int orderId = order.OrderID;
            using (var context = new ERaceingSystemContext())
            {

                List<OrderDetail> orderDetailsOld = (from x in context.OrderDetails
                                                     where x.OrderID == order.OrderID
                                                     select x).ToList();
                foreach (OrderDetail item in orderDetailsOld)
                {
                    context.OrderDetails.Remove(item);
                }
                Order oldOrder = (from x in context.Orders
                                  where x.OrderID == order.OrderID
                                  select x).FirstOrDefault();
                if (oldOrder != null)
                {
                    context.Orders.Remove(oldOrder);
                }

                context.SaveChanges();
            }
        }
        #endregion
    }
}
