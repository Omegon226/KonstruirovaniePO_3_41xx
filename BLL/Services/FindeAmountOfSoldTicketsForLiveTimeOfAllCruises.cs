using DAL.EFClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL.Services
{
    public class FindeAmountOfSoldTicketsForLiveTimeOfAllCruises
    {
        public class StoredProcedureResult
        {
            public int CruiseID { get; set; }
            public int AmountOfSaldTickets { get; set; }
        }

        public static List<StoredProcedureResult> StoredProcedureExecute()
        {
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_FindeAmountOfSoldTicketsForLiveTimeOfAllCruises").ToList();

            var data = result.GroupBy(i => new { i.CruiseID, i.AmountOfSaldTickets })
                .Select(i => new StoredProcedureResult
                {
                    CruiseID = i.Key.CruiseID,
                    AmountOfSaldTickets = i.Key.AmountOfSaldTickets
                }).ToList();

            return (data);
        }
    }
}
