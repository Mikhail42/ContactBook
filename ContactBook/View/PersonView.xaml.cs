using ContactBook.Util;
using log4net;
using System.Windows;
using System.Windows.Input;

namespace ContactBook.View
{
    public partial class PersonView : Window
    {
        private static readonly ILog log = LogManager.GetLogger("PersonView");

        public PersonView(PersonViewModel model)
        {
            log.Debug("Init PersonView");
            InitializeComponent();
            this.Model = model;
            contacts.MouseRightButtonUp += new MouseButtonEventHandler(Contacts_MouseRightButtonUp);
        }

        public PersonViewModel Model
        {
            get => this.DataContext as PersonViewModel;
            set
            {
                log.DebugFormat("DataContext new value is {0}", value);
                this.DataContext = value;
            }
        }

        void Contacts_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            log.Debug("Book_MouseRightButtonUp");
            DataGridHelper.OnMouseRightButtonUp(e, (row) => contacts.SelectedItem = row.DataContext);
        }

        private void RemoveContact_Click(object sender, RoutedEventArgs e)
        {
            log.DebugFormat("RemoveContact_Click with SelectedIndex = {0}", contacts.SelectedIndex);
            // this.viewModel.RemovePerson(dataGrid.SelectedItem as Person); // TODO
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Model.Changable = false;
            this.Model.Save();
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Model.Changable = true;
        }
    }
}
