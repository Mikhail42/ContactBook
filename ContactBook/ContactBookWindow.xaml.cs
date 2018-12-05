using ContactBook.Model;
using ContactBook.View;
using log4net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ContactBook
{
    public partial class ContactBookWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger("ContactBookWindow");

        private ContactBookViewModel viewModel;

        public ContactBookWindow()
        {
            log.Info("Start");
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            this.viewModel = new ContactBookViewModel();
            this.viewModel.Init();
            dataGrid.ItemsSource = viewModel.Book;
            dataGrid.MouseRightButtonUp += new MouseButtonEventHandler(Book_MouseRightButtonUp);
        }

        void Book_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            log.Debug("Book_MouseRightButtonUp");
            DependencyObject dep = e.OriginalSource as DependencyObject;
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null) return;

            while ((dep != null) && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null) return;

            DataGridRow row = dep as DataGridRow;
            dataGrid.SelectedItem = row.DataContext;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            log.Debug("DataGridRow_MouseDoubleClick");
            DataGridRow row = sender as DataGridRow;
            Person person = row.DataContext as Person;
            viewModel.ShowPerson(person);
        }

        private void RemoveContact_Click(object sender, RoutedEventArgs e)
        {
            log.DebugFormat("RemoveContact_Click with SelectedIndex = {0}", dataGrid.SelectedIndex);
            this.viewModel.RemovePerson(dataGrid.SelectedItem as Person);
        }
    }
}
