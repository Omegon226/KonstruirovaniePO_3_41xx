using BLL.Models;
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

namespace Интерфейс
{
    /// <summary>
    /// Логика взаимодействия для OrderTicketWindow.xaml
    /// </summary>
    public partial class OrderTicketWindow : Window
    {
        public OrderTicketWindow()
        {
            InitializeComponent();
            SetIndentificationInfoAsPasport();
        }
        public OrderTicketWindow(UserModel UserInfo)
        {
            InitializeComponent();
            SetIndentificationInfoAsPasport();

            string[] FullName = UserInfo.FullName.Split(' ');

            if (FullName.Length == 1)
            {
                this.SurnameTextBox.Text = FullName[0];
            }
            if (FullName.Length == 2)
            {
                this.SurnameTextBox.Text = FullName[0];
                this.NameTextBox.Text = FullName[1];
            }
            if (FullName.Length == 3)
            {
                this.SurnameTextBox.Text = FullName[0];
                this.NameTextBox.Text = FullName[1];
                this.PatronymicTextBox.Text = FullName[2];
            }
        }
        private void SetIndentificationInfoAsPasport()
        {
            DocumentTypeComboBox.SelectedIndex = 0;
            IndentificationInformationTextBox.Mask = "0000 000000";
        }

        private void DocumentTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DocumentTypeComboBox.SelectedIndex == 1)
            {
                IndentificationInformationTextBox.Text = "";
                IndentificationInformationTextBox.Mask = "LLL-LL № 000000";
            }
            if (DocumentTypeComboBox.SelectedIndex == 0)
            {
                IndentificationInformationTextBox.Text = "";
                IndentificationInformationTextBox.Mask = "0000 000000";
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            bool SuccessOfSurnameParse = SurnameTextBox.Text.All(Char.IsLetter) && SurnameTextBox.Text != "";
            bool SuccessOfNameParse = NameTextBox.Text.All(Char.IsLetter) && NameTextBox.Text != "";
            bool SuccessOfPatronomicParse = PatronymicTextBox.Text.All(Char.IsLetter) && PatronymicTextBox.Text != "";

            bool SuccessOfFullNameInfo = SuccessOfSurnameParse && SuccessOfNameParse && SuccessOfPatronomicParse;

            if (SuccessOfFullNameInfo)
            {
                bool CorectnessOfIndetificationInformatio = ValidationOfIndetificationInformatio(IndentificationInformationTextBox.Text, DocumentTypeComboBox.SelectedIndex);

                if (CorectnessOfIndetificationInformatio)
                {
                    DialogResult = true;
                    return;
                }
                else
                {
                    MessageBox.Show("Значения Индентификационная информации, введены неверно! Измените её");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Видимо какое то поле имеет неправильный формат ввода");
                return;
            }
        }

        private bool ValidationOfIndetificationInformatio(string IndentificationInfo, int TypeOfDocument)
        {
            if (TypeOfDocument == 0)
            {
                // Проверка по паспорту
                if (IndentificationInfo.Length == 11)
                {
                    return (true);
                }
            }
            if (TypeOfDocument == 1)
            {
                // Проверка по сведетельству о рождении
                if ((13 <= IndentificationInfo.Length) && (IndentificationInfo.Length <= 15))
                {
                    return (true);
                }
            }
            return (false);
        }

    }
}
