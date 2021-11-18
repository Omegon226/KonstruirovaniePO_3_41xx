using DAL.EFClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FindeAmountOfStoppingOnTheRoute
    {
        public class StoredProcedureResult
        {
            public int RouteID { get; set; }
            public int AmountOfStoppings { get; set; }
        }

        public static List<StoredProcedureResult> StoredProcedureExecute()
        {
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_FindeAmountOfStoppingOnTheRoute").ToList();

            var data = result.GroupBy(i => new { i.RouteID, i.AmountOfStoppings })
                .Select(i => new StoredProcedureResult
                {
                    RouteID = i.Key.RouteID,
                    AmountOfStoppings = i.Key.AmountOfStoppings
                }).ToList();

            return (data);
        }
    }
}
