using BLL.DBInteraction;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Интерфейс.CeateUpdateWindows;

namespace Интерфейс.CRUDClasses
{
    public class CRUDLogicForTicket
    {
        DBDataOperations DBComunication;
        private List<string> IndentificationDocuments = new List<string>();

        List<CruiseModel> allCruiseCRUD;
        List<UserModel> allUserCRUD;

        public CRUDLogicForTicket()
        {
            IndentificationDocuments.Add("Паспорт");
            IndentificationDocuments.Add("Свидетельство о рождении");

            allCruiseCRUD = DBComunication.Cruise.GetAll();
            allUserCRUD = DBComunication.User.GetAll();
        }
        public CRUDLogicForTicket(DBDataOperations DBComunicationFromWindow)
        {
            DBComunication = DBComunicationFromWindow;

            IndentificationDocuments.Add("Паспорт");
            IndentificationDocuments.Add("Свидетельство о рождении");
        }

        public TicketModel Create()
        {
            TicketCUWindow CreateWindow = new TicketCUWindow();
            MakeSetupForTicketCUWindow(CreateWindow);

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                TicketModel NewObject = new TicketModel();

                int Year = Int32.Parse(CreateWindow.DateOfIssueYearParametrTextBox.Text);
                int Month = Int32.Parse(CreateWindow.DateOfIssueMonthParametrTextBox.Text);
                int Day = Int32.Parse(CreateWindow.DateOfIssueDayParametrTextBox.Text);
                int Hour = Int32.Parse(CreateWindow.DateOfIssueHourParametrTextBox.Text);
                int Minute = Int32.Parse(CreateWindow.DateOfIssueMinuteParametrTextBox.Text);
                int Second = Int32.Parse(CreateWindow.DateOfIssueSecondParametrTextBox.Text);
                NewObject.DateOfIssue = new DateTime(Year, Month, Day, Hour, Minute, Second);

                NewObject.IdentificationInformation = CreateWindow.IndentificationInformationTextBox.Text;
                NewObject.SeatNumberOnTheTransport = CreateWindow.SeatNumberOnTheTransportIntegerUpDown.Value;
                NewObject.FullName = CreateWindow.SurnameTextBox.Text + " " + CreateWindow.NameTextBox.Text + " " + CreateWindow.PatronymicTextBox.Text;
                NewObject.CruiseID = (int)CreateWindow.CruiseIDComboBox.SelectedValue;
                NewObject.UserID = (int)CreateWindow.UserIDComboBox.SelectedValue;

                Year = Int32.Parse(CreateWindow.RaceDepartureTimeYearParametrTextBox.Text);
                Month = Int32.Parse(CreateWindow.RaceDepartureTimeMonthParametrTextBox.Text);
                Day = Int32.Parse(CreateWindow.RaceDepartureTimeDayParametrTextBox.Text);
                Hour = Int32.Parse(CreateWindow.RaceDepartureTimeHourParametrTextBox.Text);
                Minute = Int32.Parse(CreateWindow.RaceDepartureTimeMinuteParametrTextBox.Text);
                Second = Int32.Parse(CreateWindow.RaceDepartureTimeSecondParametrTextBox.Text);
                NewObject.RaceDepartureTime = new DateTime(Year, Month, Day, Hour, Minute, Second);

                return (NewObject);
            }
        }
        public TicketModel Update(TicketModel ph)
        {
            TicketCUWindow UpdateWindow = new TicketCUWindow();
            MakeSetupForTicketCUWindow(UpdateWindow);

            int Year = ph.DateOfIssue.Value.Year;
            int Month = ph.DateOfIssue.Value.Month;
            int Day = ph.DateOfIssue.Value.Day;
            int Hour = ph.DateOfIssue.Value.Hour;
            int Minute = ph.DateOfIssue.Value.Minute;
            int Second = ph.DateOfIssue.Value.Second;
            UpdateWindow.DateOfIssueYearParametrTextBox.Text = Year.ToString();
            UpdateWindow.DateOfIssueMonthParametrTextBox.Text = Month.ToString();
            UpdateWindow.DateOfIssueDayParametrTextBox.Text = Day.ToString();
            UpdateWindow.DateOfIssueHourParametrTextBox.Text = Hour.ToString();
            UpdateWindow.DateOfIssueMinuteParametrTextBox.Text = Minute.ToString();
            UpdateWindow.DateOfIssueSecondParametrTextBox.Text = Second.ToString();

            if ((ph.IdentificationInformation.Length == 11 && ph.IdentificationInformation[4] == ' ') || (ph.IdentificationInformation.Length == 10))
            {
                UpdateWindow.DocumentTypeComboBox.SelectedIndex = 0;
                UpdateWindow.IndentificationInformationTextBox.Text = ph.IdentificationInformation;
            }
            if ((15 <= ph.IdentificationInformation.Length) && (ph.IdentificationInformation.Length <= 17))
            {
                UpdateWindow.DocumentTypeComboBox.SelectedIndex = 1;
                UpdateWindow.IndentificationInformationTextBox.Text = ph.IdentificationInformation;
            }
            if (ph.IdentificationInformation.Length < 10 || ph.IdentificationInformation.Length == 12 || ph.IdentificationInformation.Length > 15)
            {
                MessageBox.Show("Индентификационные данные по билету не могут быть занесены, т.к. их формат не правильный");
            }

            UpdateWindow.SeatNumberOnTheTransportIntegerUpDown.Value = ph.SeatNumberOnTheTransport;

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

            UpdateWindow.CruiseIDComboBox.SelectedValue = ph.CruiseID;
            UpdateWindow.UserIDComboBox.SelectedValue = ph.UserID;

            Year = ph.RaceDepartureTime.Value.Year;
            Month = ph.RaceDepartureTime.Value.Month;
            Day = ph.RaceDepartureTime.Value.Day;
            Hour = ph.RaceDepartureTime.Value.Hour;
            Minute = ph.RaceDepartureTime.Value.Minute;
            Second = ph.RaceDepartureTime.Value.Second;
            UpdateWindow.RaceDepartureTimeYearParametrTextBox.Text = Year.ToString();
            UpdateWindow.RaceDepartureTimeMonthParametrTextBox.Text = Month.ToString();
            UpdateWindow.RaceDepartureTimeDayParametrTextBox.Text = Day.ToString();
            UpdateWindow.RaceDepartureTimeHourParametrTextBox.Text = Hour.ToString();
            UpdateWindow.RaceDepartureTimeMinuteParametrTextBox.Text = Minute.ToString();
            UpdateWindow.RaceDepartureTimeSecondParametrTextBox.Text = Second.ToString();

            bool? result = UpdateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                Year = Int32.Parse(UpdateWindow.DateOfIssueYearParametrTextBox.Text);
                Month = Int32.Parse(UpdateWindow.DateOfIssueMonthParametrTextBox.Text);
                Day = Int32.Parse(UpdateWindow.DateOfIssueDayParametrTextBox.Text);
                Hour = Int32.Parse(UpdateWindow.DateOfIssueHourParametrTextBox.Text);
                Minute = Int32.Parse(UpdateWindow.DateOfIssueMinuteParametrTextBox.Text);
                Second = Int32.Parse(UpdateWindow.DateOfIssueSecondParametrTextBox.Text);
                ph.DateOfIssue = new DateTime(Year, Month, Day, Hour, Minute, Second);

                ph.IdentificationInformation = UpdateWindow.IndentificationInformationTextBox.Text;
                ph.SeatNumberOnTheTransport = UpdateWindow.SeatNumberOnTheTransportIntegerUpDown.Value;
                ph.FullName = UpdateWindow.SurnameTextBox.Text + " " + UpdateWindow.NameTextBox.Text + " " + UpdateWindow.PatronymicTextBox.Text;
                ph.CruiseID = (int)UpdateWindow.CruiseIDComboBox.SelectedValue;
                ph.UserID = (int)UpdateWindow.UserIDComboBox.SelectedValue;

                Year = Int32.Parse(UpdateWindow.RaceDepartureTimeYearParametrTextBox.Text);
                Month = Int32.Parse(UpdateWindow.RaceDepartureTimeMonthParametrTextBox.Text);
                Day = Int32.Parse(UpdateWindow.RaceDepartureTimeDayParametrTextBox.Text);
                Hour = Int32.Parse(UpdateWindow.RaceDepartureTimeHourParametrTextBox.Text);
                Minute = Int32.Parse(UpdateWindow.RaceDepartureTimeMinuteParametrTextBox.Text);
                Second = Int32.Parse(UpdateWindow.RaceDepartureTimeSecondParametrTextBox.Text);
                ph.RaceDepartureTime = new DateTime(Year, Month, Day, Hour, Minute, Second);

                return (ph);
            }
        }

        private void MakeSetupForTicketCUWindow(TicketCUWindow Window)
        {
            Window.CruiseIDComboBox.ItemsSource = allCruiseCRUD;
            Window.CruiseIDComboBox.DisplayMemberPath = "ID";
            Window.CruiseIDComboBox.SelectedValuePath = "ID";
            Window.UserIDComboBox.ItemsSource = allUserCRUD;
            Window.UserIDComboBox.DisplayMemberPath = "FullName";
            Window.UserIDComboBox.SelectedValuePath = "ID";
            Window.DocumentTypeComboBox.ItemsSource = IndentificationDocuments;
        }
    }
}

