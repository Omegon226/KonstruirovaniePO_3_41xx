using DAL.EFClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FindeAmountOfCruiseOnTheRoute
    {
        public class StoredProcedureResult
        {
            public int IDOfRoute { get; set; }
            public int AmountOfCruises { get; set; }
        }

        public static List<StoredProcedureResult> StoredProcedureExecute()
        {
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_AmountOfCruiseOnTheRoute").ToList();

            var data = result.GroupBy(i => new { i.IDOfRoute , i.AmountOfCruises })
                .Select(i => new StoredProcedureResult
                {
                    IDOfRoute = i.Key.IDOfRoute,
                    AmountOfCruises = i.Key.AmountOfCruises
                }).ToList();

            return (data);
        }
    }
}
