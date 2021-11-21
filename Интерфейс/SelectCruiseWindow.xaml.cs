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
        class CruiseForWindowInfo
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

            public int AmountOfFreeSeats;

            public CruiseForWindowInfo(CruiseModel CruiseInfo, FindeRouteForCruises.FinalResult RouteInfo)
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

        private DBDataOperations DBComunication;

        private List<CruiseForWindowInfo> allNidedCruises = new List<CruiseForWindowInfo>();

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

        const int minimalPriceOfCruise = 50;

        int IDOfStartingLocation;
        int IDOfEndLocation;

        public SelectCruiseWindow(DBDataOperations DBComunicationFromMainWindow, int IDOfStartingLocationFromMainWindow, int IDOfEndLocationFromMainWindow)
        {
            DBComunication = DBComunicationFromMainWindow;
            IDOfStartingLocation = IDOfStartingLocationFromMainWindow;
            IDOfEndLocation = IDOfEndLocationFromMainWindow;

            InitializeComponent();
            LoadAllInformationFromDataBase();
            FindeRoutesForCruises();
            FindeAditionalInfoForCruises();
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
            CruiseForWindowInfo CruiseToAdd;

            for (int i = 0; i < Routes.Count; ++i)
            {
                for (int j = 0; j < allCruise.Count; ++j)
                {
                    if (Routes[i].RouteID == allCruise[j].RouteIDOfTheCruise)
                    {
                        CruiseToAdd = new CruiseForWindowInfo(allCruise[j], Routes[i]);
                        allNidedCruises.Add(CruiseToAdd);
                    }
                }
            }
        }
        private void FindeAditionalInfoForCruises()
        {
            for (int i = 0; i < allNidedCruises.Count; ++i)
            {
                for (int j = 0; j < allRoute.Count; ++j)
                {
                    if (allNidedCruises[i].RouteIDOfTheCruise == allRoute[j].ID)
                    {
                        List<FindeAditionalInformationForCruise.StoredProcedureResult> AdditionalInfoForCruises =
                            FindeAditionalInformationForCruise.StoredProcedureExecute(allNidedCruises[i].RouteIDOfTheCruise, 
                                                                                        allNidedCruises[i].StartPointIndex, 
                                                                                        allNidedCruises[i].EndPointIndex, 
                                                                                        minimalPriceOfCruise);
                        // В этом объекте всегда может быть только одна строка
                        allNidedCruises[i].AddPriceAndTimeInfo(AdditionalInfoForCruises[0]);
                    }
                }
            }
        }
    }
}
