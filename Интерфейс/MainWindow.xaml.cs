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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL.Models;
using BLL.DBInteraction;

namespace Интерфейс
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBDataOperations DBComunication = new DBDataOperations();

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

        public MainWindow()
        {
            InitializeComponent();
            LoadAllInformationFromDataBase();
            InsertInformationInListViews();
        }

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

        private void InsertInformationInListViews()
        {
            InsertInformationInCruiseListView();
            InsertInformationInDayOfTheWeekListView();
            InsertInformationInDriverListView();
            InsertInformationInLocalityListView();
            InsertInformationInRouteListView();
            InsertInformationInStoppingOnTheRouteListView();
            InsertInformationInStopSequencesListView();
            InsertInformationInTicketListView();
            InsertInformationInTransportListView();
            InsertInformationInUserListView();
        }
        private void InsertInformationInCruiseListView()
        {
            CruiseList.ItemsSource = allCruise;
        }
        private void InsertInformationInDayOfTheWeekListView()
        {
            DayOfTheWeekList.ItemsSource = allDayOfTheWeek;
        }
        private void InsertInformationInDriverListView()
        {
            DriverList.ItemsSource = allDriver;
        }
        private void InsertInformationInLocalityListView()
        {
            LocalityList.ItemsSource = allLocality;
        }
        private void InsertInformationInRouteListView()
        {
            RouteList.ItemsSource = allRoute;
        }
        private void InsertInformationInStoppingOnTheRouteListView()
        {
            StoppingOnTheRouteList.ItemsSource = allStoppingOnTheRoute;
        }
        private void InsertInformationInStopSequencesListView()
        {
            StopSequencesList.ItemsSource = allStopSequences;
        }
        private void InsertInformationInTicketListView()
        {
            TicketList.ItemsSource = allTicket;
        }
        private void InsertInformationInTransportListView()
        {
            TransportList.ItemsSource = allTransport;
        }
        private void InsertInformationInUserListView()
        {
            UsersList.ItemsSource = allUser;
        }

    }

}