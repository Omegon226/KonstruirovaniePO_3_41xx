using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class PossibleCruises
    {
        public int CruiseID { get; set; }
        public int DayOfTheWeekCruiseID { get; set; }
        public int RouteIDOfTheCruise { get; set; }
        public int DriverIDOfTheCruise { get; set; }
        public int TransportIDOfTheCruise { get; set; }
        public TimeSpan? StartTime { get; set; }

        public int? StartPointIndex { get; set; }
        public int StartPointStoppingID { get; set; }
        public int StartPointLocalityID { get; set; }
        public string StartPointLocalityName { get; set; }
        public int? EndPointIndex { get; set; }
        public int EndPointStoppingID { get; set; }
        public int EndPointLocalityID { get; set; }
        public string EndPointLocalityName { get; set; }

        public double? FullPrice;
        public int? FullTimeInCruise;
        public TransportModel TransportOfTheCruise;

        public PossibleCruises(CruiseModel CruiseInfo, FindeRouteForCruises.FinalResult RouteInfo)
        {
            this.CruiseID = CruiseInfo.ID;
            this.DayOfTheWeekCruiseID = CruiseInfo.DayOfTheWeekCruiseID;
            this.RouteIDOfTheCruise = CruiseInfo.RouteIDOfTheCruise;
            this.DriverIDOfTheCruise = CruiseInfo.DriverIDOfTheCruise;
            this.TransportIDOfTheCruise = CruiseInfo.TransportIDOfTheCruise;
            this.StartTime = CruiseInfo.StartTime;

            this.StartPointIndex = RouteInfo.StartPointIndex;
            this.StartPointStoppingID = RouteInfo.StartPointStoppingID;
            this.StartPointLocalityID = RouteInfo.StartPointLocalityID;
            this.StartPointLocalityName = RouteInfo.StartPointLocalityName;
            this.EndPointIndex = RouteInfo.EndPointIndex;
            this.EndPointStoppingID = RouteInfo.EndPointStoppingID;
            this.EndPointLocalityID = RouteInfo.EndPointLocalityID;
            this.EndPointLocalityName = RouteInfo.EndPointLocalityName;
        }

        public void AddPriceAndTimeInfo(FindeAditionalInformationForCruise.StoredProcedureResult AdditionalInfoForCruises)
        {
            this.FullPrice = AdditionalInfoForCruises.PriceOfCruise;
            this.FullTimeInCruise = AdditionalInfoForCruises.TravelTimeInHours;
        }
    }
}
