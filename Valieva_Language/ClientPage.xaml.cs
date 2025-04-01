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
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage;
        int RecordsPerPage = 10;
        List <Client> CurrentPageList = new List <Client> ();
        List<Client> TableList;
        public ClientPage()
        {
            InitializeComponent();
            var currentClients = ValievaLanguageEntities.GetContext().Client.ToList();
            ClientListView.ItemsSource = currentClients;
            //ComboPage.Items.Add("10");
            //ComboPage.Items.Add("50");
            //ComboPage.Items.Add("200");
            //ComboPage.Items.Add("Все");
            ComboPage.SelectedIndex = 0; // По умолчанию 10 записей
            UpdateClients();
            ComboGender.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = (sender as Button).DataContext as Client;
            var lastJoinItem = ValievaLanguageEntities.GetContext().ClientService
        .Where(p => p.ClientID == currentClient.ID)
        .ToList();

            if (lastJoinItem.Count() != 0)
                MessageBox.Show("Невозможно выполнить удаление, так как существует записи на эту услугу");
            else {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {

                        ValievaLanguageEntities.GetContext().Client.Remove(currentClient);
                        ValievaLanguageEntities.GetContext().SaveChanges();
                        ClientListView.ItemsSource = ValievaLanguageEntities.GetContext().Client.ToList();
                        UpdateClients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }

        }

        private void UpdateClients()
        {
            var currentClients = ValievaLanguageEntities.GetContext().Client.ToList();

            currentClients = currentClients.Where (p => p.LastName.ToLower().Contains(TBSearch.Text.ToLower()) || p.FirstName.ToLower().Contains(TBSearch.Text.ToLower()) || p.Patronymic.ToLower().Contains(TBSearch.Text.ToLower()) || p.Email.ToLower().Contains(TBSearch.Text.ToLower()) || p.Phone.Replace("+", "").Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").ToLower().Contains(TBSearch.Text.Replace("+", "").Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").ToLower())).ToList();

            if (ComboGender.SelectedIndex == 2)
            {
                currentClients = currentClients.Where(p => p.GenderCode == 1).ToList();
            }
            if (ComboGender.SelectedIndex == 1)
            {
                currentClients = currentClients.Where(p => p.GenderCode == 2).ToList();
            }

            if (ComboSort.SelectedIndex == 1)
            {
                currentClients = currentClients.OrderBy(p => p.LastName).ToList();
            }
            if (ComboSort.SelectedIndex == 2)
            {
                currentClients = currentClients.OrderByDescending(p => DateTime.Parse((p.LastJoin.ToString() != "нет") ? p.LastJoin.ToString() : "01.01.1991 09:00")).ToList();
            }
            if (ComboSort.SelectedIndex == 3)
            {
                currentClients = currentClients.OrderByDescending(p => p.countOfJoinsc).ToList();
            }

            if (ComboPage.SelectedItem != null)
            {
                switch (ComboPage.SelectedIndex)
                {
                    case 0:
                        RecordsPerPage = 10;
                        break;
                    case 1:
                        RecordsPerPage = 50;
                        break;
                    case 2:
                        RecordsPerPage = 200;
                        break;
                    case 3:
                        RecordsPerPage = TableList.Count; // Показать все записи
                        break;
                }
            }

            if (ComboGender.SelectedItem != null)
            {

            }

            //currentClients = currentClients.Where(

            ////для отображения итогов фильтра и поиска в листвью
            //ServiceListView.ItemsSource = currentClients.ToList();



            ClientListView.ItemsSource = currentClients;
            //
            ClientListView.Items.Refresh();
            TableList = currentClients;
            //
            //
            //
            ChangePage(0, 0);


        }

        private void ComboPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();

        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);

        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);

        }
        private void ChangePage (int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if (RecordsPerPage > 0)
            {
                CountPage = (CountRecords + RecordsPerPage - 1) / RecordsPerPage; // Вычисление количества страниц
            }
            else
            {
                CountPage = 1;
            }

            bool Ifupdate = true;
            int min;

            if (selectedPage.HasValue) // Если выбрана конкретная страница
            {
                if (selectedPage >= 0 && selectedPage < CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = Math.Min(CurrentPage * RecordsPerPage + RecordsPerPage, CountRecords);
                    for (int i = CurrentPage * RecordsPerPage; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else // Если нажата стрелка (вперёд/назад)
            {
                switch (direction)
                {
                    case 1: // Предыдущая страница
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = Math.Min(CurrentPage * RecordsPerPage + RecordsPerPage, CountRecords);
                            for (int i = CurrentPage * RecordsPerPage; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;

                    case 2: // Следующая страница
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = Math.Min(CurrentPage * RecordsPerPage + RecordsPerPage, CountRecords);
                            for (int i = CurrentPage * RecordsPerPage; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }

            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = Math.Min(CurrentPage * RecordsPerPage + RecordsPerPage, CountRecords);
                TBCount.Text = CountRecords.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();

                ClientListView.ItemsSource = CurrentPageList;
                ClientListView.Items.Refresh();
            }

        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (PageListBox.SelectedItem != null)
            {
                ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
            }
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateClients();
        }

        private void ComboGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
   //         Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Client));

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
//            Manager.MainFrame.Navigate(new AddEditPage(null));

        }
    }
}
