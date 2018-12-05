using log4net;
using System.Windows;

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
