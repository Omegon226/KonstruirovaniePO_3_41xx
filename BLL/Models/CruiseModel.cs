using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EFClasses;

namespace BLL.Models
{
    public class CruiseModel
    {
        public int ID { get; set; }
        public int DayOfTheWeekCruiseID { get; set; }
        public int RouteIDOfTheCruise { get; set; }
        public int DriverIDOfTheCruise { get; set; }
        public int TransportIDOfTheCruise { get; set; }
        public TimeSpan? StartTime { get; set; }

        public CruiseModel()
        { 
        
        }
        public CruiseModel(Cruise DALinfo)
        {
            this.ID = DALinfo.ID;
            this.DayOfTheWeekCruiseID = DALinfo.DayOfTheWeekCruiseID;
            this.RouteIDOfTheCruise = DALinfo.RouteIDOfTheCruise;
            this.DriverIDOfTheCruise = DALinfo.DriverIDOfTheCruise;
            this.TransportIDOfTheCruise = DALinfo.TransportIDOfTheCruise;
            this.StartTime = DALinfo.StartTime;
        }
    }
}