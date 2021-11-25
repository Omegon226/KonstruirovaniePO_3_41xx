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
using Интерфейс.CRUDClasses;
using Интерфейс.CharsBuildersClasses;

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

        private CRUDOperations CRUD;
        private ChartsBuilder Charts;

        private int StatusLevelOfUser = 0;
        private UserModel AuthorisedUser;

        public MainWindow()
        {
            CRUD = new CRUDOperations(DBComunication);
            Charts = new ChartsBuilder();

            InitializeComponent();
            LoadAllInformationFromDataBase();
            InsertInformationInCRUDDataGrids();
            InsertInformationInReportsComboBoxes();
            InsertInformationInChartsTab();
            BuildCharts();
            InsertInformationInFindeRouteComboBoxes();

            CheckUserPrivilegesAndChangeElementVisibility();
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

        private void CheckUserPrivilegesAndChangeElementVisibility()
        {
            bool IfUserOnAdminGrids = (CRUDGrid.Visibility == Visibility.Visible) || 
                                        (CreateReportsGrid.Visibility == Visibility.Visible) || 
                                        (CreateChartsGrid.Visibility == Visibility.Visible);

            if (StatusLevelOfUser == 0)
            {
                SetElemetsVisibilityAsForUser();

                if (IfUserOnAdminGrids)
                {
                    ReturnToFindeRouteGrid();
                }

                return;
            }
            if (StatusLevelOfUser == 1)
            {
                SetElemetsVisibilityAsForUser();

                if (IfUserOnAdminGrids)
                {
                    ReturnToFindeRouteGrid();
                }

                return;
            }
            if (StatusLevelOfUser == 2)
            {
                SetElemetsVisibilityAsForAdministrator();
                return;
            }
        }
        private void SetElemetsVisibilityAsForUser()
        {
            FindeRouteTabOpenButton.Visibility = Visibility.Visible;
            CRUDTabOpenButton.Visibility = Visibility.Hidden;
            CreateReportsTabOpenButton.Visibility = Visibility.Hidden;
            CreateChartsTabOpenButton.Visibility = Visibility.Hidden;
        }
        private void SetElemetsVisibilityAsForAdministrator()
        {
            FindeRouteTabOpenButton.Visibility = Visibility.Visible;
            CRUDTabOpenButton.Visibility = Visibility.Visible;
            CreateReportsTabOpenButton.Visibility = Visibility.Visible;
            CreateChartsTabOpenButton.Visibility = Visibility.Visible;
        }
        private void ReturnToFindeRouteGrid()
        {
            FindeRouteGrid.Visibility = Visibility.Visible;
            CreateReportsGrid.Visibility = Visibility.Hidden;
            CRUDGrid.Visibility = Visibility.Hidden;
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
                CheckUserPrivilegesAndChangeElementVisibility();
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
                    SetAuthorizeExitAndRegistrationButtonVisibiliy();

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

        private void RegistrationButtonButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterUserWindow RegistrationWindow = new RegisterUserWindow();

            bool? result = RegistrationWindow.ShowDialog();
            if (result == false)
                return;
            else
            {
                UserModel AuthorizedUser = new UserModel();

                AuthorizedUser.FullName = RegistrationWindow.SurnameTextBox.Text + " " + RegistrationWindow.NameTextBox.Text + " " + RegistrationWindow.PatronymicTextBox.Text;
                AuthorizedUser.Login = RegistrationWindow.LoginTextBox.Text;
                AuthorizedUser.Password = RegistrationWindow.PasswordTextBox.Text;
                AuthorizedUser.Status = 1;

                FindeSameUserAndRegistrait(AuthorizedUser);
                CheckUserPrivilegesAndChangeElementVisibility();
            }
        }
        private void FindeSameUserAndRegistrait(UserModel UserToFinde)
        {
            for (int i = 0; i < allUser.Count; ++i)
            {
                if ((UserToFinde.Login == allUser[i].Login) && (UserToFinde.FullName == allUser[i].FullName))
                {
                    MessageBox.Show("Такой пользователь уже существует");
                    return;
                }
            }

            StatusLevelOfUser = (int)UserToFinde.Status;
            AuthorisedUser = UserToFinde;
            SetAuthorizeExitAndRegistrationButtonVisibiliy();

            DBComunication.User.Create(UserToFinde);
            allUser = DBComunication.User.GetAll();
            InsertInformationInUserDataGrid();
        }

        private void DeauthorisetionButton_Click(object sender, RoutedEventArgs e)
        {
            StatusLevelOfUser = 0;
            AuthorisedUser = null;
            SetAuthorizeExitAndRegistrationButtonVisibiliy();
            MessageBox.Show("Вы вышли из системы");
            CheckUserPrivilegesAndChangeElementVisibility();
        }

        private void SetAuthorizeExitAndRegistrationButtonVisibiliy()
        {
            if (AuthorisedUser == null)
            {
                AuthorisetionButton.Visibility = Visibility.Visible;
                RegistrationButton.Visibility = Visibility.Visible;
                DeauthorisetionButton.Visibility = Visibility.Hidden;
            }
            else
            {
                AuthorisetionButton.Visibility = Visibility.Hidden;
                RegistrationButton.Visibility = Visibility.Hidden;
                DeauthorisetionButton.Visibility = Visibility.Visible;
            }
        }

        private void FindeRouteTabOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FindeRouteGrid.Visibility = Visibility.Visible;
            CRUDGrid.Visibility = Visibility.Hidden;
            CreateReportsGrid.Visibility = Visibility.Hidden;
            CreateChartsGrid.Visibility = Visibility.Hidden;
        }

        private void CRUDTabOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FindeRouteGrid.Visibility = Visibility.Hidden;
            CRUDGrid.Visibility = Visibility.Visible;
            CreateReportsGrid.Visibility = Visibility.Hidden;
            CreateChartsGrid.Visibility = Visibility.Hidden;
        }

        private void CreateReportsTabOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FindeRouteGrid.Visibility = Visibility.Hidden;
            CRUDGrid.Visibility = Visibility.Hidden;
            CreateReportsGrid.Visibility = Visibility.Visible;
            CreateChartsGrid.Visibility = Visibility.Hidden;
        }

        private void CreateChartsTabOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FindeRouteGrid.Visibility = Visibility.Hidden;
            CRUDGrid.Visibility = Visibility.Hidden;
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
            if (DateOfRoteToFindeDatePicker.SelectedDate - DateTime.Now > new TimeSpan(30, 0, 0, 0))
            {
                MessageBox.Show("Вы не можете заказывать билеты больше чем за 30 дней");
                return;
            }

            SelectCruiseWindow WindowToFindeCruise;

            if (DateOfRoteToFindeDatePicker.SelectedDate == null)
            {
                WindowToFindeCruise = new SelectCruiseWindow(this, DBComunication, StartPoint, EndPoint, StatusLevelOfUser, AuthorisedUser);
            }
            else
            {
                WindowToFindeCruise = new SelectCruiseWindow(this, DBComunication, StartPoint, EndPoint, StatusLevelOfUser, AuthorisedUser, DateOfRoteToFindeDatePicker.SelectedDate);
            }    

            bool? result = WindowToFindeCruise.ShowDialog();
            if (result == false)
                return;

            LoadAllInformationFromDataBase();
            InsertInformationInCRUDDataGrids();
            InsertInformationInReportsComboBoxes();
            InsertInformationInChartsTab();
            BuildCharts();
        }

        public void ChangeStatusOfUser(int StatusLevle)
        {
            StatusLevelOfUser = StatusLevle;
            CheckUserPrivilegesAndChangeElementVisibility();
        }
        public void ChangeRegistraitedUserInfo(UserModel User)
        {
            AuthorisedUser = User;
            AuthorisetionButton.Visibility = Visibility.Hidden;
            RegistrationButton.Visibility = Visibility.Hidden;
            DeauthorisetionButton.Visibility = Visibility.Visible;
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

        #region --- Функции для CRUD

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
            UserModel NewObject = CRUD.User.Create();

            if (NewObject != null)
            {
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
                    ph = CRUD.User.Update(ph);

                    if (ph != null)
                    {
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
            TicketModel NewObject = CRUD.Ticket.Create();

            if (NewObject != null)
            {
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
                    ph = CRUD.Ticket.Update(ph);

                    if (ph != null)
                    {
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
            TransportModel NewObject = CRUD.Transport.Create();

            if (NewObject != null)
            {
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
                    ph = CRUD.Transport.Update(ph);

                    if (ph != null)
                    {
                        DBComunication.Transport.Update(ph);
                        allTransport = DBComunication.Transport.GetAll();
                        InsertInformationInTransportDataGrid();

                        MessageBox.Show("Объект обновлен");
                    }
                }
                else
                {
                    MessageBox.Show("Ни один объект не выбран!");
                }

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
            StopSequencesModel NewObject = CRUD.StopSequences.Create();

            if (NewObject != null)
            {
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
                    ph = CRUD.StopSequences.Update(ph);

                    if (ph != null)
                    {
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
            StoppingOnTheRouteModel NewObject = CRUD.StoppingOnTheRoute.Create();

            if (NewObject != null)
            {
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
                    ph = CRUD.StoppingOnTheRoute.Update(ph);

                    if (ph != null)
                    {
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
            RouteModel NewObject = CRUD.Route.Create();

            if (NewObject != null)
            {
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
                    ph = CRUD.Route.Update(ph);

                    if (ph != null)
                    {
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
            LocalityModel NewObject = CRUD.Locality.Create();

            if (NewObject != null)
            {
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
                    ph = CRUD.Locality.Update(ph);

                    if (ph != null)
                    {
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
            DriverModel NewObject = CRUD.Driver.Create();

            if (NewObject != null)
            {
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
                    ph = CRUD.Driver.Update(ph);

                    if (ph != null)
                    {
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
            CruiseModel NewObject = CRUD.Cruise.Create();

            if (NewObject != null)
            {
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
                    ph = CRUD.Cruise.Update(ph);

                    if (ph != null)
                    {
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
        private void MakeSetupForCruiseCUWindow(CruiseCUWindow Window)
        {
            Window.DayOfTheWeekIDComboBox.ItemsSource = allDayOfTheWeek;
            Window.DayOfTheWeekIDComboBox.DisplayMemberPath = "DayOfTheWeekName";
            Window.DayOfTheWeekIDComboBox.SelectedValuePath = "ID";
            Window.RouteIDOfTheCruiseComboBox.ItemsSource = allRoute;
            Window.RouteIDOfTheCruiseComboBox.DisplayMemberPath = "ID";
            Window.RouteIDOfTheCruiseComboBox.SelectedValuePath = "ID";
            Window.DriverIDOfTheCruiseComboBox.ItemsSource = allDriver;
            Window.DriverIDOfTheCruiseComboBox.DisplayMemberPath = "FullName";
            Window.DriverIDOfTheCruiseComboBox.SelectedValuePath = "ID";
            Window.TransportIDOfTheCruiseComboBox.ItemsSource = allTransport;
            Window.TransportIDOfTheCruiseComboBox.DisplayMemberPath = "RegistrationNumber";
            Window.TransportIDOfTheCruiseComboBox.SelectedValuePath = "ID";
        }

        #endregion

        #region --- Функции для создания отчётов

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
            UpdateDriverChartInfo();
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
            Charts.DependenceOfSalaryOnLengthOfService.UpdateDriversForChartDependenceOfSalaryOnLengthOfService();
            DriversForChartDependenceOfSalaryOnLengthOfService = FindeDriver.StoredProcedureExecute();
            InsertDriversInformationForChartsDataGrid();
        }

        private void BuildChartDependenceOfSalaryOnLengthOfService()
        {
            Charts.DependenceOfSalaryOnLengthOfService.SetInformationForChart();

            ChartDependenceOfSalaryOnLengthOfService.Series = Charts.DependenceOfSalaryOnLengthOfService.SeriesCollection;
            ChartDependenceOfSalaryOnLengthOfServiceXAx.LabelFormatter = Charts.DependenceOfSalaryOnLengthOfService.YFormatter;
            ChartDependenceOfSalaryOnLengthOfServiceYAx.Labels = Charts.DependenceOfSalaryOnLengthOfService.Labels;
        }

        private void BuildChartDriversSalary()
        {
            Charts.DriversSalary.SetInformationForChart();

            ChartDriversSalary.Series = Charts.DriversSalary.SeriesCollection;
            ChartDriversSalaryeXAx.LabelFormatter = Charts.DriversSalary.YFormatter;
            ChartDriversSalaryYAx.Labels = Charts.DriversSalary.Labels;
        }

        private void BuildChartUsersStatusCount()
        {
            Charts.UsersStatusCount.SetInformationForChart();

            ChartUsersStatusesCount.Series = Charts.UsersStatusCount.SeriesCollection;
            ChartUsersStatusesCountXAx.LabelFormatter = Charts.UsersStatusCount.Formatter;
            ChartUsersStatusesCountYAx.Labels = Charts.UsersStatusCount.Labels;
        }
            
        private void BuildChartAmountOfCreatedCruisesOnTheRoute()
        {
            Charts.AmountOfCreatedCruisesOnTheRoute.SetInformationForChart();

            ChartAmountOfCreatedCruisesOnTheRoute.Series = Charts.AmountOfCreatedCruisesOnTheRoute.SeriesCollection;
            ChartAmountOfCreatedCruisesOnTheRouteXAx.LabelFormatter = Charts.AmountOfCreatedCruisesOnTheRoute.Formatter;
            ChartAmountOfCreatedCruisesOnTheRouteYAx.Labels = Charts.AmountOfCreatedCruisesOnTheRoute.Labels;
        }

        private void BuildChartAmountOfStoppingOnTheRoute()
        {
            Charts.AmountOfStoppingOnTheRoute.SetInformationForChart();

            ChartAmountOfStoppingOnTheRoute.Series = Charts.AmountOfStoppingOnTheRoute.SeriesCollection;
            ChartAmountOfStoppingOnTheRouteXAx.LabelFormatter = Charts.AmountOfStoppingOnTheRoute.Formatter;
            ChartAmountOfStoppingOnTheRouteYAx.Labels = Charts.AmountOfStoppingOnTheRoute.Labels;
        }

        #endregion

    }
}