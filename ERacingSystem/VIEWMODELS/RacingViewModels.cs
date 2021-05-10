using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERacingSystem.VIEWMODELS
{
    public class RosterViewModel
    {
        public int RaceDetailID { get; set; }
        public int RaceID { get; set; }
        public int MemberID { get; set; }
        public string Name { get; set; }
        public string RaceDetailComment { get; set; }
        public decimal RaceFee { get; set; }
        public decimal RentalFee { get; set; }
        public bool Refund { get; set; }
        public string RefundReason { get; set; }
        public int? CarClassID { get; set; }
        public string CarClassName { get; set; }
        public int? CarID { get; set; }
        public string SerialNumber { get; set; }
        public int? Place { get; set; }
    }

    public class ResultsViewModel
    {
        public int RaceDetailID { get; set; }
        public int RaceID { get; set; }
        public string Name { get; set; }
        public int? Place { get; set; }
        public TimeSpan? RunTime { get; set; }
        public int? PenaltyID { get; set; }
    }

    public class ScheduleViewModel
    {
        public int RaceID { get; set; }
        public DateTime Time { get; set; }
        public int Drivers { get; set; }
        public string Run { get; set; }
        public string Competition { get; set; }
    }

    #region DropDowns
    public class PenaltiesViewModel
    {
        public int PenaltyID { get; set; }
        public string Description { get; set; }
    }

    public class CarClassViewModel
    {
        public int CarClassID { get; set; }
        public string CarClassName { get; set; }
    }

    public class CarViewModel
    {
        public int CarID { get; set; }
        public string SerialNumber { get; set; }
    }

    public class MemberViewModel
    {
        public int MemberID { get; set; }
        public string MemberName { get; set; }
    }

    public class FeesViewModel
    {
        public int RaceFeeID { get; set; }
        public decimal RaceFee { get; set; }
    }

    public class RacerCountViewModel
    {
        public int Max { get; set; }
        public int Current { get; set; }
    }

    //public class InvoiceViewModel
    //{
    //    public int InvoiceID { get; set; }
    //    public int EmployeeID { get; set; }
    //    public decimal Subtotal { get; set; }
    //    public decimal GST { get; set; }
    //    public decimal Total { get; set; }
    //}
    #endregion
}
