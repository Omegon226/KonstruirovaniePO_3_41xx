using DAL.EFClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FindeAditionalInformationForCruise
    {
        public class StoredProcedureResult
        {
            public int StopRouteID { get; set; }
            public double? PriceOfCruise { get; set; }
            public int? TravelTimeInHours { get; set; }
        }

        public static List<StoredProcedureResult> StoredProcedureExecute(int RouteID, int? StartLocalityIndex, int? EndLocalityIndex, int AdditionalPriceForCruise)
        {
            System.Data.SqlClient.SqlParameter paramRouteID = new System.Data.SqlClient.SqlParameter("@RouteID", RouteID);
            System.Data.SqlClient.SqlParameter paramStartLocalityIndex = new System.Data.SqlClient.SqlParameter("@StartLocalityIndex", StartLocalityIndex);
            System.Data.SqlClient.SqlParameter paramEndLocalityIndex = new System.Data.SqlClient.SqlParameter("@EndLocalityIndex", EndLocalityIndex);
            System.Data.SqlClient.SqlParameter paramAdditionalPriceForCruise = new System.Data.SqlClient.SqlParameter("@AdditionalPriceForCruise", AdditionalPriceForCruise);
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_FindeFullPriceAndTravelTimeInHoursHoCruise @RouteID,@StartLocalityIndex,@EndLocalityIndex,@AdditionalPriceForCruise", 
                new object[] { paramRouteID, paramStartLocalityIndex, paramEndLocalityIndex, paramAdditionalPriceForCruise }).ToList();

            var data = result.GroupBy(i => new { i.StopRouteID, i.PriceOfCruise, i.TravelTimeInHours})
                .Select(i => new StoredProcedureResult
                {
                    StopRouteID = i.Key.StopRouteID,
                    PriceOfCruise = i.Key.PriceOfCruise,
                    TravelTimeInHours = i.Key.TravelTimeInHours
                }).ToList();

            return (data);

        }
    }
}
