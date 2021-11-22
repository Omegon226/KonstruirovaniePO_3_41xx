using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLL.Models;
using BLL.DBInteraction;
using BLL.Services;
using System.Collections.ObjectModel;

namespace Интерфейс
{
    /// <summary>
    /// Логика взаимодействия для SelectCruiseWindow.xaml
    /// </summary>
    public partial class SelectCruiseWindow : Window
    {
        public class PossibleCruises
        {
            public int CruiseID { get; set; }
            public int DayOfTheWeekCruiseID { get; set; }
            public int RouteIDOfTheCruise { get; set; }
            public int DriverIDOfTheCruise { get; set; }
            public int TransportIDOfTheCruise { get; set; }
            public TimeSpan? StartTime { get; set; }

            public int? StartPointIndex { get; set; }
            public int StartPointStoppingID { get; set; }
            public int StartPointLocalityID { get; set; }
            public string StartPointLocalityName { get; set; }
            public int? EndPointIndex { get; set; }
            public int EndPointStoppingID { get; set; }
            public int EndPointLocalityID { get; set; }
            public string EndPointLocalityName { get; set; }

            public double? FullPrice;
            public int? FullTimeInCruise;

            public PossibleCruises(CruiseModel CruiseInfo, FindeRouteForCruises.FinalResult RouteInfo)
            {
                this.CruiseID = CruiseInfo.ID;
                this.DayOfTheWeekCruiseID = CruiseInfo.DayOfTheWeekCruiseID;
                this.RouteIDOfTheCruise = CruiseInfo.RouteIDOfTheCruise;
                this.DriverIDOfTheCruise = CruiseInfo.DriverIDOfTheCruise;
                this.TransportIDOfTheCruise = CruiseInfo.TransportIDOfTheCruise;
                this.StartTime = CruiseInfo.StartTime;

                this.StartPointIndex = RouteInfo.StartPointIndex;
                this.StartPointStoppingID = RouteInfo.StartPointStoppingID;
                this.StartPointLocalityID = RouteInfo.StartPointLocalityID;
                this.StartPointLocalityName = RouteInfo.StartPointLocalityName;
                this.EndPointIndex = RouteInfo.EndPointIndex;
                this.EndPointStoppingID = RouteInfo.EndPointStoppingID;
                this.EndPointLocalityID = RouteInfo.EndPointLocalityID;
                this.EndPointLocalityName = RouteInfo.EndPointLocalityName;
            }

            public void AddPriceAndTimeInfo(FindeAditionalInformationForCruise.StoredProcedureResult AdditionalInfoForCruises)
            {
                this.FullPrice = AdditionalInfoForCruises.PriceOfCruise;
                this.FullTimeInCruise = AdditionalInfoForCruises.TravelTimeInHours;
            }
        }

        public class CruisesForWindowInfo
        {
            public PossibleCruises Cruise;

            public DateTime StartDate;
            public int AmountOfFreeSeats;
            public List<int> OccupiedSeats = new List<int>();

            public string CruiseStartDate { get; set; }
            public string CruiseStartPointLocalityName { get; set; }
            public string CruiseFullTimeInCruise { get; set; }
            public string CruiseEndPointLocalityName { get; set; }
            public string CruiseFullPrice { get; set; }
            public string CruiseAmountOfFreeSeats { get; set; }

            public CruisesForWindowInfo(PossibleCruises PossibleCruises)
            {
                this.Cruise = PossibleCruises;
            }
            public void SetStartDate(DateTime Date)
            {
                this.StartDate = Date;
            }
            public void InitioliseStringInfoForWindow()
            {
                this.CruiseStartDate = StartDate.ToString();
                this.CruiseStartPointLocalityName = "Нач: " + Cruise.StartPointLocalityName;
                this.CruiseFullTimeInCruise = Cruise.FullTimeInCruise.ToString() + " Часа";
                this.CruiseEndPointLocalityName = "Кон: " + Cruise.EndPointLocalityName;
                this.CruiseFullPrice = "Цена : " + Cruise.FullPrice.ToString() + " Руб.";
                this.CruiseAmountOfFreeSeats = AmountOfFreeSeats.ToString();
            }
        }

        private DBDataOperations DBComunication;

        private List<PossibleCruises> allPossibleCruises = new List<PossibleCruises>();
        private ObservableCollection<CruisesForWindowInfo> allCruisesForWindow = new ObservableCollection<CruisesForWindowInfo>();

