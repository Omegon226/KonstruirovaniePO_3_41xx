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

namespace Интерфейс.CeateUpdateWindows
{
    /// <summary>
    /// Логика взаимодействия для UserCUWindow.xaml
    /// </summary>
    public partial class UserCUWindow : Window
    {
        public UserCUWindow()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            bool SuccessOfSurnameParse = SurnameTextBox.Text.All(Char.IsLetter) && SurnameTextBox.Text != "";
            bool SuccessOfNameParse = NameTextBox.Text.All(Char.IsLetter) && NameTextBox.Text != "";
            bool SuccessOfPatronomicParse = PatronymicTextBox.Text.All(Char.IsLetter) && PatronymicTextBox.Text != "";
            bool SuccessOfLoginParse = LoginTextBox.Text.All(Char.IsLetterOrDigit);
            bool SuccessOfPasswordParse = PasswordTextBox.Text.All(Char.IsLetterOrDigit);

            if (SuccessOfSurnameParse && SuccessOfNameParse && SuccessOfPatronomicParse && SuccessOfLoginParse && SuccessOfPasswordParse)
            {
                bool CorectnessOfLogin = ValidationOfLogin();
                bool CorectnessOfPassword = ValidationOfPassword();

                if (CorectnessOfLogin && CorectnessOfPassword)
                {
                    DialogResult = true;
                    return;
                }
                else
                {
                    MessageBox.Show("Значения логина или пароля неверны! Измените их");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Видимо какое то поле имеет неправильный формат ввода");
                return;
            }
        }
        private bool ValidationOfLogin()
        {
            if (LoginTextBox.Text.Contains(' '))
            {
                return (false);
            }
            return (true);
        }
        private bool ValidationOfPassword()
        {
            if (PasswordTextBox.Text.Contains(' '))
            {
                return (false);
            }
            return (true);
        }
    }
}
