using BLL.Models;
using DAL.EFClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FindeCruise
    {
        public class StoredProcedureResult
        {
            public int ID { get; set; }
            public int DayOfTheWeekCruiseID { get; set; }
            public int RouteIDOfTheCruise { get; set; }
            public int DriverIDOfTheCruise { get; set; }
            public int TransportIDOfTheCruise { get; set; }
            public TimeSpan? StartTime { get; set; }
        }

        public static List<StoredProcedureResult> StoredProcedureExecute(int Time)
        {
            System.Data.SqlClient.SqlParameter param1 = new System.Data.SqlClient.SqlParameter("@TravelTimeRoad", Time);
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_FindeCruiseUsingTravelTimeRoad @TravelTimeRoad", new object[] { param1 }).ToList();

            var data = result.GroupBy(i => new { i.ID, i.DayOfTheWeekCruiseID, i.RouteIDOfTheCruise, i.DriverIDOfTheCruise, i.TransportIDOfTheCruise, i.StartTime })
                .Select(i => new StoredProcedureResult
                {
                    ID = i.Key.ID,
                    DayOfTheWeekCruiseID = i.Key.DayOfTheWeekCruiseID,
                    RouteIDOfTheCruise = i.Key.RouteIDOfTheCruise,
                    DriverIDOfTheCruise = i.Key.DriverIDOfTheCruise,
                    TransportIDOfTheCruise = i.Key.TransportIDOfTheCruise,
                    StartTime = i.Key.StartTime
                }).ToList();
            return (data);

        }
    }
}
