using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EFClasses;

namespace BLL.Models
{
    public class  TicketModel
    {
        public int ID { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public string IdentificationInformation { get; set; }
        public int? SeatNumberOnTheTransport { get; set; }
        public string FullName { get; set; }
        public int CruiseID { get; set; }
        public int UserID { get; set; }
        public DateTime? RaceDepartureTime { get; set; }

        public TicketModel()
        {

        }
        public TicketModel(Ticket DALinfo)
        {
            this.ID = DALinfo.ID;
            this.DateOfIssue = DALinfo.DateOfIssue;
            this.IdentificationInformation = DALinfo.IdentificationInformation;
            this.SeatNumberOnTheTransport = DALinfo.SeatNumberOnTheTransport;
            this.FullName = DALinfo.FullName;
            this.CruiseID = DALinfo.CruiseID;
            this.UserID = DALinfo.UserID;
            this.RaceDepartureTime = DALinfo.RaceDepartureTime;
        }
    }
}