        private List<CruiseModel> allCruise;
        private List<DayOfTheWeekModel> allDayOfTheWeek;
        private List<DriverModel> allDriver;
        private List<LocalityModel> allLocality;
        private List<RouteModel> allRoute;
        private List<StoppingOnTheRouteModel> allStoppingOnTheRoute;
        private List<StopSequencesModel> allStopSequences;
        private List<TicketModel> allTicket;
        private List<TransportModel> allTransport;
        private List<UserModel> allUser;

        private const int minimalPriceOfCruise = 50;
        private const int MaxDayForOrderingPossibility = 31;
        private TimeSpan MinimumTimeForOrderingTicket = new TimeSpan(0, 15, 0);

        private int StatusLevelOfUser = 0;
        private UserModel AuthorisedUser;
        private int AmountOfTicketsToBuy = 0;

        private int IDOfStartingLocation;
        private int IDOfEndLocation;

        private CruisesForWindowInfo CheckedCruiseToBuy;
        private int IDOfCheckedCruiseToBuy;
        private TransportModel TransportOfCheckedCruiseToBuy;

        public SelectCruiseWindow(DBDataOperations DBComunicationFromMainWindow, int IDOfStartingLocationFromMainWindow, int IDOfEndLocationFromMainWindow, int UserStausLevel, UserModel User)
        {
            DBComunication = DBComunicationFromMainWindow;
            IDOfStartingLocation = IDOfStartingLocationFromMainWindow;
            IDOfEndLocation = IDOfEndLocationFromMainWindow;
            StatusLevelOfUser = UserStausLevel;
            AuthorisedUser = User;

            LoadAllInformationFromDataBase();
            CreateCruisesForWindow();

            InitializeComponent();

            CruisesList.ItemsSource = allCruisesForWindow;
        }

        private void CruisesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckedCruiseToBuy = (CruisesForWindowInfo)CruisesList.SelectedItem;
            IDOfCheckedCruiseToBuy = CruisesList.SelectedIndex;
            for (int i = 0; i < allTransport.Count; ++i)
            {
                if (CheckedCruiseToBuy.Cruise.TransportIDOfTheCruise == allTransport[i].ID)
                {
                    TransportOfCheckedCruiseToBuy = allTransport[i];
                }
            }
        }

        private void ReturnBackOnMainWindowFromSelectCruiseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BuyTicketsButton_Click(object sender, RoutedEventArgs e)
        {
            AmountOfTicketsToBuy = (int)AmountOfTeacketsToBuyIntegerUpDown.Value;

            List<string> IndentificationDocuments = new List<string>();
            IndentificationDocuments.Add("Паспорт");
            IndentificationDocuments.Add("Свидетельство о рождении");

            List<int> FreeSeats = new List<int>();

            if (AmountOfTicketsToBuy <= 0)
            {
                MessageBox.Show("Вы ввели неверное кол-во билетов для покупки");
                return;
            }

            List<TicketModel> AllTicketsToBuy = new List<TicketModel>();


            for (int i = 1; i <= TransportOfCheckedCruiseToBuy.NumberOfSeats; ++i)
            {
                bool flagOfOccpiedSeat = false;
                for (int j = 0; j < CheckedCruiseToBuy.OccupiedSeats.Count; ++j)
                {
                    if (CheckedCruiseToBuy.OccupiedSeats[j] == i)
                    {
                        flagOfOccpiedSeat = true;
                    }
                }

                if (flagOfOccpiedSeat == false)
                {
                    FreeSeats.Add(i);
                }
            }

            for (int i = 0; i < AmountOfTicketsToBuy; ++i)
            {
                OrderTicketWindow OrderTicketWindow = new OrderTicketWindow();
                OrderTicketWindow.FreeSeatsComboBox.ItemsSource = FreeSeats;
                OrderTicketWindow.DocumentTypeComboBox.ItemsSource = IndentificationDocuments;

                bool? result = OrderTicketWindow.ShowDialog();
                if (result == false)
                    return;
                else
                {
                    TicketModel NewObject = new TicketModel();

                    NewObject.DateOfIssue = DateTime.Now;
                    NewObject.IdentificationInformation = OrderTicketWindow.DocumentInfoTextBox.Text;
                    NewObject.SeatNumberOnTheTransport = (int)OrderTicketWindow.FreeSeatsComboBox.SelectedValue;
                    NewObject.FullName = OrderTicketWindow.SurnameTextBox.Text + OrderTicketWindow.NameTextBox.Text + OrderTicketWindow.PatronymicTextBox.Text;
                    NewObject.CruiseID = CheckedCruiseToBuy.Cruise.CruiseID;
                    NewObject.UserID = AuthorisedUser.ID;
                    NewObject.RaceDepartureTime = (DateTime?)CheckedCruiseToBuy.StartDate;

                    AllTicketsToBuy.Add(NewObject);

                }
            }
        }

