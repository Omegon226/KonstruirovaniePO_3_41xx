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

        #region --- Подгрузка информации в таблицы для администрирования

        private void InsertInformationInListViews()
        {
            InsertInformationInCruiseDataGrid();
            InsertInformationInDayOfTheWeekDataGrid();
            InsertInformationInDriverDataGrid();
            InsertInformationInLocalityDataGrid();
            InsertInformationInRouteDataGrid();
            InsertInformationInStoppingOnTheRouteDataGrid();
            InsertInformationInStopSequencesDataGrid();
            InsertInformationInTicketDataGrid();
            InsertInformationInTransportDataGrid();
            InsertInformationInUserDataGrid();
        }
        private void InsertInformationInCruiseDataGrid()
        {
            CruiseDataGrid.ItemsSource = allCruise;
        }
        private void InsertInformationInDayOfTheWeekDataGrid()
        {
            DayOfTheWeekDataGrid.ItemsSource = allDayOfTheWeek;
        }
        private void InsertInformationInDriverDataGrid()
        {
            DriverDataGrid.ItemsSource = allDriver;
        }
        private void InsertInformationInLocalityDataGrid()
        {
            LocalityDataGrid.ItemsSource = allLocality;
        }
        private void InsertInformationInRouteDataGrid()
        {
            RouteDataGrid.ItemsSource = allRoute;
        }
        private void InsertInformationInStoppingOnTheRouteDataGrid()
        {
            StoppingOnTheRouteDataGrid.ItemsSource = allStoppingOnTheRoute;
        }
        private void InsertInformationInStopSequencesDataGrid()
        {
            StopSequencesDataGrid.ItemsSource = allStopSequences;
        }
        private void InsertInformationInTicketDataGrid()
        {
            TicketDataGrid.ItemsSource = allTicket;
        }
        private void InsertInformationInTransportDataGrid()
        {
            TransportDataGrid.ItemsSource = allTransport;
        }
        private void InsertInformationInUserDataGrid()
        {
            UsersDataGrid.ItemsSource = allUser;
        }

        #endregion



        private int getSelectedRow(DataGrid dataGrid)
        {
            int index = -1;
            if (dataGrid.SelectedItems.Count > 0 || dataGrid.SelectedCells.Count == 1)
            {
                // Проверка на кол - во выделенных элементов не нужна, ибо возвращается первый выделенный элемент
                index = dataGrid.Items.IndexOf(dataGrid.SelectedItem);
            }
            return index;
        }

        // CRUD Для User
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
                InsertInformationInUserDataGrid();

                MessageBox.Show("Новый объект добавлен");
            }

        }
        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForUpdateOperation = UsersDataGrid;

            int index = getSelectedRow(DataGridForUpdateOperation);
            if (index != -1)
            {

                int id = 0;
                UserModel MarkedRow = (UserModel)DataGridForUpdateOperation.Items[index];
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
                        InsertInformationInUserDataGrid();

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
            DataGrid DataGridForDeleteOperation = UsersDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                UserModel MarkedRow = (UserModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.User.Delete(id);
                allUser = DBComunication.User.GetAll();
                InsertInformationInUserDataGrid();
            }
        }

        // CRUD Для Ticket
        private void CreateTicketButton_Click(object sender, RoutedEventArgs e)
        {
            var c = allUser;

            TicketCUWindow CreateWindow = new TicketCUWindow();
            CreateWindow.CruiseIDComboBox.ItemsSource = allCruise;
            CreateWindow.CruiseIDComboBox.DisplayMemberPath = "ID";
            CreateWindow.CruiseIDComboBox.SelectedValuePath = "ID";
            CreateWindow.UserIDComboBox.ItemsSource = allUser;
            CreateWindow.UserIDComboBox.DisplayMemberPath = "FullName";
            CreateWindow.UserIDComboBox.SelectedValuePath = "ID";

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return;
            else
            {
                TicketModel NewObject = new TicketModel();

                NewObject.DateOfIssue = DateTime.Parse(CreateWindow.DateOfIssueTextBox.Text);
                NewObject.IdentificationInformation = CreateWindow.IndentificationInformationTextBox.Text;
                NewObject.SeatNumberOnTheTransport = CreateWindow.SeatNumberOnTheTransportIntegerUpDown.Value;
                NewObject.FullName = CreateWindow.FullNameTextBox.Text;
                NewObject.CruiseID = (int)CreateWindow.CruiseIDComboBox.SelectedValue;
                NewObject.UserID = (int)CreateWindow.UserIDComboBox.SelectedValue;
                NewObject.RaceDepartureTime = DateTime.Parse(CreateWindow.RaceDepartureTimeTextBox.Text);

                DBComunication.Ticket.Create(NewObject);
                allTicket = DBComunication.Ticket.GetAll();
                InsertInformationInTicketDataGrid();

                MessageBox.Show("Новый объект добавлен");
            }
        }
        private void UpdateTicketButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForUpdateOperation = TicketDataGrid;

            int index = getSelectedRow(DataGridForUpdateOperation);
            if (index != -1)
            {

                int id = 0;
                TicketModel MarkedRow = (TicketModel)DataGridForUpdateOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                TicketModel ph = allTicket.Where(i => i.ID == id).FirstOrDefault();
                if (ph != null)
                {
                    TicketCUWindow UpdateWindow = new TicketCUWindow();
                    UpdateWindow.CruiseIDComboBox.ItemsSource = allCruise;
                    UpdateWindow.CruiseIDComboBox.DisplayMemberPath = "ID";
                    UpdateWindow.CruiseIDComboBox.SelectedValuePath = "ID";
                    UpdateWindow.UserIDComboBox.ItemsSource = allUser;
                    UpdateWindow.UserIDComboBox.DisplayMemberPath = "FullName";
                    UpdateWindow.UserIDComboBox.SelectedValuePath = "ID";

                    UpdateWindow.DateOfIssueTextBox.Text = ph.DateOfIssue.ToString();
                    UpdateWindow.IndentificationInformationTextBox.Text = ph.IdentificationInformation;
                    UpdateWindow.SeatNumberOnTheTransportIntegerUpDown.Value = ph.SeatNumberOnTheTransport;
                    UpdateWindow.FullNameTextBox.Text = ph.FullName;
                    UpdateWindow.CruiseIDComboBox.SelectedValue = ph.CruiseID;
                    UpdateWindow.UserIDComboBox.SelectedValue = ph.UserID;
                    UpdateWindow.RaceDepartureTimeTextBox.Text = ph.RaceDepartureTime.ToString();

                    bool? result = UpdateWindow.ShowDialog();
                    if (result == false)
                        return;
                    else
                    {
                        ph.DateOfIssue = DateTime.Parse(UpdateWindow.DateOfIssueTextBox.Text);
                        ph.IdentificationInformation = UpdateWindow.IndentificationInformationTextBox.Text;
                        ph.SeatNumberOnTheTransport = UpdateWindow.SeatNumberOnTheTransportIntegerUpDown.Value;
                        ph.FullName = UpdateWindow.FullNameTextBox.Text;
                        ph.CruiseID = (int)UpdateWindow.CruiseIDComboBox.SelectedValue;
                        ph.UserID = (int)UpdateWindow.UserIDComboBox.SelectedValue;
                        ph.RaceDepartureTime = DateTime.Parse(UpdateWindow.RaceDepartureTimeTextBox.Text);

                        DBComunication.Ticket.Update(ph);
                        allTicket = DBComunication.Ticket.GetAll();
                        InsertInformationInTicketDataGrid();

                        MessageBox.Show("Объект обновлен");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ни один объект не выбран!");
            }
        }
        private void DeleteTicketButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = TicketDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                UserModel MarkedRow = (UserModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.Ticket.Delete(id);
                allTicket = DBComunication.Ticket.GetAll();
                InsertInformationInTicketDataGrid();
            }
        }

        // CRUD Для Transport
        private void CreateTransportButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateTransportButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteTransportButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = TransportDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                UserModel MarkedRow = (UserModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.User.Delete(id);
                allUser = DBComunication.User.GetAll();
                InsertInformationInTransportDataGrid();
            }
        }

        // CRUD Для StopSequences
        private void CreateStopSequencesButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateStopSequencesButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteStopSequencesButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = StopSequencesDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                UserModel MarkedRow = (UserModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.User.Delete(id);
                allUser = DBComunication.User.GetAll();
                InsertInformationInStopSequencesDataGrid();
            }
        }

        // CRUD Для StoppingOnTheRoute
        private void CreateStoppingOnTheRouteButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateStoppingOnTheRouteButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteStoppingOnTheRouteButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = StoppingOnTheRouteDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                UserModel MarkedRow = (UserModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.User.Delete(id);
                allUser = DBComunication.User.GetAll();
                InsertInformationInStoppingOnTheRouteDataGrid();
            }
        }

        // CRUD Для RouteButton
        private void CreateRouteButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateRouteButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteRouteButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = RouteDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                UserModel MarkedRow = (UserModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.User.Delete(id);
                allUser = DBComunication.User.GetAll();
                InsertInformationInRouteDataGrid();
            }
        }

        // CRUD Для Locality
        private void CreateLocalityButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateLocalityButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteLocalityButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = LocalityDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                UserModel MarkedRow = (UserModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.User.Delete(id);
                allUser = DBComunication.User.GetAll();
                InsertInformationInLocalityDataGrid();
            }
        }

        // CRUD Для Driver
        private void CreateDriverButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateDriverButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteDriverButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = DriverDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                UserModel MarkedRow = (UserModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.User.Delete(id);
                allUser = DBComunication.User.GetAll();
                InsertInformationInDriverDataGrid();
            }
        }

        // CRUD Для DayOfTheWeek
        private void CreateDayOfTheWeekButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateDayOfTheWeekButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteDayOfTheWeekButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = DayOfTheWeekDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                UserModel MarkedRow = (UserModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.User.Delete(id);
                allUser = DBComunication.User.GetAll();
                InsertInformationInDayOfTheWeekDataGrid();
            }
        }

        // CRUD Для Cruise
        private void CreateCruiseButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateCruiseButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteCruiseButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = CruiseDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                UserModel MarkedRow = (UserModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.User.Delete(id);
                allUser = DBComunication.User.GetAll();
                InsertInformationInCruiseDataGrid();
            }
        }
    }
}