using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using DAL.EFClasses;
using DAL;

namespace BLL.DBInteraction
{
    public class DayOfTheWeekOperations
    {
        private AvtovokzalDBContext DBContext;

        public DayOfTheWeekOperations()
        {
            DBContext = new AvtovokzalDBContext();
        }
        public DayOfTheWeekOperations(AvtovokzalDBContext DBContextFromMain)
        {
            DBContext = DBContextFromMain;
        }

        public bool Save()
        {
            if (DBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public void Create(DayOfTheWeekModel RowToCreate)
        {
            DBContext.DayOfTheWeek.Add(new DayOfTheWeek()
            {
                DayOfTheWeekName = RowToCreate.DayOfTheWeekName
            });
            Save();
        }

        public void Delete(int ID)
        {
            DayOfTheWeek RowToDelete = DBContext.DayOfTheWeek.Find(ID);
            if (RowToDelete != null)
            {
                DBContext.DayOfTheWeek.Remove(RowToDelete);
                Save();
            }
        }

        public void Update(DayOfTheWeekModel RowToUpdate)
        {
            DayOfTheWeek DALRawToUpdate = DBContext.DayOfTheWeek.Find(RowToUpdate.ID);
            DALRawToUpdate.DayOfTheWeekName = RowToUpdate.DayOfTheWeekName;
        }

        public List<DayOfTheWeekModel> GetAll()
        {
            return (DBContext.DayOfTheWeek.ToList().Select(i => new DayOfTheWeekModel(i)).ToList());
        }

        public DayOfTheWeekModel GetCruise(int ID)
        {
            return new DayOfTheWeekModel(DBContext.DayOfTheWeek.Find(ID));
        }
    }
}
