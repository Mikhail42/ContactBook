using ContactBook.Model;
using ContactBook.Util;
using log4net;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ContactBook
{
    public partial class ContactBookWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(nameof(ContactBookWindow));

        private ContactBookViewModel viewModel;

        public ContactBookWindow()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            log.Info(nameof(Init));
            this.viewModel = new ContactBookViewModel();
            this.viewModel.Init();
            this.dataGrid.ItemsSource = viewModel.Book;
            dataGrid.MouseRightButtonUp += new MouseButtonEventHandler(Book_MouseRightButtonUp);
        }

        void Book_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            log.Debug(nameof(Book_MouseRightButtonUp));
            DataGridHelper.OnMouseRightButtonUp(e, (row) => dataGrid.SelectedItem = row.DataContext);
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            log.Debug(nameof(DataGridRow_MouseDoubleClick));
            DataGridRow row = sender as DataGridRow;
            Person person = row.DataContext as Person;
            viewModel.ShowPerson(person);
        }

        private async void RemoveContact_ClickAsync(object sender, RoutedEventArgs e)
        {
            log.DebugFormat("{0} with SelectedItem = {1}", nameof(RemoveContact_ClickAsync), dataGrid.SelectedItem);
            try
            {
                await this.viewModel.RemovePersonAsync(dataGrid.SelectedItem as Person);
            }
            catch (Exception exc)
            {
                log.Error("Can't remove selected person", exc);
                throw;
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            log.Info(nameof(CloseWindow));
            this.Close();
        }

        private void AddNewPerson_Click(object sender, RoutedEventArgs e)
        {
            log.Debug(nameof(AddNewPerson_Click));
            try
            {
                this.viewModel.ShowNewPerson();
            }
            catch (Exception exc)
            {
                log.Error("Can't add new person", exc);
                throw;
            }
        }
    }
}
