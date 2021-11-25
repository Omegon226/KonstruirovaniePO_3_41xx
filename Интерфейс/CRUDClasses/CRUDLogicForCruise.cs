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
    public class CRUDLogicForCruise
    {
        DBDataOperations DBComunication;

        List<DayOfTheWeekModel> allDayOfTheWeekCRUD;
        List<RouteModel> allRouteCRUD;
        List<DriverModel> allDriverCRUD;
        List<TransportModel> allTransportCRUD;

        public CRUDLogicForCruise()
        {
            allDayOfTheWeekCRUD = DBComunication.DayOfTheWeek.GetAll();
            allRouteCRUD = DBComunication.Route.GetAll();
            allDriverCRUD = DBComunication.Driver.GetAll();
            allTransportCRUD = DBComunication.Transport.GetAll();
        }
        public CRUDLogicForCruise(DBDataOperations DBComunicationFromWindow)
        {
            DBComunication = DBComunicationFromWindow;

            allDayOfTheWeekCRUD = DBComunication.DayOfTheWeek.GetAll();
            allRouteCRUD = DBComunication.Route.GetAll();
            allDriverCRUD = DBComunication.Driver.GetAll();
            allTransportCRUD = DBComunication.Transport.GetAll();
        }

        public CruiseModel Create()
        {
            CruiseCUWindow CreateWindow = new CruiseCUWindow();
            MakeSetupForCruiseCUWindow(CreateWindow);

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                CruiseModel NewObject = new CruiseModel();

                NewObject.DayOfTheWeekCruiseID = (int)CreateWindow.DayOfTheWeekIDComboBox.SelectedValue;
                NewObject.RouteIDOfTheCruise = (int)CreateWindow.RouteIDOfTheCruiseComboBox.SelectedValue;
                NewObject.DriverIDOfTheCruise = (int)CreateWindow.DayOfTheWeekIDComboBox.SelectedValue;
                NewObject.TransportIDOfTheCruise = (int)CreateWindow.TransportIDOfTheCruiseComboBox.SelectedValue;

                int Hours = Int32.Parse(CreateWindow.StartTimeHoursTextBox.Text);
                int Minutes = Int32.Parse(CreateWindow.StartTimeMinutesTextBox.Text);
                int Seconds = Int32.Parse(CreateWindow.StartTimeSecondsTextBox.Text);
                NewObject.StartTime = new TimeSpan(Hours, Minutes, Seconds);

                return (NewObject);
            }
        }
        public CruiseModel Update(CruiseModel ph)
        {
            CruiseCUWindow UpdateWindow = new CruiseCUWindow();
            MakeSetupForCruiseCUWindow(UpdateWindow);

            UpdateWindow.DayOfTheWeekIDComboBox.SelectedValue = ph.DayOfTheWeekCruiseID;
            UpdateWindow.RouteIDOfTheCruiseComboBox.SelectedValue = ph.RouteIDOfTheCruise;
            UpdateWindow.DriverIDOfTheCruiseComboBox.SelectedValue = ph.DriverIDOfTheCruise;
            UpdateWindow.TransportIDOfTheCruiseComboBox.SelectedValue = ph.TransportIDOfTheCruise;

            int Hours = ph.StartTime.Value.Hours;
            int Minutes = ph.StartTime.Value.Minutes;
            int Seconds = ph.StartTime.Value.Seconds;
            UpdateWindow.StartTimeHoursTextBox.Text = Hours.ToString();
            UpdateWindow.StartTimeMinutesTextBox.Text = Minutes.ToString();
            UpdateWindow.StartTimeSecondsTextBox.Text = Seconds.ToString();

            bool? result = UpdateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                ph.DayOfTheWeekCruiseID = (int)UpdateWindow.DayOfTheWeekIDComboBox.SelectedValue;
                ph.RouteIDOfTheCruise = (int)UpdateWindow.RouteIDOfTheCruiseComboBox.SelectedValue;
                ph.DriverIDOfTheCruise = (int)UpdateWindow.DriverIDOfTheCruiseComboBox.SelectedValue;
                ph.TransportIDOfTheCruise = (int)UpdateWindow.TransportIDOfTheCruiseComboBox.SelectedValue;

                Hours = Int32.Parse(UpdateWindow.StartTimeHoursTextBox.Text);
                Minutes = Int32.Parse(UpdateWindow.StartTimeMinutesTextBox.Text);
                Seconds = Int32.Parse(UpdateWindow.StartTimeSecondsTextBox.Text);
                ph.StartTime = new TimeSpan(Hours, Minutes, Seconds);

                return (ph);
            }
        }
        private void MakeSetupForCruiseCUWindow(CruiseCUWindow Window)
        {
            Window.DayOfTheWeekIDComboBox.ItemsSource = allDayOfTheWeekCRUD;
            Window.DayOfTheWeekIDComboBox.DisplayMemberPath = "DayOfTheWeekName";
            Window.DayOfTheWeekIDComboBox.SelectedValuePath = "ID";
            Window.RouteIDOfTheCruiseComboBox.ItemsSource = allRouteCRUD;
            Window.RouteIDOfTheCruiseComboBox.DisplayMemberPath = "ID";
            Window.RouteIDOfTheCruiseComboBox.SelectedValuePath = "ID";
            Window.DriverIDOfTheCruiseComboBox.ItemsSource = allDriverCRUD;
            Window.DriverIDOfTheCruiseComboBox.DisplayMemberPath = "FullName";
            Window.DriverIDOfTheCruiseComboBox.SelectedValuePath = "ID";
            Window.TransportIDOfTheCruiseComboBox.ItemsSource = allTransportCRUD;
            Window.TransportIDOfTheCruiseComboBox.DisplayMemberPath = "RegistrationNumber";
            Window.TransportIDOfTheCruiseComboBox.SelectedValuePath = "ID";
        }
    }
}
