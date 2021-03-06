using DAL.EFClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FindeUsersStatusesCount
    {
        public class StoredProcedureResult
        {
            public int IDOfStatus { get; set; }
            public int Status { get; set; }
        }

        public static List<StoredProcedureResult> StoredProcedureExecute()
        {
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_GetUsersStatusCount").ToList();

            var data = result.GroupBy(i => new { i.IDOfStatus, i.Status })
                .Select(i => new StoredProcedureResult
                {
                    IDOfStatus = i.Key.IDOfStatus,
                    Status = i.Key.Status
                }).ToList();

            return (data);
        }
    }
}
