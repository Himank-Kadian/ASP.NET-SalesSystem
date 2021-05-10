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
    public class RacingController
    {
        #region Queries
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ScheduleViewModel> Schedules_List(DateTime day)
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.Races
                              where x.RaceDate.Month == day.Month && x.RaceDate.Year == day.Year && x.RaceDate.Day == day.Day
                              select new ScheduleViewModel
                              {
                                  RaceID = x.RaceID,
                                  Time = x.RaceDate,
                                  Competition = (x.Certification.Description + " - " + x.Comment),
                                  Run = x.Run,
                                  Drivers = x.RaceDetails.Count()
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DateTime> DateList()
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.Races
                               select x.RaceDate;
                return results.ToList();
            }
            
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RosterViewModel> Roster_List(int raceId)
        {
            List<RosterViewModel> result;
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.RaceDetails
                              where x.RaceID == raceId
                              select new RosterViewModel
                              {
                                  RaceDetailID = x.RaceDetailID,
                                  RaceID = x.RaceID,
                                  MemberID = x.MemberID,
                                  Name = (x.Member.FirstName + " " + x.Member.LastName),
                                  RaceDetailComment = x.Comment,
                                  RaceFee = x.RaceFee,
                                  RentalFee = x.RentalFee,
                                  Refund = x.Refund,
                                  RefundReason = x.RefundReason,
                                  CarClassID = x.Car.CarClassID,
                                  CarClassName = x.Car.CarClass.CarClassName,
                                  CarID = x.Car.CarID,
                                  SerialNumber = x.Car.SerialNumber,
                                  Place = x.Place
                              };
                result = results.ToList();
            }
                
                if (result.Count < 1)
                {
                    RosterViewModel blank = new RosterViewModel()
                    {
                        RaceDetailID = 0,
                        RaceID = raceId,
                        MemberID = 0,
                        Name = " ",
                        RaceDetailComment = " ",
                        RaceFee = 0,
                        RentalFee = 0,
                        Refund = false,
                        RefundReason = " "
                    };
                    result.Add(blank);
                }
                return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<PenaltiesViewModel> Penalties_List()
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.RacePenalties
                              select new PenaltiesViewModel
                              {
                                  PenaltyID = x.PenaltyID,
                                  Description = x.Description
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ResultsViewModel> Results_List(int raceId)
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.RaceDetails
                              where x.RaceID == raceId
                              select new ResultsViewModel
                              {
                                  RaceDetailID = x.RaceDetailID,
                                  RaceID = x.RaceID,
                                  Name = (x.Member.FirstName + " " + x.Member.LastName),
                                  Place = x.Place,
                                  RunTime = x.RunTime,
                                  PenaltyID = x.PenaltyID
                              };
                return results.ToList();
            }

        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CarClassViewModel> CarClasses_List(int raceId)
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.CarClasses
                              where x.CertificationLevel == (from y in context.Races where y.RaceID == raceId select y.CertificationLevel).FirstOrDefault()
                              select new CarClassViewModel
                              {
                                  CarClassID = x.CarClassID,
                                  CarClassName = x.CarClassName
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CarViewModel> Cars_List(int classID, int raceID, int currentCarID)
        {
            List<CarViewModel> finalCarList = null;
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.Cars
                              where x.CarClassID == classID
                              select new CarViewModel
                              {
                                  CarID = x.CarID,
                                  SerialNumber = x.SerialNumber
                              };
                var reject = (from x in context.RaceDetails
                              where x.RaceID == raceID
                              select new CarViewModel
                              {
                                  CarID = ((int)x.CarID),
                                  SerialNumber = (from y in context.Cars where y.CarID == x.CarID select y.SerialNumber).FirstOrDefault()
                              });
                var filteredList = results.Except(reject);

                finalCarList = filteredList.ToList();
                if (currentCarID != 0)
                {
                    var currentCar = (from x in context.Cars
                                      where x.CarID == currentCarID
                                      select new CarViewModel
                                      {
                                          CarID = currentCarID,
                                          SerialNumber = x.SerialNumber
                                      }).FirstOrDefault();                    
                    finalCarList.Insert(0, currentCar);
                } 
                
                   
                
                return finalCarList;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<MemberViewModel> Members_List(int raceID)
        {
            using (var context = new ERaceingSystemContext())
            {
                var results = from x in context.Members
                              where x.CertificationLevel == (from y in context.Races where y.RaceID == raceID select y.CertificationLevel).FirstOrDefault()
                              select new MemberViewModel
                              {
                                  MemberID = x.MemberID,
                                  MemberName = (x.FirstName + " " + x.LastName)
                              };
                var reject = from x in context.RaceDetails
                             where x.RaceID == raceID
                             select new MemberViewModel
                             {
                                 MemberID = x.MemberID,
                                 MemberName = (x.Member.FirstName + " " + x.Member.LastName)
                             };

                var filteredList = results.Except(reject);
                return filteredList.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<FeesViewModel> Fees_List()
        {
            using (var context = new ERaceingSystemContext()){
                var results = from x in context.RaceFees                              
                              select new FeesViewModel
                              {
                                  RaceFeeID = x.RaceFeeID,
                                  RaceFee = x.Fee
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RacerCountViewModel Racer_Count(RosterViewModel roster)
        {
            using (var context = new ERaceingSystemContext())
            {

            var maxRacers = (from x in context.Races
                             where x.RaceID == roster.RaceID
                             select new RacerCountViewModel
                             {
                                 Max = x.NumberOfCars,
                                 Current = (x.RaceDetails).Count()
                             }).FirstOrDefault();
                return maxRacers;
            }
        }
        #endregion

        #region Commands

        #region Add Racer
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public InvoiceViewModel Add_Racer(RosterViewModel roster, string employeeName)
        {
            using (var context = new ERaceingSystemContext())
            {                
                decimal rentalFee = (from x in context.Cars
                                   where x.CarID == roster.CarID
                                   select x.CarClass.RaceRentalFee).FirstOrDefault();
                decimal subTotal = roster.RaceFee + rentalFee;
                decimal GST = decimal.Multiply(subTotal, (decimal)0.05);

                int? employeeID = (from x in context.AspNetUsers
                                   where x.UserName == employeeName
                                   select x.EmployeeId).FirstOrDefault();
                Invoice invoice = new Invoice()
                {
                    EmployeeID = (int)employeeID,
                    InvoiceDate = DateTime.Now,
                    SubTotal = subTotal,
                    GST = GST,
                    Total = decimal.Add(subTotal, GST)
                };
                context.Invoices.Add(invoice);
             
                    RaceDetail info = new RaceDetail()
                    {
                        InvoiceID = invoice.InvoiceID,
                        RaceID = roster.RaceID,
                        MemberID = roster.MemberID,
                        RaceFee = roster.RaceFee,
                        RentalFee = rentalFee,
                    };
                    if (roster.CarID != 0)
                    {
                        var fee = (from x in context.Cars
                                   where x.CarID == roster.CarID
                                   select x.CarClass.RaceRentalFee).FirstOrDefault();
                        info.RentalFee = fee;
                        info.CarID = roster.CarID;
                    }
                    context.RaceDetails.Add(info);
                                
                    context.SaveChanges();
                    InvoiceViewModel createdInvoice = new InvoiceViewModel()
                    {
                        InvoiceID = invoice.InvoiceID,
                        EmployeeID = (int)employeeID,
                        Subtotal = invoice.SubTotal,
                        GST = invoice.GST,
                        Total = decimal.Add(invoice.SubTotal, invoice.GST)
                    };
                return createdInvoice;                
            }
        }
        #endregion

        #region Edit Results
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Edit_Results(List<ResultsViewModel> results)
        {
            int raceId = results[0].RaceID;
            using (var context = new ERaceingSystemContext())
            {
                List<RaceDetail> updatedResults = (from x in context.RaceDetails
                                                   where x.RaceID == raceId
                                                   select x).ToList();
                for (var index = 0; index < results.Count; index++)
                {
                    if (results[index].PenaltyID > 0)
                    {
                        if (results[index].PenaltyID == 4)
                        {
                            updatedResults[index].RunTime = null;
                        }
                    }
                    if (results[index].PenaltyID != 4)
                    {
                        updatedResults[index].RunTime = results[index].RunTime;
                    }

                    updatedResults[index].PenaltyID = results[index].PenaltyID;
                    if (results[index].PenaltyID == 0)
                    {
                        updatedResults[index].PenaltyID = null;
                    }
                }
                List<RaceDetail> placedResults = updatedResults.OrderBy(e => e.RunTime).ToList();

                int place = 1;
                foreach (RaceDetail result in placedResults)
                {
                    if (result.PenaltyID != 4)
                    {
                        result.Place = place;
                        place++;
                    } else
                    {
                        result.Place = null;
                    }
                }
            
                context.SaveChanges();          
            }
        }

        #endregion

        #region Edit Roster

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Edit_Roster(RosterViewModel roster)
        {
            using (var context = new ERaceingSystemContext())
            {
                RaceDetail oldRaceDetail = (from x in context.RaceDetails
                                            where x.RaceDetailID == roster.RaceDetailID
                                            select x).FirstOrDefault();
                if (roster.CarID != 0)
                {
                    var fee = (from x in context.Cars
                               where x.CarID == roster.CarID
                               select x.CarClass.RaceRentalFee).SingleOrDefault();
                    oldRaceDetail.RentalFee = fee;
                }
                
                oldRaceDetail.CarID = roster.CarID;                
                oldRaceDetail.Comment = roster.RaceDetailComment;
                oldRaceDetail.RefundReason = roster.RefundReason;
                oldRaceDetail.Refund = roster.Refund;
                
                context.SaveChanges();
            }
        }
        #endregion

        #endregion

    }
}
