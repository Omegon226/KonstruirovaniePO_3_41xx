using BLL.DBInteraction;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Интерфейс.CeateUpdateWindows;

namespace Интерфейс.CRUDClasses
{
    public class CRUDLogicForUser
    {
        DBDataOperations DBComunication;

        public CRUDLogicForUser()
        {

        }
        public CRUDLogicForUser(DBDataOperations DBComunicationFromWindow)
        {
            DBComunication = DBComunicationFromWindow;
        }

        public UserModel Create()
        {
            UserCUWindow CreateWindow = new UserCUWindow();

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                UserModel NewObject = new UserModel();

                NewObject.FullName = CreateWindow.SurnameTextBox.Text + " " + CreateWindow.NameTextBox.Text + " " + CreateWindow.PatronymicTextBox.Text;
                NewObject.Login = CreateWindow.LoginTextBox.Text;
                NewObject.Password = CreateWindow.PasswordTextBox.Text;
                if (CreateWindow.StatusUserRadioButton.IsChecked == true)
                {
                    NewObject.Status = 1;
                }
                if (CreateWindow.StatusAdministratorRadioButton.IsChecked == true)
                {
                    NewObject.Status = 2;
                }

                return (NewObject);
            }
        }
        public UserModel Update(UserModel ph)
        {
            UserCUWindow UpdateWindow = new UserCUWindow();

            string[] FullName = ph.FullName.Split(' ');

            if (FullName.Length == 1)
            {
                UpdateWindow.SurnameTextBox.Text = FullName[0];
            }
            if (FullName.Length == 2)
            {
                UpdateWindow.SurnameTextBox.Text = FullName[0];
                UpdateWindow.NameTextBox.Text = FullName[1];
            }
            if (FullName.Length == 3)
            {
                UpdateWindow.SurnameTextBox.Text = FullName[0];
                UpdateWindow.NameTextBox.Text = FullName[1];
                UpdateWindow.PatronymicTextBox.Text = FullName[2];
            }

            UpdateWindow.LoginTextBox.Text = ph.Login;
            UpdateWindow.PasswordTextBox.Text = ph.Password;

            if (ph.Status == 1)
            {
                UpdateWindow.StatusUserRadioButton.IsChecked = true;
                UpdateWindow.StatusAdministratorRadioButton.IsChecked = false;
            }
            if (ph.Status == 2)
            {
                UpdateWindow.StatusUserRadioButton.IsChecked = false;
                UpdateWindow.StatusAdministratorRadioButton.IsChecked = true;
            }

            bool? result = UpdateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                ph.FullName = UpdateWindow.SurnameTextBox.Text + " " + UpdateWindow.NameTextBox.Text + " " + UpdateWindow.PatronymicTextBox.Text;
                ph.Login = UpdateWindow.LoginTextBox.Text;
                ph.Password = UpdateWindow.PasswordTextBox.Text;
                if (UpdateWindow.StatusUserRadioButton.IsChecked == true)
                {
                    ph.Status = 1;
                }
                if (UpdateWindow.StatusAdministratorRadioButton.IsChecked == true)
                {
                    ph.Status = 2;
                }

                return (ph);
            }
        }
    }
}


