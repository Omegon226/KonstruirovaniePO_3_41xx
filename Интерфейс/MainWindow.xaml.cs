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
using LiveCharts;
using LiveCharts.Wpf;
using BLL.Models;
using BLL.DBInteraction;
using BLL.Services;
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
        private UserModel AuthorisedUser;

        public MainWindow()
        {
            InitializeComponent();
            LoadAllInformationFromDataBase();
            InsertInformationInCRUDDataGrids();
            InsertInformationInReportsComboBoxes();
            InsertInformationInChartsTab();
            BuildCharts();
            InsertInformationInFindeRouteComboBoxes();

            FindeRouteGrid.Visibility = Visibility.Visible;
            CreateReportsGrid.Visibility = Visibility.Hidden;
            CreateChartsGrid.Visibility = Visibility.Hidden;

            CheckUserPrivileges();
        }

        private void InsertInformationInFindeRouteComboBoxes()
        {
            DepartureLocalityComboBox.ItemsSource = allLocality;
            DepartureLocalityComboBox.DisplayMemberPath = "Name";
            DepartureLocalityComboBox.SelectedValuePath = "ID";

            DepartureLocalityComboBox.SelectedItem = allLocality[0];

            ArrivalLocalityComboBox.ItemsSource = allLocality;
            ArrivalLocalityComboBox.DisplayMemberPath = "Name";
            ArrivalLocalityComboBox.SelectedValuePath = "ID";

            ArrivalLocalityComboBox.SelectedItem = allLocality[6];
        }

        private void CheckUserPrivileges()
        {
            if (StatusLevelOfUser == 0)
            {
                FindeRouteTabOpenButton.Visibility = Visibility.Visible;
                CreateReportsTabOpenButton.Visibility = Visibility.Hidden;
                CreateChartsTabOpenButton.Visibility = Visibility.Hidden;

                if ((CreateReportsGrid.Visibility == Visibility.Visible) || (CreateChartsGrid.Visibility == Visibility.Visible))
                {
                    ReturnToFindeRouteGrid();
                }

                return;
            }
            if (StatusLevelOfUser == 1)
            {
                FindeRouteTabOpenButton.Visibility = Visibility.Visible;
                CreateReportsTabOpenButton.Visibility = Visibility.Hidden;
                CreateChartsTabOpenButton.Visibility = Visibility.Hidden;

                if ((CreateReportsGrid.Visibility == Visibility.Visible) || (CreateChartsGrid.Visibility == Visibility.Visible))
                {
                    ReturnToFindeRouteGrid();
                }

                return;
            }
            if (StatusLevelOfUser == 2)
            {
                FindeRouteTabOpenButton.Visibility = Visibility.Visible;
                CreateReportsTabOpenButton.Visibility = Visibility.Visible;
                CreateChartsTabOpenButton.Visibility = Visibility.Visible;
                return;
            }
        }
        private void ReturnToFindeRouteGrid()
        {
            FindeRouteGrid.Visibility = Visibility.Visible;
            CreateReportsGrid.Visibility = Visibility.Hidden;
            CreateChartsGrid.Visibility = Visibility.Hidden;
        }

        private void AuthorisetionButton_Click(object sender, RoutedEventArgs e)
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

                FindeSameUser(AuthorizedUser);
                CheckUserPrivileges();
            }
        }
        private void FindeSameUser(UserModel UserToFinde)
        {
            for (int i = 0; i < allUser.Count; ++i)
            {
                if ((UserToFinde.Login == allUser[i].Login) && (UserToFinde.Password == allUser[i].Password))
                {
                    StatusLevelOfUser = (int)allUser[i].Status;
                    AuthorisedUser = allUser[i];
                    AuthorisetionButton.Visibility = Visibility.Hidden;
                    DeauthorisetionButton.Visibility = Visibility.Visible;
                    
                    if (allUser[i].Status == 1)
                    {
                        MessageBox.Show("Вход в систему осуществлён! Вы являетесь пользователем.");
                    }
                    if (allUser[i].Status == 2)
                    {
                        MessageBox.Show("Вход в систему осуществлён! Вы являетесь администратором.");
                    }

                    return;
                }
            }
            MessageBox.Show("Похоже такого пользователя нет...");
        }

        private void DeauthorisetionButton_Click(object sender, RoutedEventArgs e)
        {
            StatusLevelOfUser = 0;
            AuthorisetionButton.Visibility = Visibility.Visible;
            DeauthorisetionButton.Visibility = Visibility.Hidden;
            MessageBox.Show("Вы вышли из системы");
            CheckUserPrivileges();
        }

        private void FindeRouteTabOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FindeRouteGrid.Visibility = Visibility.Visible;
            CreateReportsGrid.Visibility = Visibility.Hidden;
            CreateChartsGrid.Visibility = Visibility.Hidden;
        }

        private void CreateReportsTabOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FindeRouteGrid.Visibility = Visibility.Hidden;
            CreateReportsGrid.Visibility = Visibility.Visible;
            CreateChartsGrid.Visibility = Visibility.Hidden;
        }

        private void CreateChartsTabOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FindeRouteGrid.Visibility = Visibility.Hidden;
            CreateReportsGrid.Visibility = Visibility.Hidden;
            CreateChartsGrid.Visibility = Visibility.Visible;
        }

        private void FindeRouteButton_Click(object sender, RoutedEventArgs e)
        {
            int StartPoint = (int)DepartureLocalityComboBox.SelectedValue;
            int EndPoint = (int)ArrivalLocalityComboBox.SelectedValue;

            if (StartPoint == EndPoint)
            {
                MessageBox.Show("Начальный и конечный пункты не должны быть одинаковыми");
                return;
            }

            SelectCruiseWindow WindowToFindeCruise = new SelectCruiseWindow(DBComunication, StartPoint, EndPoint, StatusLevelOfUser, AuthorisedUser);
            WindowToFindeCruise.Show();
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

        private void InsertInformationInCRUDDataGrids()
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

        private void InsertInformationInReportsComboBoxes()
        {
            InsertInformationInReport1ComboBoxes();
            InsertInformationInReport3ComboBoxes();
        }

        private void InsertInformationInReport1ComboBoxes()
        {
            Report1LastStoppingComboBox.ItemsSource = allLocality;
            Report1LastStoppingComboBox.DisplayMemberPath = "Name";
            Report1LastStoppingComboBox.SelectedValuePath = "ID";
        }
        private void InsertInformationInReport3ComboBoxes()
        {
            Report3StartingLocationComboBox.ItemsSource = allLocality;
            Report3StartingLocationComboBox.DisplayMemberPath = "Name";
            Report3StartingLocationComboBox.SelectedValuePath = "ID";

            Report3LastLocationComboBox.ItemsSource = allLocality;
            Report3LastLocationComboBox.DisplayMemberPath = "Name";
            Report3LastLocationComboBox.SelectedValuePath = "ID";
        }

        #endregion

        #region --- Функции для работы с Отчётами и CRUD Администратора

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

                NewObject.FullName = CreateWindow.SurnameTextBox.Text + " " + CreateWindow.NameTextBox.Text + " " + CreateWindow.PatronymicTextBox.Text;
                NewObject.Login = CreateWindow.LoginTextBox.Text;
                NewObject.Password = CreateWindow.PasswordTextBox.Text;
                if (CreateWindow.StatusUserRadioButton.IsChecked == true)
                {
                    NewObject.Status = 1;
                }
                if (CreateWindow.StatusAdministratorRadioButton.IsChecked == true)
                {
                    NewObject.Status = 2;
                }

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

                    UpdateWindow.LoginTextBox.Text = ph.Login;
                    UpdateWindow.PasswordTextBox.Text = ph.Password;

                    if (ph.Status == 1)
                    {
                        UpdateWindow.StatusUserRadioButton.IsChecked = true;
                        UpdateWindow.StatusAdministratorRadioButton.IsChecked = false;
                    }
                    if (ph.Status == 2)
                    {
                        UpdateWindow.StatusUserRadioButton.IsChecked = false;
                        UpdateWindow.StatusAdministratorRadioButton.IsChecked = true;
                    }

                    bool? result = UpdateWindow.ShowDialog();
                    if (result == false)
                        return;
                    else 
                    {
                        ph.FullName = UpdateWindow.SurnameTextBox.Text + " " + UpdateWindow.NameTextBox.Text + " " + UpdateWindow.PatronymicTextBox.Text;
                        ph.Login = UpdateWindow.LoginTextBox.Text;
                        ph.Password = UpdateWindow.PasswordTextBox.Text;
                        if (UpdateWindow.StatusUserRadioButton.IsChecked == true)
                        {
                            ph.Status = 1;
                        }
                        if (UpdateWindow.StatusAdministratorRadioButton.IsChecked == true)
                        {
                            ph.Status = 2;
                        }

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

                for (int i = 0; i < allTicket.Count; ++i)
                {
                    if (allTicket[i].UserID == id)
                    {
                        MessageBox.Show("Невозможно удалить эту строку, есть зависимость по внешнему ключу с таблицей Ticket");
                        return;
                    }
                }

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

                    UpdateWindow.IndentificationInformationTextBox.Text = ph.IdentificationInformation;
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
                        return;
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
                if (CreateWindow.HiddenNoRadioButton.IsChecked == true)
                {
                    NewObject.Hidden = false;
                }
                if (CreateWindow.HiddenYesRadioButton.IsChecked == true)
                {
                    NewObject.Hidden = true;
                }

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
                    if (ph.Hidden == false)
                    {
                        UpdateWindow.HiddenNoRadioButton.IsChecked = true;
                        UpdateWindow.HiddenYesRadioButton.IsChecked = false;
                    }
                    else
                    {
                        UpdateWindow.HiddenNoRadioButton.IsChecked = false;
                        UpdateWindow.HiddenYesRadioButton.IsChecked = true;
                    }

                    bool? result = UpdateWindow.ShowDialog();
                    if (result == false)
                        return;
                    else
                    {
                        ph.NumberOfSeats = UpdateWindow.NumberOfSeatsIntegerUpDown.Value;
                        ph.RegistrationNumber = UpdateWindow.RegistrationNumberTextBox.Text;
                        ph.Model = UpdateWindow.ModelTextBox.Text;
                        if (UpdateWindow.HiddenNoRadioButton.IsChecked == true)
                        {
                            ph.Hidden = false;
                        }
                        if (UpdateWindow.HiddenYesRadioButton.IsChecked == true)
                        {
                            ph.Hidden = true;
                        }

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

                for (int i = 0; i < allCruise.Count; ++i)
                {
                    if (allCruise[i].TransportIDOfTheCruise == id)
                    {
                        MessageBox.Show("Невозможно удалить эту строку, есть зависимость по внешнему ключу с таблицей Cruise");
                        return;
                    }
                }

                DBComunication.User.Delete(id);
                allUser = DBComunication.User.GetAll();
                InsertInformationInTransportDataGrid();
            }
        }

        // CRUD Для StopSequences
        private void CreateStopSequencesButton_Click(object sender, RoutedEventArgs e)
        {
            StopSequencesCUWindow CreateWindow = new StopSequencesCUWindow(allLocality, allStoppingOnTheRoute);
            CreateWindow.StoppingIDComboBox.ItemsSource = allStoppingOnTheRoute;
            CreateWindow.StoppingIDComboBox.DisplayMemberPath = "ID";
            CreateWindow.StoppingIDComboBox.SelectedValuePath = "ID"; 
            CreateWindow.StopRouteIDComboBox.ItemsSource = allRoute;
            CreateWindow.StopRouteIDComboBox.DisplayMemberPath = "ID";
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

                int Hours = Int32.Parse(CreateWindow.TravelTimeToStopHoursTextBox.Text);
                int Minutes = Int32.Parse(CreateWindow.TravelTimeToStopMinutesTextBox.Text);
                int Seconds = Int32.Parse(CreateWindow.TravelTimeToStopSecondsTextBox.Text);
                NewObject.TravelTimeToStop = new TimeSpan(Hours, Minutes, Seconds);

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
                    StopSequencesCUWindow UpdateWindow = new StopSequencesCUWindow(allLocality, allStoppingOnTheRoute);
                    UpdateWindow.StoppingIDComboBox.ItemsSource = allStoppingOnTheRoute;
                    UpdateWindow.StoppingIDComboBox.DisplayMemberPath = "ID";
                    UpdateWindow.StoppingIDComboBox.SelectedValuePath = "ID";
                    UpdateWindow.StopRouteIDComboBox.ItemsSource = allRoute;
                    UpdateWindow.StopRouteIDComboBox.DisplayMemberPath = "ID";
                    UpdateWindow.StopRouteIDComboBox.SelectedValuePath = "ID";

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
                        return;
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

                for (int i = 0; i < allStoppingOnTheRoute.Count; ++i)
                {
                    if (allStopSequences[i].StoppingID == id)
                    {
                        MessageBox.Show("Невозможно удалить эту строку, есть зависимость по внешнему ключу с таблицей StopSequences");
                        return;
                    }
                }

                DBComunication.StoppingOnTheRoute.Delete(id);
                allStoppingOnTheRoute = DBComunication.StoppingOnTheRoute.GetAll();
                InsertInformationInStoppingOnTheRouteDataGrid();
            }
        }

        // CRUD Для Route
        private void CreateRouteButton_Click(object sender, RoutedEventArgs e)
        {
            RouteCUWindow CreateWindow = new RouteCUWindow();

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return;
            else
            {
                RouteModel NewObject = new RouteModel();

                NewObject.TravelTimeInHours = CreateWindow.TravelTimeInHoursIntegerUpDown.Value;
                if (CreateWindow.HiddenNoRadioButton.IsChecked == true)
                {
                    NewObject.Hidden = false;
                }
                if (CreateWindow.HiddenYesRadioButton.IsChecked == true)
                {
                    NewObject.Hidden = true;
                }

                DBComunication.Route.Create(NewObject);
                allRoute = DBComunication.Route.GetAll();
                InsertInformationInRouteDataGrid();

                MessageBox.Show("Новый объект добавлен");
            }
        }
        private void UpdateRouteButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForUpdateOperation = RouteDataGrid;

            int index = getSelectedRow(DataGridForUpdateOperation);
            if (index != -1)
            {

                int id = 0;
                RouteModel MarkedRow = (RouteModel)DataGridForUpdateOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                RouteModel ph = allRoute.Where(i => i.ID == id).FirstOrDefault();
                if (ph != null)
                {
                    RouteCUWindow UpdateWindow = new RouteCUWindow();

                    UpdateWindow.TravelTimeInHoursIntegerUpDown.Value = ph.TravelTimeInHours;
                    if (ph.Hidden == false)
                    {
                        UpdateWindow.HiddenNoRadioButton.IsChecked = true;
                        UpdateWindow.HiddenYesRadioButton.IsChecked = false;
                    }
                    else
                    {
                        UpdateWindow.HiddenNoRadioButton.IsChecked = false;
                        UpdateWindow.HiddenYesRadioButton.IsChecked = true;
                    }

                    bool? result = UpdateWindow.ShowDialog();
                    if (result == false)
                        return;
                    else
                    {
                        ph.TravelTimeInHours = UpdateWindow.TravelTimeInHoursIntegerUpDown.Value;
                        if (UpdateWindow.HiddenNoRadioButton.IsChecked == true)
                        {
                            ph.Hidden = false;
                        }
                        if (UpdateWindow.HiddenYesRadioButton.IsChecked == true)
                        {
                            ph.Hidden = true;
                        }

                        DBComunication.Route.Update(ph);
                        allRoute = DBComunication.Route.GetAll();
                        InsertInformationInRouteDataGrid();

                        MessageBox.Show("Объект обновлен");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ни один объект не выбран!");
            }
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

                for (int i = 0; i < allStoppingOnTheRoute.Count; ++i)
                {
                    if (allStopSequences[i].StopRouteID == id)
                    {
                        MessageBox.Show("Невозможно удалить эту строку, есть зависимость по внешнему ключу с таблицей StopSequences");
                        return;
                    }
                }
                for (int i = 0; i < allCruise.Count; ++i)
                {
                    if (allCruise[i].RouteIDOfTheCruise == id)
                    {
                        MessageBox.Show("Невозможно удалить эту строку, есть зависимость по внешнему ключу с таблицей Cruise");
                        return;
                    }
                }

                DBComunication.Route.Delete(id);
                allRoute = DBComunication.Route.GetAll();
                InsertInformationInRouteDataGrid();
            }
        }

        // CRUD Для Locality
        private void CreateLocalityButton_Click(object sender, RoutedEventArgs e)
        {
            LocalityCUWindow CreateWindow = new LocalityCUWindow();

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return;
            else
            {
                LocalityModel NewObject = new LocalityModel();

                NewObject.Region = CreateWindow.RegionTextBox.Text;
                NewObject.Name = CreateWindow.NameTextBox.Text;

                DBComunication.Locality.Create(NewObject);
                allLocality = DBComunication.Locality.GetAll();
                InsertInformationInLocalityDataGrid();

                MessageBox.Show("Новый объект добавлен");
            }
        }
        private void UpdateLocalityButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForUpdateOperation = LocalityDataGrid;

            int index = getSelectedRow(DataGridForUpdateOperation);
            if (index != -1)
            {

                int id = 0;
                LocalityModel MarkedRow = (LocalityModel)DataGridForUpdateOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                LocalityModel ph = allLocality.Where(i => i.ID == id).FirstOrDefault();
                if (ph != null)
                {
                    LocalityCUWindow UpdateWindow = new LocalityCUWindow();

                    UpdateWindow.RegionTextBox.Text = ph.Region;
                    UpdateWindow.NameTextBox.Text = ph.Name;

                    bool? result = UpdateWindow.ShowDialog();
                    if (result == false)
                        return;
                    else
                    {
                        ph.Region = UpdateWindow.RegionTextBox.Text;
                        ph.Name = UpdateWindow.NameTextBox.Text;

                        DBComunication.Locality.Update(ph);
                        allLocality = DBComunication.Locality.GetAll();
                        InsertInformationInLocalityDataGrid();

                        MessageBox.Show("Объект обновлен");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ни один объект не выбран!");
            }
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

                for (int i = 0; i < allStoppingOnTheRoute.Count; ++i)
                {
                    if (allStoppingOnTheRoute[i].StopLocalityID == id)
                    {
                        MessageBox.Show("Невозможно удалить эту строку, есть зависимость по внешнему ключу с таблицей StoppingOnTheRoute");
                        return;
                    }
                }

                DBComunication.Locality.Delete(id);
                allLocality = DBComunication.Locality.GetAll();
                InsertInformationInLocalityDataGrid();
            }
        }

        // CRUD Для Driver
        private void CreateDriverButton_Click(object sender, RoutedEventArgs e)
        {
            DriverCUWindow CreateWindow = new DriverCUWindow();

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return;
            else
            {
                DriverModel NewObject = new DriverModel();

                NewObject.FullName = CreateWindow.SurnameTextBox.Text + " " + CreateWindow.NameTextBox.Text + " " + CreateWindow.PatronymicTextBox.Text;
                NewObject.Experience = Int32.Parse(CreateWindow.ExperienceTextBox.Text);
                NewObject.Salary = Int32.Parse(CreateWindow.SalaryTextBox.Text);
                if (CreateWindow.HiddenNoRadioButton.IsChecked == true)
                {
                    NewObject.Hidden = false;
                }
                if (CreateWindow.HiddenYesRadioButton.IsChecked == true)
                {
                    NewObject.Hidden = true;
                }

                DBComunication.Driver.Create(NewObject);
                allDriver = DBComunication.Driver.GetAll();
                InsertInformationInDriverDataGrid();

                MessageBox.Show("Новый объект добавлен");
            }
        }
        private void UpdateDriverButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForUpdateOperation = DriverDataGrid;

            int index = getSelectedRow(DataGridForUpdateOperation);
            if (index != -1)
            {

                int id = 0;
                DriverModel MarkedRow = (DriverModel)DataGridForUpdateOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                DriverModel ph = allDriver.Where(i => i.ID == id).FirstOrDefault();
                if (ph != null)
                {
                    DriverCUWindow UpdateWindow = new DriverCUWindow();

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

                    UpdateWindow.ExperienceTextBox.Text = ph.Experience.ToString();
                    UpdateWindow.SalaryTextBox.Text = ph.Salary.ToString();

                    if (ph.Hidden == false)
                    {
                        UpdateWindow.HiddenNoRadioButton.IsChecked = true;
                        UpdateWindow.HiddenYesRadioButton.IsChecked = false;
                    }
                    else
                    {
                        UpdateWindow.HiddenNoRadioButton.IsChecked = false;
                        UpdateWindow.HiddenYesRadioButton.IsChecked = true;
                    }

                    bool? result = UpdateWindow.ShowDialog();
                    if (result == false)
                        return;
                    else
                    {
                        ph.FullName = UpdateWindow.SurnameTextBox.Text + " " + UpdateWindow.NameTextBox.Text + " " + UpdateWindow.PatronymicTextBox.Text;
                        ph.Experience = Int32.Parse(UpdateWindow.ExperienceTextBox.Text);
                        ph.Salary = Int32.Parse(UpdateWindow.SalaryTextBox.Text);
                        if (UpdateWindow.HiddenNoRadioButton.IsChecked == true)
                        {
                            ph.Hidden = false;
                        }
                        if (UpdateWindow.HiddenYesRadioButton.IsChecked == true)
                        {
                            ph.Hidden = true;
                        }

                        DBComunication.Driver.Update(ph);
                        allDriver = DBComunication.Driver.GetAll();
                        InsertInformationInDriverDataGrid();

                        MessageBox.Show("Объект обновлен");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ни один объект не выбран!");
            }
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

                for (int i = 0; i < allCruise.Count; ++i)
                {
                    if (allCruise[i].DriverIDOfTheCruise == id)
                    {
                        MessageBox.Show("Невозможно удалить эту строку, есть зависимость по внешнему ключу с таблицей Cruise");
                        return;
                    }
                }

                DBComunication.Driver.Delete(id);
                allDriver = DBComunication.Driver.GetAll();
                InsertInformationInDriverDataGrid();
            }
        }

        // CRUD Для DayOfTheWeek
        // Функционал был удалён, т.к. константные значения в таблицах нельзя изменять
        private void CreateDayOfTheWeekButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateDayOfTheWeekButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteDayOfTheWeekButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // CRUD Для Cruise
        private void CreateCruiseButton_Click(object sender, RoutedEventArgs e)
        {
            CruiseCUWindow CreateWindow = new CruiseCUWindow();
            CreateWindow.DayOfTheWeekIDComboBox.ItemsSource = allDayOfTheWeek;
            CreateWindow.DayOfTheWeekIDComboBox.DisplayMemberPath = "DayOfTheWeekName";
            CreateWindow.DayOfTheWeekIDComboBox.SelectedValuePath = "ID";
            CreateWindow.RouteIDOfTheCruiseComboBox.ItemsSource = allRoute;
            CreateWindow.RouteIDOfTheCruiseComboBox.DisplayMemberPath = "ID";
            CreateWindow.RouteIDOfTheCruiseComboBox.SelectedValuePath = "ID";
            CreateWindow.DriverIDOfTheCruiseComboBox.ItemsSource = allDriver;
            CreateWindow.DriverIDOfTheCruiseComboBox.DisplayMemberPath = "FullName";
            CreateWindow.DriverIDOfTheCruiseComboBox.SelectedValuePath = "ID";
            CreateWindow.TransportIDOfTheCruiseComboBox.ItemsSource = allTransport;
            CreateWindow.TransportIDOfTheCruiseComboBox.DisplayMemberPath = "RegistrationNumber";
            CreateWindow.TransportIDOfTheCruiseComboBox.SelectedValuePath = "ID";

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return;
            else
            {
                CruiseModel NewObject = new CruiseModel();

                NewObject.DayOfTheWeekCruiseID = (int)CreateWindow.DayOfTheWeekIDComboBox.SelectedValue;
                NewObject.RouteIDOfTheCruise = (int)CreateWindow.RouteIDOfTheCruiseComboBox.SelectedValue;
                NewObject.DriverIDOfTheCruise = (int)CreateWindow.DayOfTheWeekIDComboBox.SelectedValue;
                NewObject.TransportIDOfTheCruise = (int)CreateWindow.TransportIDOfTheCruiseComboBox.SelectedValue;

                int Hours = Int32.Parse(CreateWindow.StartTimeHoursTextBox.Text);
                int Minutes = Int32.Parse(CreateWindow.StartTimeMinutesTextBox.Text);
                int Seconds = Int32.Parse(CreateWindow.StartTimeSecondsTextBox.Text);
                NewObject.StartTime = new TimeSpan(Hours, Minutes, Seconds);

                DBComunication.Cruise.Create(NewObject);
                allCruise = DBComunication.Cruise.GetAll();
                InsertInformationInCruiseDataGrid();

                MessageBox.Show("Новый объект добавлен");
            }
        }
        private void UpdateCruiseButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid DataGridForUpdateOperation = CruiseDataGrid;

            int index = getSelectedRow(DataGridForUpdateOperation);
            if (index != -1)
            {

                int id = 0;
                CruiseModel MarkedRow = (CruiseModel)DataGridForUpdateOperation.Items[index];
                bool converted = Int32.TryParse(MarkedRow.ID.ToString(), out id);
                if (converted == false)
                    return;

                CruiseModel ph = allCruise.Where(i => i.ID == id).FirstOrDefault();
                if (ph != null)
                {
                    CruiseCUWindow UpdateWindow = new CruiseCUWindow();
                    UpdateWindow.DayOfTheWeekIDComboBox.ItemsSource = allDayOfTheWeek;
                    UpdateWindow.DayOfTheWeekIDComboBox.DisplayMemberPath = "DayOfTheWeekName";
                    UpdateWindow.DayOfTheWeekIDComboBox.SelectedValuePath = "ID";
                    UpdateWindow.RouteIDOfTheCruiseComboBox.ItemsSource = allRoute;
                    UpdateWindow.RouteIDOfTheCruiseComboBox.DisplayMemberPath = "ID";
                    UpdateWindow.RouteIDOfTheCruiseComboBox.SelectedValuePath = "ID";
                    UpdateWindow.DriverIDOfTheCruiseComboBox.ItemsSource = allDriver;
                    UpdateWindow.DriverIDOfTheCruiseComboBox.DisplayMemberPath = "FullName";
                    UpdateWindow.DriverIDOfTheCruiseComboBox.SelectedValuePath = "ID";
                    UpdateWindow.TransportIDOfTheCruiseComboBox.ItemsSource = allTransport;
                    UpdateWindow.TransportIDOfTheCruiseComboBox.DisplayMemberPath = "RegistrationNumber";
                    UpdateWindow.TransportIDOfTheCruiseComboBox.SelectedValuePath = "ID";

                    UpdateWindow.DayOfTheWeekIDComboBox.SelectedValue = ph.DayOfTheWeekCruiseID;
                    UpdateWindow.RouteIDOfTheCruiseComboBox.SelectedValue = ph.RouteIDOfTheCruise;
                    UpdateWindow.DriverIDOfTheCruiseComboBox.SelectedValue = ph.DriverIDOfTheCruise;
                    UpdateWindow.TransportIDOfTheCruiseComboBox.SelectedValue = ph.TransportIDOfTheCruise;

                    int Hours = ph.StartTime.Value.Hours;
                    int Minutes = ph.StartTime.Value.Minutes;
                    int Seconds = ph.StartTime.Value.Seconds;
                    UpdateWindow.StartTimeHoursTextBox.Text = Hours.ToString();
                    UpdateWindow.StartTimeMinutesTextBox.Text = Minutes.ToString();
                    UpdateWindow.StartTimeSecondsTextBox.Text = Seconds.ToString();

                    bool? result = UpdateWindow.ShowDialog();
                    if (result == false)
                        return;
                    else
                    {
                        ph.DayOfTheWeekCruiseID = (int)UpdateWindow.DayOfTheWeekIDComboBox.SelectedValue;
                        ph.RouteIDOfTheCruise = (int)UpdateWindow.RouteIDOfTheCruiseComboBox.SelectedValue;
                        ph.DriverIDOfTheCruise = (int)UpdateWindow.DriverIDOfTheCruiseComboBox.SelectedValue;
                        ph.TransportIDOfTheCruise = (int)UpdateWindow.TransportIDOfTheCruiseComboBox.SelectedValue;

                        Hours = Int32.Parse(UpdateWindow.StartTimeHoursTextBox.Text);
                        Minutes = Int32.Parse(UpdateWindow.StartTimeMinutesTextBox.Text);
                        Seconds = Int32.Parse(UpdateWindow.StartTimeSecondsTextBox.Text);
                        ph.StartTime = new TimeSpan(Hours, Minutes, Seconds);

                        DBComunication.Cruise.Update(ph);
                        allCruise = DBComunication.Cruise.GetAll();
                        InsertInformationInCruiseDataGrid();

                        MessageBox.Show("Объект обновлен");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ни один объект не выбран!");
            }
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

                for (int i = 0; i < allTicket.Count; ++i)
                {
                    if (allTicket[i].CruiseID == id)
                    {
                        MessageBox.Show("Невозможно удалить эту строку, есть зависимость по внешнему ключу с таблицей Ticket");
                        return;
                    }
                }

                DBComunication.Cruise.Delete(id);
                allCruise = DBComunication.Cruise.GetAll();
                InsertInformationInCruiseDataGrid();
            }
        }

        private void Report1Button_Click(object sender, RoutedEventArgs e)
        {
            Report1DataGrid.ItemsSource = FindeRoute.StoredProcedureExecute2((int)Report1LastStoppingComboBox.SelectedValue);
        }

        private void Report2Button_Click(object sender, RoutedEventArgs e)
        {
            Report2DataGrid.ItemsSource = FindeCruise.StoredProcedureExecute((int)Report2AmountOfHoursIntegerUpDown.Value);
        }

        private void Report3Button_Click(object sender, RoutedEventArgs e)
        {
            Report3DataGrid.ItemsSource = FindeRoute.StoredProcedureExecute1((int)Report3StartingLocationComboBox.SelectedValue, (int)Report3LastLocationComboBox.SelectedValue);
        }

        #endregion

        #region --- Функции для постройки и отображения графиков

        private List<FindeDriver.StoredProcedureResult> DriversForChartDependenceOfSalaryOnLengthOfService = FindeDriver.StoredProcedureExecute();

        private void UpdateChartDependenceOfSalaryOnLengthOfServiceTabItemButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllChartsInfo();
        }
        private void UpdateChartDriversSalaryTabItemButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllChartsInfo();
        }
        private void UpdateChartUsersStatusesCountTabItemButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllChartsInfo();
        }
        private void UpdateChartAmountOfCreatedCruisesOnTheRouteTabItemButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllChartsInfo();
        }
        private void UpdateChartAmountOfStoppingOnTheRouteTabItemButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllChartsInfo();
        }

        private void UpdateAllChartsInfo()
        {
            DriversForChartDependenceOfSalaryOnLengthOfService = FindeDriver.StoredProcedureExecute();
            InsertInformationInChartsTab();
            BuildCharts();
        }


        private void InsertInformationInChartsTab()
        {
            InsertDriversInformationForChartsDataGrid();
            InsertUsersInformationForChartsDataGrid();
        }
        private void InsertDriversInformationForChartsDataGrid()
        {
            DriverForChartDependenceOfSalaryOnLengthOfServiceDataGrid.ItemsSource = DriversForChartDependenceOfSalaryOnLengthOfService;
            ChartDriversSalaryDataGrid.ItemsSource = DriversForChartDependenceOfSalaryOnLengthOfService;
        }
        private void InsertUsersInformationForChartsDataGrid()
        {
            UsersStatusesCountDataGrid.ItemsSource = allUser;
        }

        private void BuildCharts()
        {
            BuildChartDependenceOfSalaryOnLengthOfService();
            BuildChartDriversSalary();
            BuildChartUsersStatusCount();
            BuildChartAmountOfCreatedCruisesOnTheRoute();
            BuildChartAmountOfStoppingOnTheRoute();
        }

        private void UpdateDriverChartInfo()
        {
            DriversForChartDependenceOfSalaryOnLengthOfService = FindeDriver.StoredProcedureExecute();
            InsertDriversInformationForChartsDataGrid();
        }

        private void BuildChartDependenceOfSalaryOnLengthOfService()
        {
            List<FindeDriver.StoredProcedureResult> DriversForChart = new List<FindeDriver.StoredProcedureResult>();
            ChartValues<int> Salarys = new ChartValues<int>();
            string[] Labels = new string[DriversForChartDependenceOfSalaryOnLengthOfService.Count];

            int countOfDrivers = DriversForChartDependenceOfSalaryOnLengthOfService.Count;
            for (int i = 0; i < countOfDrivers; i++)
            {
                int minExperience = int.MaxValue;
                int index = 0;
                for (int j = 0; j < DriversForChartDependenceOfSalaryOnLengthOfService.Count; j++)
                {
                    if (DriversForChartDependenceOfSalaryOnLengthOfService[j].Experience < minExperience)
                    {
                        minExperience = DriversForChartDependenceOfSalaryOnLengthOfService[j].Experience;
                        index = j;
                    }
                }
                DriversForChart.Add(DriversForChartDependenceOfSalaryOnLengthOfService[index]);
                DriversForChartDependenceOfSalaryOnLengthOfService.RemoveAt(index);
            }

            UpdateDriverChartInfo();

            for (int i = 0; i < DriversForChart.Count; ++i)
            {
                Salarys.Add(DriversForChart[i].Salary);
            }
            for (int i = 0; i < DriversForChart.Count; ++i)
            {
                Labels[i] = DriversForChart[i].Experience.ToString()+ "Года/Лет Стажа " + DriversForChart[i].FullName;
            }

            SeriesCollection SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Зависимость зарплаты от стажа работы",
                    Values = Salarys
                }
            };

            Func<double, string> YFormatter = value => value.ToString("C");

            ChartDependenceOfSalaryOnLengthOfService.Series = SeriesCollection;
            ChartDependenceOfSalaryOnLengthOfServiceXAx.LabelFormatter = YFormatter;
            ChartDependenceOfSalaryOnLengthOfServiceYAx.Labels = Labels;
        }

        private void BuildChartDriversSalary()
        {
            List<FindeDriver.StoredProcedureResult> DriversForChart = DriversForChartDependenceOfSalaryOnLengthOfService;
            ChartValues<int> Salarys = new ChartValues<int>();
            string[] Labels = new string[DriversForChartDependenceOfSalaryOnLengthOfService.Count];

            UpdateDriverChartInfo();

            for (int i = 0; i < DriversForChart.Count; ++i)
            {
                Salarys.Add(DriversForChart[i].Salary);
            }
            for (int i = 0; i < DriversForChart.Count; ++i)
            {
                Labels[i] = DriversForChart[i].FullName;
            }

            SeriesCollection SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Зарплата водителей",
                    Values = Salarys
                }
            };

            Func<double, string> Formatter = value => value.ToString("C");

            ChartDriversSalary.Series = SeriesCollection;
            ChartDriversSalaryeXAx.LabelFormatter = Formatter;
            ChartDriversSalaryYAx.Labels = Labels;
        }

        private void BuildChartUsersStatusCount()
        {
            List<FindeUsersStatusesCount.StoredProcedureResult> ResultOfSP = FindeUsersStatusesCount.StoredProcedureExecute();
            ChartValues<int> CountOfStatuses = new ChartValues<int>();
            string[] Labels = { "Обычные пользователи", "Администраторы" };

            for (int i = 0; i < ResultOfSP.Count; ++i)
            {
                CountOfStatuses.Add(ResultOfSP[i].Status);
            }

            SeriesCollection SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Кол-во пользователей",
                    Values = CountOfStatuses
                }
            };

            Func<double, string> Formatter = value => value.ToString("N");

            ChartUsersStatusesCount.Series = SeriesCollection;
            ChartUsersStatusesCountXAx.LabelFormatter = Formatter;
            ChartUsersStatusesCountYAx.Labels = Labels;
        }
            
        private void BuildChartAmountOfCreatedCruisesOnTheRoute()
        {
            List<FindeAmountOfCruiseOnTheRoute.StoredProcedureResult> ResultOfSP = FindeAmountOfCruiseOnTheRoute.StoredProcedureExecute();
            ChartValues<int> CountOfCruises = new ChartValues<int>();
            string[] Labels = new string[ResultOfSP.Count];

            for (int i = 0; i < ResultOfSP.Count; ++i)
            {
                CountOfCruises.Add(ResultOfSP[i].AmountOfCruises);
            }
            for (int i = 0; i < ResultOfSP.Count; ++i)
            {
                Labels[i] = "ID Маршрута = " + ResultOfSP[i].IDOfRoute.ToString();
            }

            SeriesCollection SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Кол-во рейсов",
                    Values = CountOfCruises
                }
            };

            Func<double, string> Formatter = value => value.ToString("N");

            ChartAmountOfCreatedCruisesOnTheRoute.Series = SeriesCollection;
            ChartAmountOfCreatedCruisesOnTheRouteXAx.LabelFormatter = Formatter;
            ChartAmountOfCreatedCruisesOnTheRouteYAx.Labels = Labels;
        }

        private void BuildChartAmountOfStoppingOnTheRoute()
        {
            List<FindeAmountOfStoppingOnTheRoute.StoredProcedureResult> ResultOfSP = FindeAmountOfStoppingOnTheRoute.StoredProcedureExecute();
            ChartValues<int> CountOfCruises = new ChartValues<int>();
            string[] Labels = new string[ResultOfSP.Count];

            for (int i = 0; i < ResultOfSP.Count; ++i)
            {
                CountOfCruises.Add(ResultOfSP[i].AmountOfStoppings);
            }
            for (int i = 0; i < ResultOfSP.Count; ++i)
            {
                Labels[i] = "ID Маршрута = " + ResultOfSP[i].RouteID.ToString();
            }

            SeriesCollection SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Кол-во остановок",
                    Values = CountOfCruises
                }
            };

            Func<double, string> Formatter = value => value.ToString("N");

            ChartAmountOfStoppingOnTheRoute.Series = SeriesCollection;
            ChartAmountOfStoppingOnTheRouteXAx.LabelFormatter = Formatter;
            ChartAmountOfStoppingOnTheRouteYAx.Labels = Labels;
        }

        #endregion
    }
}