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

namespace Valieva_Language
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Client _currentClient = new Client();

        public AddEditPage(Client SelectedClient)
        {
            InitializeComponent();
            if (SelectedClient != null)
                _currentClient = SelectedClient;

            DataContext = _currentClient;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (_currentClient.ID == 0)
                errors.AppendLine("Укажите ID");
            if (string.IsNullOrWhiteSpace(_currentClient.LastName))
                errors.AppendLine("Укажите фамилию");
            if (string.IsNullOrWhiteSpace(_currentClient.FirstName))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(_currentClient.Patronymic))
                errors.AppendLine("Укажите отчество");
            if (string.IsNullOrWhiteSpace(_currentClient.Email))
                errors.AppendLine("Укажите email");
            if (string.IsNullOrWhiteSpace(_currentClient.Phone))
                errors.AppendLine("Укажите номер телефона");
            if (string.IsNullOrWhiteSpace(_currentClient.BirthdayString))
                errors.AppendLine("Укажите дату рождения");
            //////...
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentClient.ID == 0)
                ValievaLanguageEntities.GetContext().Client.Add(_currentClient);

            try
            {
                ValievaLanguageEntities.GetContext().SaveChanges();
                MessageBox.Show("");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }

        private void BtnEditPhoto_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
