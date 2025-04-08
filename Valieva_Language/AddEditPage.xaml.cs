using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            {
                _currentClient = SelectedClient;

                if (_currentClient.GenderCode == 1)
                    RBtnMal.IsChecked = true;
                else
                    RBtnFem.IsChecked = true;
                _currentClient.Birthday = Convert.ToDateTime(_currentClient.Birthday);
            } else
            {
                BirthdayDate.Text = "";
                TBIDName.Visibility = Visibility.Hidden;
                TBID.Visibility = Visibility.Hidden;
                //Photo.Source = new BitmapImage(new Uri("Клиенты/picture.png"));
               // TBID.Text = (ValievaLanguageEntities.GetContext().Client.OrderByDescending(x => x.ID).First().ID + 1).ToString();
            }

            DataContext = _currentClient;

            //RBtnFem.IsChecked = cli == "ж";
            //RBtnMal.IsChecked = cli == "м";


        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            /* if (_currentClient.ID == 0)
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

             */
            // Валидация ФИО
            if (string.IsNullOrWhiteSpace(_currentClient.LastName) || !_currentClient.LastName.All(c => char.IsLetter(c) || c == ' ' || c == '-') || _currentClient.LastName.Length > 50)
                errors.AppendLine("Фамилия может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов.");
            if (string.IsNullOrWhiteSpace(_currentClient.FirstName) || !_currentClient.FirstName.All(c => char.IsLetter(c) || c == ' ' || c == '-') || _currentClient.FirstName.Length > 50)
                errors.AppendLine("Имя может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов.");
            if (string.IsNullOrWhiteSpace(_currentClient.Patronymic) || !_currentClient.Patronymic.All(c => char.IsLetter(c) || c == ' ' || c == '-') || _currentClient.Patronymic.Length > 50)
                errors.AppendLine("Отчество может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов.");

            // Валидация Email
            if (string.IsNullOrWhiteSpace(_currentClient.Email))
                errors.AppendLine("Email не может быть пустым.");
            else
            {
                try
                {
                    string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}$";
                    if (!Regex.IsMatch(_currentClient.Email, emailPattern))
                        errors.AppendLine("Некорректный Email!");
                    //var addr = new System.Net.Mail.MailAddress(_currentClient.Email);
                    //if (addr.Address != _currentClient.Email)
                    //    throw new Exception();
                }
                catch
                {
                    errors.AppendLine("Укажите правильный email агента.");
                }
            }

            // Валидация телефона
            if (string.IsNullOrWhiteSpace(_currentClient.Phone))
                errors.AppendLine("Телефон не может быть пустым.");
            else
            {
                string phonePattern = @"^\+?\d[\d\-\(\)\s]+$";
                if (!Regex.IsMatch(_currentClient.Phone, phonePattern))
                    errors.AppendLine("Телефон может содержать только цифры, плюс, минус, скобки и пробелы.");
            }
            if (BirthdayDate.Text == "")
            {
                errors.AppendLine("Укажите дату рождения");
            }





            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClient.GenderCode = RBtnMal.IsChecked == true ? 1 : 2;

            if (_currentClient.ID == 0)
            {
                //_currentClient.ID = Convert.ToInt32(TBID.Text);
                _currentClient.LastName = TBLastName.Text;
                _currentClient.FirstName = TBLastName.Text;
                _currentClient.Patronymic = TBPatron.Text;
                _currentClient.Email = TBEmail.Text;
                _currentClient.Phone = TBNumber.Text;
      /*          DateTime var = DateTime.Now;
                DateTime.TryParse(TBBirthday.Text, out var);
               */
                _currentClient.Birthday = Convert.ToDateTime(BirthdayDate.Text);
                _currentClient.RegistrationDate = DateTime.Now;
                if (RBtnFem.IsChecked == true)
                {
                    _currentClient.GenderCode = 2;
                } else
                {
                    _currentClient.GenderCode = 1;
                }
                //_currentClient.PhotoPath = "/Resources/Клиенты/picture.png";
                ValievaLanguageEntities.GetContext().Client.Add(_currentClient);
            }
            try
            {
                ValievaLanguageEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void BtnEditPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();

            myOpenFileDialog.InitialDirectory = @"C:\Users\Gulnaz\Documents\3 курс учеба\программирование\Языковая школа\Сессия 1\clients_import\Клиенты";

            if (myOpenFileDialog.ShowDialog() == true)
            {
                var relativePathIs1 = Regex.Split(myOpenFileDialog.FileName, @"C:\\Users\\Gulnaz\\Documents\\3 курс учеба\\программирование\\Языковая школа\\Сессия 1\\clients_import\\");
                _currentClient.PhotoPath = relativePathIs1[1];
                Photo.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));

                /*FileInfo fileInfo = new FileInfo(myOpenFileDialog.FileName);


                if (fileInfo.Length < 2 * 1024 * 1024)
                {
                    _currentClient.PhotoPath = myOpenFileDialog.FileName;
                    Photo.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
                }
                else
                {

                    MessageBox.Show("Размер файла превышает 2 мегабайта. Выберите другой файл.");
                }*/
            }
        }

    }
}
