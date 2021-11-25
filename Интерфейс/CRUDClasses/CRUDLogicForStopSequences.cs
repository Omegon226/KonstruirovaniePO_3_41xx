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
    public class CRUDLogicForStopSequences
    {
        DBDataOperations DBComunication;

        List<LocalityModel> allLocalityCRUD;
        List<StoppingOnTheRouteModel> allStoppingOnTheRouteCRUD;
        List<RouteModel> allRouteCRUD;

        public CRUDLogicForStopSequences()
        {
            allLocalityCRUD = DBComunication.Locality.GetAll();
            allStoppingOnTheRouteCRUD = DBComunication.StoppingOnTheRoute.GetAll();
            allRouteCRUD = DBComunication.Route.GetAll();
        }
        public CRUDLogicForStopSequences(DBDataOperations DBComunicationFromWindow)
        {
            DBComunication = DBComunicationFromWindow;

            allLocalityCRUD = DBComunication.Locality.GetAll();
            allStoppingOnTheRouteCRUD = DBComunication.StoppingOnTheRoute.GetAll();
            allRouteCRUD = DBComunication.Route.GetAll();
        }

        public StopSequencesModel Create()
        {
            StopSequencesCUWindow CreateWindow = new StopSequencesCUWindow(allLocalityCRUD, allStoppingOnTheRouteCRUD);
            MakeSetupForStopSequencesCUWindow(CreateWindow);

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                StopSequencesModel NewObject = new StopSequencesModel();

                NewObject.IndexNumber = CreateWindow.IndexNumberIntegerUpDown.Value;
                NewObject.StoppingID = (int)CreateWindow.StoppingIDComboBox.SelectedValue;
                NewObject.StopRouteID = (int)CreateWindow.StopRouteIDComboBox.SelectedValue;
                NewObject.TripPrice = float.Parse(CreateWindow.TripPriceTextBox.Text);

                int Hours = Int32.Parse(CreateWindow.TravelTimeToStopHoursTextBox.Text);
                int Minutes = Int32.Parse(CreateWindow.TravelTimeToStopMinutesTextBox.Text);
                int Seconds = Int32.Parse(CreateWindow.TravelTimeToStopSecondsTextBox.Text);
                NewObject.TravelTimeToStop = new TimeSpan(Hours, Minutes, Seconds);

                return (NewObject);
            }
        }
        public StopSequencesModel Update(StopSequencesModel ph)
        {
            StopSequencesCUWindow UpdateWindow = new StopSequencesCUWindow(allLocalityCRUD, allStoppingOnTheRouteCRUD);
            MakeSetupForStopSequencesCUWindow(UpdateWindow);

            UpdateWindow.IndexNumberIntegerUpDown.Value = ph.IndexNumber;
            UpdateWindow.StoppingIDComboBox.SelectedValue = ph.StoppingID;
            UpdateWindow.StopRouteIDComboBox.SelectedValue = ph.StopRouteID;
            UpdateWindow.TripPriceTextBox.Text = ph.TripPrice.ToString();

            int Hours = ph.TravelTimeToStop.Value.Hours;
            int Minutes = ph.TravelTimeToStop.Value.Minutes;
            int Seconds = ph.TravelTimeToStop.Value.Seconds;
            UpdateWindow.TravelTimeToStopHoursTextBox.Text = Hours.ToString();
            UpdateWindow.TravelTimeToStopMinutesTextBox.Text = Minutes.ToString();
            UpdateWindow.TravelTimeToStopSecondsTextBox.Text = Seconds.ToString();

            bool? result = UpdateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                ph.IndexNumber = UpdateWindow.IndexNumberIntegerUpDown.Value;
                ph.StoppingID = (int)UpdateWindow.StoppingIDComboBox.SelectedValue;
                ph.StopRouteID = (int)UpdateWindow.StopRouteIDComboBox.SelectedValue;
                ph.TripPrice = float.Parse(UpdateWindow.TripPriceTextBox.Text);

                Hours = Int32.Parse(UpdateWindow.TravelTimeToStopHoursTextBox.Text);
                Minutes = Int32.Parse(UpdateWindow.TravelTimeToStopMinutesTextBox.Text);
                Seconds = Int32.Parse(UpdateWindow.TravelTimeToStopSecondsTextBox.Text);
                ph.TravelTimeToStop = new TimeSpan(Hours, Minutes, Seconds);

                return (ph);
            }
        }
        private void MakeSetupForStopSequencesCUWindow(StopSequencesCUWindow Window)
        {
            Window.StoppingIDComboBox.ItemsSource = allStoppingOnTheRouteCRUD;
            Window.StoppingIDComboBox.DisplayMemberPath = "ID";
            Window.StoppingIDComboBox.SelectedValuePath = "ID";
            Window.StopRouteIDComboBox.ItemsSource = allRouteCRUD;
            Window.StopRouteIDComboBox.DisplayMemberPath = "ID";
            Window.StopRouteIDComboBox.SelectedValuePath = "ID";
        }
    }
}
