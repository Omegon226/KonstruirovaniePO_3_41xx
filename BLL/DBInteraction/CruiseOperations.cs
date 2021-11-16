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
    public class CruiseOperations
    {
        private AvtovokzalDBContext DBContext;

        public CruiseOperations()
        {
            DBContext = new AvtovokzalDBContext();
        }
        public CruiseOperations(AvtovokzalDBContext DBContextFromMain)
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

        public void Create(CruiseModel RowToCreate)
        {
            DBContext.Cruise.Add(new Cruise()
            {
                DayOfTheWeekCruiseID = RowToCreate.DayOfTheWeekCruiseID,
                RouteIDOfTheCruise = RowToCreate.RouteIDOfTheCruise,
                DriverIDOfTheCruise = RowToCreate.DriverIDOfTheCruise,
                TransportIDOfTheCruise = RowToCreate.TransportIDOfTheCruise,
                StartTime = RowToCreate.StartTime
            });
            Save();
        }

        public void Delete(int ID)
        {
            Cruise RowToDelete = DBContext.Cruise.Find(ID);
            if (RowToDelete != null)
            {
                DBContext.Cruise.Remove(RowToDelete);
                Save();
            }
        }

        public void Update(CruiseModel RowToUpdate)
        {
            Cruise CruiseToUpdate = DBContext.Cruise.Find(RowToUpdate.ID);
            CruiseToUpdate.DayOfTheWeekCruiseID = RowToUpdate.DayOfTheWeekCruiseID;
            CruiseToUpdate.RouteIDOfTheCruise = RowToUpdate.RouteIDOfTheCruise;
            CruiseToUpdate.DriverIDOfTheCruise = RowToUpdate.DriverIDOfTheCruise;
            CruiseToUpdate.TransportIDOfTheCruise = RowToUpdate.TransportIDOfTheCruise;
            CruiseToUpdate.StartTime = RowToUpdate.StartTime;
            Save();
        }

        public List<CruiseModel> GetAll()
        {
            return (DBContext.Cruise.ToList().Select(i => new CruiseModel(i)).ToList());
        }

        public CruiseModel GetCruise(int ID)
        {
            return new CruiseModel(DBContext.Cruise.Find(ID));
        }
    }
}