        #region --- Подгрузка информации в переменные эмулирующие таблицы

        private void LoadAllInformationFromDataBase()
        {
            LoadAllCruiseInformation();
            LoadAllDayOfTheWeekInformation();
            LoadAllDriverInformation();
            LoadAllLocalityInformation();
            LoadAllRouteInformation();
            LoadAllStoppingOnTheRouteInformation();
            LoadAllStopSequencesInformation();
            LoadAllTicketInformation();
            LoadAllTransportInformation();
            LoadAllUserInformation();
        }
        private void LoadAllCruiseInformation()
        {
            allCruise = DBComunication.Cruise.GetAll();
        }
        private void LoadAllDayOfTheWeekInformation()
        {
            allDayOfTheWeek = DBComunication.DayOfTheWeek.GetAll();
        }
        private void LoadAllDriverInformation()
        {
            allDriver = DBComunication.Driver.GetAll();
        }
        private void LoadAllLocalityInformation()
        {
            allLocality = DBComunication.Locality.GetAll();
        }
        private void LoadAllRouteInformation()
        {
            allRoute = DBComunication.Route.GetAll();
        }
        private void LoadAllStoppingOnTheRouteInformation()
        {
            allStoppingOnTheRoute = DBComunication.StoppingOnTheRoute.GetAll();
        }
        private void LoadAllStopSequencesInformation()
        {
            allStopSequences = DBComunication.StopSequences.GetAll();
        }
        private void LoadAllTicketInformation()
        {
            allTicket = DBComunication.Ticket.GetAll();
        }
        private void LoadAllTransportInformation()
        {
            allTransport = DBComunication.Transport.GetAll();
        }
        private void LoadAllUserInformation()
        {
            allUser = DBComunication.User.GetAll();
        }

        #endregion

        #region --- Изучение таблиц и создание Рейсов для заказа билетов

        private void CreateCruisesForWindow()
        {
            FindeRoutesForCruises();
            FindeAditionalInfoForCruises();
            InitioliseStartDateOfCruises();
            FindeFreeSeatsForCruises();
            InitialiseCruiseStringInfoForWindow();
        }

        private void FindeRoutesForCruises()
        {
            List<FindeRouteForCruises.FinalResult> Routes = FindeRouteForCruises.FindeRouts(IDOfStartingLocation, IDOfEndLocation);
            PossibleCruises CruiseToAdd;

            for (int i = 0; i < Routes.Count; ++i)
            {
                for (int j = 0; j < allCruise.Count; ++j)
                {
                    if (Routes[i].RouteID == allCruise[j].RouteIDOfTheCruise)
                    {
                        CruiseToAdd = new PossibleCruises(allCruise[j], Routes[i]);
                        allPossibleCruises.Add(CruiseToAdd);
                    }
                }
            }
        }

        private void FindeAditionalInfoForCruises()
        {
            for (int i = 0; i < allPossibleCruises.Count; ++i)
            {
                for (int j = 0; j < allRoute.Count; ++j)
                {
                    if (allPossibleCruises[i].RouteIDOfTheCruise == allRoute[j].ID)
                    {
                        List<FindeAditionalInformationForCruise.StoredProcedureResult> AdditionalInfoForCruises =
                            FindeAditionalInformationForCruise.StoredProcedureExecute(allPossibleCruises[i].RouteIDOfTheCruise,
                                                                                        allPossibleCruises[i].StartPointIndex,
                                                                                        allPossibleCruises[i].EndPointIndex,
                                                                                        minimalPriceOfCruise);
                        // В этом объекте всегда может быть только одна строка
                        allPossibleCruises[i].AddPriceAndTimeInfo(AdditionalInfoForCruises[0]);
                    }
                }
            }
        }

