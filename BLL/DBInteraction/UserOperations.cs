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
    public class UserOperations
    {
        private AvtovokzalDBContext DBContext;

        public UserOperations()
        {
            DBContext = new AvtovokzalDBContext();
        }
        public UserOperations(AvtovokzalDBContext DBContextFromMain)
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

        public void Create(UserModel RowToCreate)
        {
            DBContext.User.Add(new User()
            {
                FullName = RowToCreate.FullName,
                Login = RowToCreate.Login,
                Password = RowToCreate.Password,
                Status = RowToCreate.Status
            });
            Save();
        }

        public void Delete(int ID)
        {
            User RowToDelete = DBContext.User.Find(ID);
            if (RowToDelete != null)
            {
                DBContext.User.Remove(RowToDelete);
                Save();
            }
        }

        public void Update(UserModel RowToUpdate)
        {
            User DALRawToUpdate = DBContext.User.Find(RowToUpdate.ID);
            DALRawToUpdate.FullName = RowToUpdate.FullName;
            DALRawToUpdate.Login = RowToUpdate.Login;
            DALRawToUpdate.Password = RowToUpdate.Password;
            DALRawToUpdate.Status = RowToUpdate.Status;
        }

        public List<UserModel> GetAll()
        {
            return (DBContext.User.ToList().Select(i => new UserModel(i)).ToList());
        }

        public UserModel GetCruise(int ID)
        {
            return new UserModel(DBContext.User.Find(ID));
        }
    }
}
