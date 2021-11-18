using DAL.EFClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FindeDriver
    {
        public class StoredProcedureResult
        {
            public string FullName { get; set; }
            public int Experience { get; set; }
            public int Salary { get; set; }
        }

        public static List<StoredProcedureResult> StoredProcedureExecute()
        {
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_GetDriversInformationForChart").ToList();

            var data = result.GroupBy(i => new { i.FullName, i.Experience, i.Salary })
                .Select(i => new StoredProcedureResult
                {
                    FullName = i.Key.FullName,
                    Experience = i.Key.Experience,
                    Salary = i.Key.Salary,
                }).ToList();

            return (data);

        }
    }
}
