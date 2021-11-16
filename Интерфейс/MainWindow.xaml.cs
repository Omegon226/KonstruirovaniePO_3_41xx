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
                TicketModel MarkedRow = (TicketModel)DataGridForDeleteOperation.Items[index];
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
            TransportCUWindow CreateWindow = new TransportCUWindow();

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return;
            else
            {
                TransportModel NewObject = new TransportModel();

                NewObject.NumberOfSeats = CreateWindow.NumberOfSeatsIntegerUpDown.Value;
                NewObject.RegistrationNumber = CreateWindow.RegistrationNumberTextBox.Text;
                NewObject.Model = CreateWindow.ModelTextBox.Text;
                NewObject.Hidden = CreateWindow.HiddenCheckBox.IsChecked;
               
                DBComunication.Transport.Create(NewObject);
                allTransport = DBComunication.Transport.GetAll();
                InsertInformationInTransportDataGrid();

                MessageBox.Show("Новый объект добавлен");
            }
        }
        private void UpdateTransportButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForUpdateOperation = TransportDataGrid;

            int index = getSelectedRow(DataGridForUpdateOperation);
            if (index != -1)
            {

                int id = 0;
                TransportModel MarkedRow = (TransportModel)DataGridForUpdateOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                TransportModel ph = allTransport.Where(i => i.ID == id).FirstOrDefault();
                if (ph != null)
                {
                    TransportCUWindow UpdateWindow = new TransportCUWindow();

                    UpdateWindow.NumberOfSeatsIntegerUpDown.Value = ph.NumberOfSeats;
                    UpdateWindow.RegistrationNumberTextBox.Text = ph.RegistrationNumber;
                    UpdateWindow.ModelTextBox.Text = ph.Model;
                    UpdateWindow.HiddenCheckBox.IsChecked = ph.Hidden;

                    bool? result = UpdateWindow.ShowDialog();
                    if (result == false)
                        return;
                    else
                    {
                        ph.NumberOfSeats = UpdateWindow.NumberOfSeatsIntegerUpDown.Value;
                        ph.RegistrationNumber = UpdateWindow.RegistrationNumberTextBox.Text;
                        ph.Model = UpdateWindow.ModelTextBox.Text;
                        ph.Hidden = UpdateWindow.HiddenCheckBox.IsChecked;

                        DBComunication.Transport.Update(ph);
                        allTransport = DBComunication.Transport.GetAll();
                        InsertInformationInTransportDataGrid();

                        MessageBox.Show("Объект обновлен");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ни один объект не выбран!");
            }
        }
        private void DeleteTransportButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = TransportDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                TransportModel MarkedRow = (TransportModel)DataGridForDeleteOperation.Items[index];
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
            StopSequencesCUWindow CreateWindow = new StopSequencesCUWindow();
            CreateWindow.StoppingIDComboBox.ItemsSource = allStoppingOnTheRoute;
            CreateWindow.StoppingIDComboBox.DisplayMemberPath = "ID";
            CreateWindow.StoppingIDComboBox.SelectedValuePath = "ID"; 
            CreateWindow.StopRouteIDComboBox.ItemsSource = allUser;
            CreateWindow.StopRouteIDComboBox.DisplayMemberPath = "FullName";
            CreateWindow.StopRouteIDComboBox.SelectedValuePath = "ID";

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return;
            else
            {
                StopSequencesModel NewObject = new StopSequencesModel();

                NewObject.IndexNumber = CreateWindow.IndexNumberIntegerUpDown.Value;
                NewObject.StoppingID = (int)CreateWindow.StoppingIDComboBox.SelectedValue;
                NewObject.StopRouteID = (int)CreateWindow.StopRouteIDComboBox.SelectedValue;
                NewObject.TripPrice = float.Parse(CreateWindow.TripPriceTextBox.Text);
                NewObject.TravelTimeToStop = TimeSpan.Parse(CreateWindow.TravelTimeToStopTextBox.Text);

                DBComunication.StopSequences.Create(NewObject);
                allStopSequences = DBComunication.StopSequences.GetAll();
                InsertInformationInStopSequencesDataGrid();

                MessageBox.Show("Новый объект добавлен");
            }
        }
        private void UpdateStopSequencesButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForUpdateOperation = StopSequencesDataGrid;

            int index = getSelectedRow(DataGridForUpdateOperation);
            if (index != -1)
            {

                int id = 0;
                StopSequencesModel MarkedRow = (StopSequencesModel)DataGridForUpdateOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                StopSequencesModel ph = allStopSequences.Where(i => i.ID == id).FirstOrDefault();
                if (ph != null)
                {
                    StopSequencesCUWindow UpdateWindow = new StopSequencesCUWindow();
                    UpdateWindow.StoppingIDComboBox.ItemsSource = allStoppingOnTheRoute;
                    UpdateWindow.StoppingIDComboBox.DisplayMemberPath = "ID";
                    UpdateWindow.StoppingIDComboBox.SelectedValuePath = "ID";
                    UpdateWindow.StopRouteIDComboBox.ItemsSource = allUser;
                    UpdateWindow.StopRouteIDComboBox.DisplayMemberPath = "FullName";
                    UpdateWindow.StopRouteIDComboBox.SelectedValuePath = "ID";

                    UpdateWindow.IndexNumberIntegerUpDown.Value = ph.IndexNumber;
                    UpdateWindow.StoppingIDComboBox.SelectedValue = ph.StoppingID;
                    UpdateWindow.StopRouteIDComboBox.SelectedValue = ph.StopRouteID;
                    UpdateWindow.TripPriceTextBox.Text = ph.TripPrice.ToString();
                    UpdateWindow.TravelTimeToStopTextBox.Text = ph.TravelTimeToStop.ToString();

                    bool? result = UpdateWindow.ShowDialog();
                    if (result == false)
                        return;
                    else
                    {
                        ph.IndexNumber = UpdateWindow.IndexNumberIntegerUpDown.Value;
                        ph.StoppingID = (int)UpdateWindow.StoppingIDComboBox.SelectedValue;
                        ph.StopRouteID = (int)UpdateWindow.StopRouteIDComboBox.SelectedValue;
                        ph.TripPrice = float.Parse(UpdateWindow.TripPriceTextBox.Text);
                        ph.TravelTimeToStop = TimeSpan.Parse(UpdateWindow.TravelTimeToStopTextBox.Text);

                        DBComunication.StopSequences.Update(ph);
                        allStopSequences = DBComunication.StopSequences.GetAll();
                        InsertInformationInStopSequencesDataGrid();

                        MessageBox.Show("Объект обновлен");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ни один объект не выбран!");
            }
        }
        private void DeleteStopSequencesButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = StopSequencesDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                StopSequencesModel MarkedRow = (StopSequencesModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.StopSequences.Delete(id);
                allStopSequences = DBComunication.StopSequences.GetAll();
                InsertInformationInStopSequencesDataGrid();
            }
        }

        // CRUD Для StoppingOnTheRoute
        private void CreateStoppingOnTheRouteButton_Click(object sender, RoutedEventArgs e)
        {
            StoppingOnTheRouteCUWindow CreateWindow = new StoppingOnTheRouteCUWindow();
            CreateWindow.StopLocalityIDComboBox.ItemsSource = allLocality;
            CreateWindow.StopLocalityIDComboBox.DisplayMemberPath = "Name";
            CreateWindow.StopLocalityIDComboBox.SelectedValuePath = "ID";

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return;
            else
            {
                StoppingOnTheRouteModel NewObject = new StoppingOnTheRouteModel();

                NewObject.StopLocalityID = (int)CreateWindow.StopLocalityIDComboBox.SelectedValue;

                DBComunication.StoppingOnTheRoute.Create(NewObject);
                allStoppingOnTheRoute = DBComunication.StoppingOnTheRoute.GetAll();
                InsertInformationInStoppingOnTheRouteDataGrid();

                MessageBox.Show("Новый объект добавлен");
            }
        }
        private void UpdateStoppingOnTheRouteButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForUpdateOperation = StoppingOnTheRouteDataGrid;

            int index = getSelectedRow(DataGridForUpdateOperation);
            if (index != -1)
            {

                int id = 0;
                StoppingOnTheRouteModel MarkedRow = (StoppingOnTheRouteModel)DataGridForUpdateOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                StoppingOnTheRouteModel ph = allStoppingOnTheRoute.Where(i => i.ID == id).FirstOrDefault();
                if (ph != null)
                {
                    StoppingOnTheRouteCUWindow UpdateWindow = new StoppingOnTheRouteCUWindow();
                    UpdateWindow.StopLocalityIDComboBox.ItemsSource = allLocality;
                    UpdateWindow.StopLocalityIDComboBox.DisplayMemberPath = "Name";
                    UpdateWindow.StopLocalityIDComboBox.SelectedValuePath = "ID";

                    UpdateWindow.StopLocalityIDComboBox.SelectedValue = ph.StopLocalityID;

                    bool? result = UpdateWindow.ShowDialog();
                    if (result == false)
                        return;
                    else
                    {
                        ph.StopLocalityID = (int)UpdateWindow.StopLocalityIDComboBox.SelectedValue;

                        DBComunication.StoppingOnTheRoute.Update(ph);
                        allStoppingOnTheRoute = DBComunication.StoppingOnTheRoute.GetAll();
                        InsertInformationInStoppingOnTheRouteDataGrid();

                        MessageBox.Show("Объект обновлен");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ни один объект не выбран!");
            }
        }
        private void DeleteStoppingOnTheRouteButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForDeleteOperation = StoppingOnTheRouteDataGrid;
            int index = getSelectedRow(DataGridForDeleteOperation);

            if (index != -1)
            {
                int id = 0;
                StoppingOnTheRouteModel MarkedRow = (StoppingOnTheRouteModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.StoppingOnTheRoute.Delete(id);
                allStoppingOnTheRoute = DBComunication.StoppingOnTheRoute.GetAll();
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
                RouteModel MarkedRow = (RouteModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.Route.Delete(id);
                allRoute = DBComunication.Route.GetAll();
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
                LocalityModel MarkedRow = (LocalityModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.Locality.Delete(id);
                allLocality = DBComunication.Locality.GetAll();
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
                DriverModel MarkedRow = (DriverModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.Driver.Delete(id);
                allDriver = DBComunication.Driver.GetAll();
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
                DayOfTheWeekModel MarkedRow = (DayOfTheWeekModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.DayOfTheWeek.Delete(id);
                allDayOfTheWeek = DBComunication.DayOfTheWeek.GetAll();
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
                CruiseModel MarkedRow = (CruiseModel)DataGridForDeleteOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DBComunication.Cruise.Delete(id);
                allCruise = DBComunication.Cruise.GetAll();
                InsertInformationInCruiseDataGrid();
            }
        }
    }
}