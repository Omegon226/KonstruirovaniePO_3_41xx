using DAL.EFClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{

    public class FindeRoute
    {
        public class StoredProcedureResult1
        {
            public int ID { get; set; }
            public int StopRouteID { get; set; }
            public int? TravelTimeInHours { get; set; }
            public bool? Hidden { get; set; }
        }

        public class StoredProcedureResult2
        {
            public int StopRouteID { get; set; }
            public int? TravelTimeInHours { get; set; }
            public bool? Hidden { get; set; }
        }

        public static List<StoredProcedureResult1> StoredProcedureExecute1(int StartPoint, int EndPoint)
        {
            System.Data.SqlClient.SqlParameter param1 = new System.Data.SqlClient.SqlParameter("@FirstLocalityName", StartPoint);
            System.Data.SqlClient.SqlParameter param2 = new System.Data.SqlClient.SqlParameter("@LastLocalityName", EndPoint);
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult1>("SP_FindeRoutesAndStoppingSequencesIDByTwoLocations @LastLocalityName,@FirstLocalityName", new object[] { param2, param1 }).ToList();

            var data = result.GroupBy(i => new { i.ID, i.StopRouteID, i.TravelTimeInHours, i.Hidden })
                .Select(i => new StoredProcedureResult1
                {
                    ID = i.Key.ID,
                    StopRouteID = i.Key.StopRouteID,
                    TravelTimeInHours = i.Key.TravelTimeInHours,
                    Hidden = i.Key.Hidden
                }).ToList();

            return (data);

        }

        public static List<StoredProcedureResult2> StoredProcedureExecute2(int EndPoint)
        {
            System.Data.SqlClient.SqlParameter param1 = new System.Data.SqlClient.SqlParameter("@LastStopID", EndPoint);
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult2>("SP_FindeRouteByFinalStopID @LastStopID", new object[] { param1 }).ToList();

            var data = result.GroupBy(i => new { i.StopRouteID, i.TravelTimeInHours, i.Hidden })
                .Select(i => new StoredProcedureResult2
                {
                    StopRouteID = i.Key.StopRouteID,
                    TravelTimeInHours = i.Key.TravelTimeInHours,
                    Hidden = i.Key.Hidden
                }).ToList();
            return (data);

        }
    }
}
