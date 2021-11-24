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
using PdfSharp.Pdf;
using PdfSharp.Drawing;

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
            public TransportModel TransportOfTheCruise;

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
        private DateTime SelectedDateFromMainWindow;

        private CruisesForWindowInfo CheckedCruiseToBuy;
        List<string> IndentificationDocuments = new List<string>();

        MainWindow LinkToMainWindow;

        public SelectCruiseWindow(MainWindow Link, DBDataOperations DBComunicationFromMainWindow, int IDOfStartingLocationFromMainWindow, int IDOfEndLocationFromMainWindow, int UserStausLevel, UserModel User)
        {
            LinkToMainWindow = Link;
            DBComunication = DBComunicationFromMainWindow;
            IDOfStartingLocation = IDOfStartingLocationFromMainWindow;
            IDOfEndLocation = IDOfEndLocationFromMainWindow;
            StatusLevelOfUser = UserStausLevel;
            AuthorisedUser = User;
            SelectedDateFromMainWindow = DateTime.Now;

            IndentificationDocuments.Add("Паспорт");
            IndentificationDocuments.Add("Свидетельство о рождении");

            LoadAllInformationFromDataBase();
            CreateCruisesForWindow();

            InitializeComponent();

            Title = "Рейсы на маршрут: " + allCruisesForWindow[0].Cruise.StartPointLocalityName + " - " + allCruisesForWindow[0].Cruise.EndPointLocalityName;

            CruisesList.ItemsSource = allCruisesForWindow;
        }
        public SelectCruiseWindow(MainWindow Link, DBDataOperations DBComunicationFromMainWindow, int IDOfStartingLocationFromMainWindow, int IDOfEndLocationFromMainWindow, int UserStausLevel, UserModel User, DateTime? SelectedDate)
        {
            LinkToMainWindow = Link;
            DBComunication = DBComunicationFromMainWindow;
            IDOfStartingLocation = IDOfStartingLocationFromMainWindow;
            IDOfEndLocation = IDOfEndLocationFromMainWindow;
            StatusLevelOfUser = UserStausLevel;
            AuthorisedUser = User;
            SelectedDateFromMainWindow = (DateTime)SelectedDate;

            IndentificationDocuments.Add("Паспорт");
            IndentificationDocuments.Add("Свидетельство о рождении");

            LoadAllInformationFromDataBase();
            CreateCruisesForWindow();

            InitializeComponent();

            CruisesList.ItemsSource = allCruisesForWindow;
        }

        private void CruisesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckedCruiseToBuy = (CruisesForWindowInfo)CruisesList.SelectedItem;
        }

        private void ReturnBackOnMainWindowFromSelectCruiseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void BuyTicketsButton_Click(object sender, RoutedEventArgs e)
        {
            AmountOfTicketsToBuy = (int)AmountOfTeacketsToBuyIntegerUpDown.Value;
                        
            if (AmountOfTicketsToBuy <= 0)
            {
                MessageBox.Show("Вы ввели неверное кол-во билетов для покупки");
                return;
            }

            List<TicketModel> AllTicketsToBuy = new List<TicketModel>();

            if (CheckedCruiseToBuy == null)
            {
                MessageBox.Show("Вы не выбрали рейс для поездки!");
                return;
            }

            if (CheckedCruiseToBuy.AmountOfFreeSeats == 0)
            {
                MessageBox.Show("Нету свободных мест на рейс");
                return;
            }

            List<int> FreeSeats = FindeFreeSeatsForCruise();

            double FullPriceOfTickets = (double)CheckedCruiseToBuy.Cruise.FullPrice * AmountOfTicketsToBuy;

            for (int i = 0; i < AmountOfTicketsToBuy; ++i)
            {
                OrderTicketWindow OrderTicketWindow;

                if (AuthorisedUser != null)
                {
                    OrderTicketWindow = new OrderTicketWindow(AuthorisedUser);
                }
                else
                {
                    OrderTicketWindow = new OrderTicketWindow();
                }

                OrderTicketWindow.FreeSeatsComboBox.ItemsSource = FreeSeats;

                OrderTicketWindow.DocumentTypeComboBox.ItemsSource = IndentificationDocuments;

                bool? result = OrderTicketWindow.ShowDialog();
                if (result == false)
                    return;
                else
                {
                    TicketModel NewObject = new TicketModel();

                    NewObject.DateOfIssue = DateTime.Now;
                    NewObject.IdentificationInformation = OrderTicketWindow.IndentificationInformationTextBox.Text;
                    NewObject.SeatNumberOnTheTransport = (int)OrderTicketWindow.FreeSeatsComboBox.SelectedValue;
                    NewObject.FullName = OrderTicketWindow.SurnameTextBox.Text + " " + OrderTicketWindow.NameTextBox.Text + " " + OrderTicketWindow.PatronymicTextBox.Text;
                    NewObject.CruiseID = CheckedCruiseToBuy.Cruise.CruiseID;
                    NewObject.RaceDepartureTime = (DateTime?)CheckedCruiseToBuy.StartDate;

                    AllTicketsToBuy.Add(NewObject);

                    int SelectedSeat = (int)OrderTicketWindow.FreeSeatsComboBox.SelectedValue;
                    int? OrderedSeat = FindeAndDeleteOrderedSeat(SelectedSeat, FreeSeats);
                    if (OrderedSeat != null)
                    {
                        FreeSeats.Remove((int)OrderedSeat);
                    }
                }
            }

            if (StatusLevelOfUser == 0)
            {
                MessageBox.Show("Для покупки билетов требуется регистрация");

                AuthorizationSecuence();
            }

            for (int i = 0; i < AllTicketsToBuy.Count; ++i)
            {
                AllTicketsToBuy[i].UserID = AuthorisedUser.ID;
            }

            PaymentSecuence(FullPriceOfTickets);

            for (int i = 0; i < AllTicketsToBuy.Count; ++i)
            {
                CreatePDFOfTickets(i + 1, AllTicketsToBuy[i], CheckedCruiseToBuy);
            }

            for (int i = 0; i < AllTicketsToBuy.Count; ++i)
            {
                DBComunication.Ticket.Create(AllTicketsToBuy[i]);
                allTicket = DBComunication.Ticket.GetAll();
            }

            MessageBox.Show("Вы успешно оплатили билеты!");

            DialogResult = true;
            this.Close();

        }
        private List<int> FindeFreeSeatsForCruise()
        {
            List<int> FreeSeats = new List<int>();

            for (int i = 1; i <= CheckedCruiseToBuy.Cruise.TransportOfTheCruise.NumberOfSeats; ++i)
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

            return (FreeSeats);
        }
        private int? FindeAndDeleteOrderedSeat(int SelectedSeat, List<int> FreeSeats)
        {
            for (int j = 0; j < FreeSeats.Count; ++j)
            {
                if ((int)FreeSeats[j] == (int)SelectedSeat)
                {
                    return (FreeSeats[j]);
                }
            }
            return (null);
        }
        private void AuthorizationSecuence()
        {
            EnterAvtovokzalSystemWindow AuthorizationWindow = new EnterAvtovokzalSystemWindow();

            bool? result = AuthorizationWindow.ShowDialog();
            if (result == false)
                return;
            else
            {
                UserModel AuthorizedUser = new UserModel();

                AuthorizedUser.Login = AuthorizationWindow.LoginTextBox.Text;
                AuthorizedUser.Password = AuthorizationWindow.PasswordTextBox.Password;

                FindeSameUserAndUpdateStatus(AuthorizedUser);
            }
        }
        private void PaymentSecuence(double FullPriceOfTickets)
        {
            string TextOfPayment = "К оплате: " + FullPriceOfTickets.ToString() + " Руб.";

            TicketPaymentWindow PayWindow = new TicketPaymentWindow(TextOfPayment);

            bool? result = PayWindow.ShowDialog();
            if (result == false)
            {
                MessageBox.Show("Вы отказались от покупки билетов");
                return;
            }
        }

        private void FindeSameUserAndUpdateStatus(UserModel UserToFinde)
        {
            for (int i = 0; i < allUser.Count; ++i)
            {
                if ((UserToFinde.Login == allUser[i].Login) && (UserToFinde.Password == allUser[i].Password))
                {
                    StatusLevelOfUser = (int)allUser[i].Status;
                    AuthorisedUser = allUser[i];

                    if (allUser[i].Status == 1)
                    {
                        MessageBox.Show("Вход в систему осуществлён! Вы являетесь пользователем.");
                        LinkToMainWindow.ChangeStatusOfUser(1);
                        LinkToMainWindow.ChangeRegistraitedUserInfo(AuthorisedUser);
                    }
                    if (allUser[i].Status == 2)
                    {
                        MessageBox.Show("Вход в систему осуществлён! Вы являетесь администратором.");
                        LinkToMainWindow.ChangeStatusOfUser(2);
                        LinkToMainWindow.ChangeRegistraitedUserInfo(AuthorisedUser);
                    }

                    return;
                }
            }
            MessageBox.Show("Похоже такого пользователя нет...");
        }
        private void CreatePDFOfTickets(int TicketNomber, TicketModel Ticket, CruisesForWindowInfo CruiseInfo)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            gfx.DrawString("Билет на рейс", new XFont("Arial", 40, XFontStyle.Bold), XBrushes.Black, new XPoint(150, 70));

            int currentYPosition = 180;

            gfx.DrawString("Информация о билете", new XFont("Arial", 30, XFontStyle.Bold), XBrushes.Black, new XPoint(120, currentYPosition));
            currentYPosition += 50;
            gfx.DrawString("Номер билета в очереди заказа: " + TicketNomber.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("ФИО: " + Ticket.FullName, new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Индентификационная информация: " + Ticket.IdentificationInformation, new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Номер сиденья: " + Ticket.SeatNumberOnTheTransport.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Дата отправки рейса: " + Ticket.RaceDepartureTime.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 150, 0)), new XPoint(50, currentYPosition), new XPoint(500, currentYPosition));
            currentYPosition += 40;

            gfx.DrawString("Информация о рейсе", new XFont("Arial", 30, XFontStyle.Bold), XBrushes.Black, new XPoint(120, currentYPosition));
            currentYPosition += 50;
            gfx.DrawString("Пункт начала движения: " + CruiseInfo.Cruise.StartPointLocalityName.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Пункт начала движения: " + CruiseInfo.Cruise.EndPointLocalityName.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Цена в Рублях: " + CruiseInfo.Cruise.FullPrice.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Время поездки: " + CruiseInfo.Cruise.FullTimeInCruise.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 150, 0)), new XPoint(50, currentYPosition), new XPoint(500, currentYPosition));
            currentYPosition += 40;

            gfx.DrawString("Информация о транспорте", new XFont("Arial", 30, XFontStyle.Bold), XBrushes.Black, new XPoint(120, currentYPosition));
            currentYPosition += 50;
            gfx.DrawString("Кол-во мест в транспорте: " + CheckedCruiseToBuy.Cruise.TransportOfTheCruise.NumberOfSeats.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Регистрационный номер: " + CheckedCruiseToBuy.Cruise.TransportOfTheCruise.RegistrationNumber, new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Марка Авто: " + CheckedCruiseToBuy.Cruise.TransportOfTheCruise.Model, new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));

            DateTime DateNow = DateTime.Now;
            string DateTimeString = DateNow.Day.ToString() + DateNow.Month.ToString() + DateNow.Year.ToString();
            Random rnd = new Random();

            document.Save(@"E:\\УНИВЕР\\3-41(5 семестр (3 курс))(Смирнов)\\Конструирование ПО\\Интерфейс\\Интерфейс\\Tickets\\Ticket" + TicketNomber.ToString() + "-" + DateTimeString + "-" + rnd.Next(100000, 999999).ToString() + ".pdf");
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
            FindeCruisesForRoute();
            FindeAditionalInfoForCruises();
            InitialiseStartDateOfCruises(SelectedDateFromMainWindow);
            FindeCruiseTransport();
            FindeFreeSeatsForCruises();
            InitialiseCruiseStringInfoForWindow();
        }

        private void FindeCruisesForRoute()
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

        private void FindeCruiseTransport()
        {
            for (int i = 0; i < allTransport.Count; ++i)
            {
                for (int j = 0; j < allPossibleCruises.Count; ++j)
                {
                    if (allPossibleCruises[j].TransportIDOfTheCruise == allTransport[i].ID)
                    {
                        allPossibleCruises[j].TransportOfTheCruise = allTransport[i];
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

        private void InitialiseStartDateOfCruises(DateTime SelectedDateFromMainWindow)
        {
            DateTime DateTimeForCruise = SelectedDateFromMainWindow;
            DateTimeForCruise = new DateTime(DateTimeForCruise.Year, DateTimeForCruise.Month, DateTimeForCruise.Day, 0, 0, 0);

            for (int i = 0; i <= MaxDayForOrderingPossibility; ++i)
            {
                if (i != 0)
                {
                    DateTimeForCruise = DateTimeForCruise.AddDays(+1);
                    DateTimeForCruise = new DateTime(DateTimeForCruise.Year, DateTimeForCruise.Month, DateTimeForCruise.Day, 0, 0, 0);
                }
                int DayOfTheWeekForCruise = ParseDaysOfTheWeek(DateTimeForCruise);

                InitialaiseCruises(DateTimeForCruise, DayOfTheWeekForCruise);

            }
        }
        private void InitialaiseCruises(DateTime DateTimeForCruise, int DayOfTheWeekForCruise)
        {
            for (int j = 0; j < allPossibleCruises.Count; ++j)
            {
                CruisesForWindowInfo CruiseForWindowToCreate = new CruisesForWindowInfo(allPossibleCruises[j]);

                bool TodayIsTheDayOfCruise = allPossibleCruises[j].DayOfTheWeekCruiseID == DayOfTheWeekForCruise;
                DateTime NowMoment = DateTime.Now;
                TimeSpan CruiseStartTime = (TimeSpan)CruiseForWindowToCreate.Cruise.StartTime;
                DateTime DateTimeForCruiseCalculations = new DateTime(DateTimeForCruise.Year, DateTimeForCruise.Month, DateTimeForCruise.Day).Add(CruiseStartTime);
                TimeSpan DateDifference = DateTimeForCruiseCalculations - NowMoment;
                bool DifferenceInNowTimeAndCruiseStartTime = DateDifference > MinimumTimeForOrderingTicket;

                if ((TodayIsTheDayOfCruise) && (DifferenceInNowTimeAndCruiseStartTime))
                {
                    CruiseForWindowToCreate.SetStartDate(DateTimeForCruise.Add((TimeSpan)CruiseForWindowToCreate.Cruise.StartTime));
                    bool FlagOfHaving = CheckIfThisCruiseAlreadyExist(CruiseForWindowToCreate);
                    
                    if (FlagOfHaving == false)
                    {
                        allCruisesForWindow.Add(CruiseForWindowToCreate);
                    }
                }
            }
        }
        private bool CheckIfThisCruiseAlreadyExist(CruisesForWindowInfo CruiseForWindowToCreate)
        {
            for (int k = 0; k < allCruisesForWindow.Count; ++k)
            {
                if ((allCruisesForWindow[k].StartDate == CruiseForWindowToCreate.StartDate) &&
                    (allCruisesForWindow[k].Cruise.StartPointLocalityID == CruiseForWindowToCreate.Cruise.StartPointLocalityID) &&
                    (allCruisesForWindow[k].Cruise.EndPointLocalityID == CruiseForWindowToCreate.Cruise.EndPointLocalityID))
                {
                    return (true);
                }
            }
            return (false);
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
            SetAmountOfFreeSeatsAsTransportSeats();

            // Работа с билетами
            FindeOcupiedSeats();
        }
        private void SetAmountOfFreeSeatsAsTransportSeats()
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
        }
        private void FindeOcupiedSeats()
        {
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
