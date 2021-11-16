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
using Интерфейс.CeateUpdateWindows;

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

        private int StatusLevelOfUser = 0;

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
            CruiseDataGrid.ItemsSource = allCruise;
        }
        private void InsertInformationInDayOfTheWeekListView()
        {
            DayOfTheWeekDataGrid.ItemsSource = allDayOfTheWeek;
        }
        private void InsertInformationInDriverListView()
        {
            DriverDataGrid.ItemsSource = allDriver;
        }
        private void InsertInformationInLocalityListView()
        {
            LocalityDataGrid.ItemsSource = allLocality;
        }
        private void InsertInformationInRouteListView()
        {
            RouteDataGrid.ItemsSource = allRoute;
        }
        private void InsertInformationInStoppingOnTheRouteListView()
        {
            StoppingOnTheRouteDataGrid.ItemsSource = allStoppingOnTheRoute;
        }
        private void InsertInformationInStopSequencesListView()
        {
            StopSequencesDataGrid.ItemsSource = allStopSequences;
        }
        private void InsertInformationInTicketListView()
        {
            TicketDataGrid.ItemsSource = allTicket;
        }
        private void InsertInformationInTransportListView()
        {
            TransportDataGrid.ItemsSource = allTransport;
        }
        private void InsertInformationInUserListView()
        {
            UsersDataGrid.ItemsSource = allUser;
        }

        private int getSelectedRow(DataGrid dataGrid)
        {
            int index = -1;
            if (dataGrid.SelectedItems.Count > 0 || dataGrid.SelectedCells.Count == 1)
            {
                // Проверка на кол - во выделенных элементов не нужна, ибо возвращается первый выделенный элемент
                index = UsersDataGrid.Items.IndexOf(UsersDataGrid.SelectedItem);
            }
            return index;
        }

        private void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            UserCUWindow CreateWindow = new UserCUWindow();

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return;
            else
            {
                UserModel NewObject = new UserModel();

                NewObject.FullName = CreateWindow.FullNameTextBox.Text;
                NewObject.Login = CreateWindow.LoginTextBox.Text;
                NewObject.Password = CreateWindow.PasswordTextBox.Text;
                NewObject.Status = CreateWindow.StatusIntegerUpDown.Value;

                DBComunication.User.Create(NewObject);
                allUser = DBComunication.User.GetAll();
                InsertInformationInUserListView();

                MessageBox.Show("Новый объект добавлен");
            }

        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            int index = getSelectedRow(UsersDataGrid);
            if (index != -1)
            {

                int id = 0;
                UserModel MarkedRow = (UserModel)UsersDataGrid.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                UserModel ph = allUser.Where(i => i.ID == id).FirstOrDefault();
                if (ph != null)
                {
                    UserCUWindow UpdateWindow = new UserCUWindow();

                    UpdateWindow.FullNameTextBox.Text = ph.FullName;
                    UpdateWindow.LoginTextBox.Text = ph.Login;
                    UpdateWindow.PasswordTextBox.Text = ph.Password;
                    UpdateWindow.StatusIntegerUpDown.Value = ph.Status;

                    bool? result = UpdateWindow.ShowDialog();
                    if (result == false)
                        return;
                    else 
                    {
                        ph.FullName = UpdateWindow.FullNameTextBox.Text;
                        ph.Login = UpdateWindow.LoginTextBox.Text;
                        ph.Password = UpdateWindow.PasswordTextBox.Text;
                        ph.Status = UpdateWindow.StatusIntegerUpDown.Value;

                        DBComunication.User.Update(ph);
                        allUser = DBComunication.User.GetAll();
                        InsertInformationInUserListView();

                        MessageBox.Show("Объект обновлен");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ни один объект не выбран!");
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            int index = getSelectedRow(UsersDataGrid);
            if (index != -1)
            {
                int id = 0;
                UserModel MarkedRow = (UserModel)UsersDataGrid.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.User.Delete(id);
                allUser = DBComunication.User.GetAll();
                InsertInformationInUserListView();
            }
        }
    }

}