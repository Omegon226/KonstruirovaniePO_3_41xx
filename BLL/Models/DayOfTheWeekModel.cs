using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EFClasses;

namespace BLL.Models
{
    public class DayOfTheWeekModel
    {
        public int ID { get; set; }
        public string DayOfTheWeekName { get; set; }

        public DayOfTheWeekModel()
        {
            
        }
        public DayOfTheWeekModel(DayOfTheWeek DALinfo)
        {
            this.ID = DALinfo.ID;
            this.DayOfTheWeekName = DALinfo.DayOfTheWeekName;
        }
    }
}