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
    public class DriverOperations
    {
        private AvtovokzalDBContext DBContext;

        public DriverOperations()
        {
            DBContext = new AvtovokzalDBContext();
        }
        public DriverOperations(AvtovokzalDBContext DBContextFromMain)
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

        public void Create(DriverModel RowToCreate)
        {
            DBContext.Driver.Add(new Driver()
            {
                FullName = RowToCreate.FullName,
                Experience = RowToCreate.Experience,
                Salary = RowToCreate.Salary,
                Hidden = RowToCreate.Hidden
            });
            Save();
        }

        public void Delete(int ID)
        {
            Driver RowToDelete = DBContext.Driver.Find(ID);
            if (RowToDelete != null)
            {
                DBContext.Driver.Remove(RowToDelete);
                Save();
            }
        }

        public void Update(DriverModel RowToUpdate)
        {
            Driver DALRawToUpdate = DBContext.Driver.Find(RowToUpdate.ID);
            DALRawToUpdate.FullName = RowToUpdate.FullName;
            DALRawToUpdate.Experience = RowToUpdate.Experience;
            DALRawToUpdate.Salary = RowToUpdate.Salary;
            DALRawToUpdate.Hidden = RowToUpdate.Hidden;
            Save();
        }

        public List<DriverModel> GetAll()
        {
            return (DBContext.Driver.ToList().Select(i => new DriverModel(i)).ToList());
        }

        public DriverModel GetCruise(int ID)
        {
            return new DriverModel(DBContext.Driver.Find(ID));
        }
    }
}
