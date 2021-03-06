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
    public class CRUDLogicForTransport
    {
        DBDataOperations DBComunication;

        public CRUDLogicForTransport()
        {

        }
        public CRUDLogicForTransport(DBDataOperations DBComunicationFromWindow)
        {
            DBComunication = DBComunicationFromWindow;
        }

        public TransportModel Create()
        {
            TransportCUWindow CreateWindow = new TransportCUWindow();

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                TransportModel NewObject = new TransportModel();

                NewObject.NumberOfSeats = CreateWindow.NumberOfSeatsIntegerUpDown.Value;
                NewObject.RegistrationNumber = CreateWindow.RegistrationNumberTextBox.Text;
                NewObject.Model = CreateWindow.ModelTextBox.Text;
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
        public TransportModel Update(TransportModel ph)
        {
            TransportCUWindow UpdateWindow = new TransportCUWindow();

            UpdateWindow.NumberOfSeatsIntegerUpDown.Value = ph.NumberOfSeats;
            UpdateWindow.RegistrationNumberTextBox.Text = ph.RegistrationNumber;
            UpdateWindow.ModelTextBox.Text = ph.Model;
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
                return(null);
            else
            {
                ph.NumberOfSeats = UpdateWindow.NumberOfSeatsIntegerUpDown.Value;
                ph.RegistrationNumber = UpdateWindow.RegistrationNumberTextBox.Text;
                ph.Model = UpdateWindow.ModelTextBox.Text;
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