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
    public class LocalityOperations
    {
        private AvtovokzalDBContext DBContext;

        public LocalityOperations()
        {
            DBContext = new AvtovokzalDBContext();
        }
        public LocalityOperations(AvtovokzalDBContext DBContextFromMain)
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

        public void Create(LocalityModel RowToCreate)
        {
            DBContext.Locality.Add(new Locality()
            {
                Region = RowToCreate.Region,
                Name = RowToCreate.Name
            });
            Save();
        }

        public void Delete(int ID)
        {
            Locality RowToDelete = DBContext.Locality.Find(ID);
            if (RowToDelete != null)
            {
                DBContext.Locality.Remove(RowToDelete);
                Save();
            }
        }

        public void Update(LocalityModel RowToUpdate)
        {
            Locality DALRawToUpdate = DBContext.Locality.Find(RowToUpdate.ID);
            DALRawToUpdate.Region = RowToUpdate.Region;
            DALRawToUpdate.Name = RowToUpdate.Name;
        }

        public List<LocalityModel> GetAll()
        {
            return (DBContext.Locality.ToList().Select(i => new LocalityModel(i)).ToList());
        }

        public LocalityModel GetCruise(int ID)
        {
            return new LocalityModel(DBContext.Locality.Find(ID));
        }
    }
}
