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

namespace Интерфейс
{
    /// <summary>
    /// Логика взаимодействия для SelectCruiseWindow.xaml
    /// </summary>
    public partial class SelectCruiseWindow : Window
    {
        class PossibleCruises
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

        class CruisesForWindowInfo
        {
            public PossibleCruises Cruises;

            public DateTime StartDate;
            public int AmountOfFreeSeats;

            public CruisesForWindowInfo(PossibleCruises PossibleCruises)
            {
                this.Cruises = PossibleCruises;
            }
            public void SetStartDate(DateTime Date)
            {
                this.StartDate = Date;
            }
        }

        private DBDataOperations DBComunication;

        private List<PossibleCruises> allPossibleCruises = new List<PossibleCruises>();
        private List<CruisesForWindowInfo> allCruisesForWindow = new List<CruisesForWindowInfo>();

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

        private int StatusLevelOfUser = 0;

        private int IDOfStartingLocation;
        private int IDOfEndLocation;

        public SelectCruiseWindow(DBDataOperations DBComunicationFromMainWindow, int IDOfStartingLocationFromMainWindow, int IDOfEndLocationFromMainWindow, int UserStausLevel)
        {
            DBComunication = DBComunicationFromMainWindow;
            IDOfStartingLocation = IDOfStartingLocationFromMainWindow;
            IDOfEndLocation = IDOfEndLocationFromMainWindow;
            StatusLevelOfUser = UserStausLevel;

            InitializeComponent();
            LoadAllInformationFromDataBase();
            FindeRoutesForCruises();
            FindeAditionalInfoForCruises();
            InitioliseStartDateOfCruises();
            FindeFreeSeatsForCruises();
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
            CruisesForWindowInfo CruiseForWindowToCreate;

            for (int i = 0; i <= MaxDayForOrderingPossibility; ++i)
            {
                if (i != 0)
                {
                    DateTimeForCruise = DateTimeForCruise.AddDays(+1);
                }
                int DayOfTheWeekForCruise = ParseDaysOfTheWeek(DateTimeForCruise);

                for (int j = 0; j < allPossibleCruises.Count; ++j)
                {
                    if (allPossibleCruises[j].DayOfTheWeekCruiseID == DayOfTheWeekForCruise)
                    {
                        CruiseForWindowToCreate = new CruisesForWindowInfo(allPossibleCruises[j]);
                        CruiseForWindowToCreate.SetStartDate(DateTimeForCruise);
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
                    if (allCruisesForWindow[i].Cruises.RouteIDOfTheCruise == allTransport[j].ID)
                    {
                        //allPossibleCruises[i].AmountOfFreeSeats = (int)allTransport[j].NumberOfSeats;
                    }
                }
            }

            // Доработать с билетами

        }
    }
}
