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
    public class CRUDLogicForDriver
    {
        DBDataOperations DBComunication;

        public CRUDLogicForDriver()
        {

        }
        public CRUDLogicForDriver(DBDataOperations DBComunicationFromWindow)
        {
            DBComunication = DBComunicationFromWindow;
        }

        public DriverModel Create()
        {
            DriverCUWindow CreateWindow = new DriverCUWindow();

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                DriverModel NewObject = new DriverModel();

                NewObject.FullName = CreateWindow.SurnameTextBox.Text + " " + CreateWindow.NameTextBox.Text + " " + CreateWindow.PatronymicTextBox.Text;
                NewObject.Experience = Int32.Parse(CreateWindow.ExperienceTextBox.Text);
                NewObject.Salary = Int32.Parse(CreateWindow.SalaryTextBox.Text);
                if (CreateWindow.HiddenNoRadioButton.IsChecked == true)
                {
                    NewObject.Hidden = false;
                }
                if (CreateWindow.HiddenYesRadioButton.IsChecked == true)
                {
                    NewObject.Hidden = true;
                }

                return (NewObject);
            }
        }
        public DriverModel Update(DriverModel ph)
        {
            DriverCUWindow UpdateWindow = new DriverCUWindow();

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

            UpdateWindow.ExperienceTextBox.Text = ph.Experience.ToString();
            UpdateWindow.SalaryTextBox.Text = ph.Salary.ToString();

            if (ph.Hidden == false)
            {
                UpdateWindow.HiddenNoRadioButton.IsChecked = true;
                UpdateWindow.HiddenYesRadioButton.IsChecked = false;
            }
            else
            {
                UpdateWindow.HiddenNoRadioButton.IsChecked = false;
                UpdateWindow.HiddenYesRadioButton.IsChecked = true;
            }

            bool? result = UpdateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                ph.FullName = UpdateWindow.SurnameTextBox.Text + " " + UpdateWindow.NameTextBox.Text + " " + UpdateWindow.PatronymicTextBox.Text;
                ph.Experience = Int32.Parse(UpdateWindow.ExperienceTextBox.Text);
                ph.Salary = Int32.Parse(UpdateWindow.SalaryTextBox.Text);
                if (UpdateWindow.HiddenNoRadioButton.IsChecked == true)
                {
                    ph.Hidden = false;
                }
                if (UpdateWindow.HiddenYesRadioButton.IsChecked == true)
                {
                    ph.Hidden = true;
                }

                return (ph);
            }
        }
    }
}
