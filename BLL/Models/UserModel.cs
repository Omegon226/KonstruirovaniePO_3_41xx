using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EFClasses;

namespace BLL.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? Status { get; set; }

        public UserModel()
        {

        }
        public UserModel(User DALinfo)
        {
            this.ID = DALinfo.ID;
            this.FullName = DALinfo.FullName;
            this.Login = DALinfo.Login;
            this.Password = DALinfo.Password;
            this.Status = DALinfo.Status;
        }
    }
}