        private void InitioliseStartDateOfCruises()
        {
            DateTime DateTimeForCruise = DateTime.Now;
            DateTimeForCruise = new DateTime(DateTimeForCruise.Year, DateTimeForCruise.Month, DateTimeForCruise.Day, 0, 0, 0);
            CruisesForWindowInfo CruiseForWindowToCreate;

            for (int i = 0; i <= MaxDayForOrderingPossibility; ++i)
            {
                if (i != 0)
                {
                    DateTimeForCruise = DateTimeForCruise.AddDays(+1);
                    DateTimeForCruise = new DateTime(DateTimeForCruise.Year, DateTimeForCruise.Month, DateTimeForCruise.Day, 0, 0, 0);
                }
                int DayOfTheWeekForCruise = ParseDaysOfTheWeek(DateTimeForCruise);

                for (int j = 0; j < allPossibleCruises.Count; ++j)
                {
                    CruiseForWindowToCreate = new CruisesForWindowInfo(allPossibleCruises[j]);

                    bool TodayIsTheDayOfCruise = allPossibleCruises[j].DayOfTheWeekCruiseID == DayOfTheWeekForCruise;
                    DateTime NowMoment = DateTime.Now;
                    TimeSpan CruiseStartTime = (TimeSpan)CruiseForWindowToCreate.Cruise.StartTime;
                    DateTime DateTimeForCruiseCalculations = new DateTime(DateTimeForCruise.Year, DateTimeForCruise.Month, DateTimeForCruise.Day).Add(CruiseStartTime);
                    TimeSpan DateDifference = DateTimeForCruiseCalculations - NowMoment;
                    bool DifferenceInNowTimeAndCruiseStartTime = DateDifference > MinimumTimeForOrderingTicket;

                    if ((TodayIsTheDayOfCruise) && (DifferenceInNowTimeAndCruiseStartTime))
                    {
                        CruiseForWindowToCreate.SetStartDate(DateTimeForCruise.Add((TimeSpan)CruiseForWindowToCreate.Cruise.StartTime));
                        allCruisesForWindow.Add(CruiseForWindowToCreate);
                    }
                }

            }
        }
        private int ParseDaysOfTheWeek(DateTime DateTimeForCruise)
        {
            int DayOfTheWeek = 0; 

            switch (DateTimeForCruise.DayOfWeek)
            {
                case (DayOfWeek.Monday):
                    DayOfTheWeek = 1;
                    break;
                case (DayOfWeek.Tuesday):
                    DayOfTheWeek = 2;
                    break;
                case (DayOfWeek.Wednesday):
                    DayOfTheWeek = 3;
                    break;
                case (DayOfWeek.Thursday):
                    DayOfTheWeek = 4;
                    break;
                case (DayOfWeek.Friday):
                    DayOfTheWeek = 5;
                    break;
                case (DayOfWeek.Saturday):
                    DayOfTheWeek = 6;
                    break;
                case (DayOfWeek.Sunday):
                    DayOfTheWeek = 7;
                    break;
                default:
                    MessageBox.Show("Что то пошло не так с парсингом дней недели");
                    break;
            }

            return (DayOfTheWeek);
        }

        private void FindeFreeSeatsForCruises()
        {
            for (int i = 0; i < allCruisesForWindow.Count; ++i)
            {
                for (int j = 0; j < allRoute.Count; ++j)
                {
                    if (allCruisesForWindow[i].Cruise.RouteIDOfTheCruise == allTransport[j].ID)
                    {
                        allCruisesForWindow[i].AmountOfFreeSeats = (int)allTransport[j].NumberOfSeats;
                    }
                }
            }

            // Работа с билетами
            for (int i = 0; i < allCruisesForWindow.Count; ++i)
            {
                FindeOrderedSeatsForCruise.FinalResult OccupiedSeatsForCruises = FindeOrderedSeatsForCruise.CreateResult(
                    allCruisesForWindow[i].StartDate, allCruisesForWindow[i].Cruise.CruiseID);

                allCruisesForWindow[i].AmountOfFreeSeats -= OccupiedSeatsForCruises.OrderedSeats.Count;

                for (int j = 0; j < OccupiedSeatsForCruises.OrderedSeats.Count; ++j)
                {
                    allCruisesForWindow[i].OccupiedSeats.Add(OccupiedSeatsForCruises.OrderedSeats[j]);
                }
            }
        }

        private void InitialiseCruiseStringInfoForWindow()
        {
            for (int i = 0; i < allCruisesForWindow.Count; ++i)
            {
                allCruisesForWindow[i].InitioliseStringInfoForWindow();
            }
        }


        #endregion

    }
}
