using ContactBook.Model;
using ContactBook.Util;
using log4net;
using System;
using System.Windows;
using System.Windows.Input;

namespace ContactBook.View
{
    public partial class PersonView : Window
    {
        private static readonly ILog log = LogManager.GetLogger(nameof(PersonView));

        public delegate void PersonViewCloseEvent();

        public PersonViewCloseEvent OnPersonViewClose;

        public PersonView(PersonViewModel model)
        {
            log.Debug("Init PersonView");
            InitializeComponent();
            this.Model = model;
            contacts.MouseRightButtonUp += new MouseButtonEventHandler(Contacts_MouseRightButtonUp);
            this.Closing += (e, v) => OnPersonViewClose?.Invoke();
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
            log.Debug(nameof(Contacts_MouseRightButtonUp));
            DataGridHelper.OnMouseRightButtonUp(e, (row) => contacts.SelectedItem = row.DataContext);
        }

        private void CopyContact_Click(object sender, RoutedEventArgs e)
        {
            log.DebugFormat("CopyContact_Click with SelectedIndex = {0}", contacts.SelectedIndex);
            Clipboard.SetText((contacts.SelectedItem as Contact).Value);
        }

        private void RemoveContact_Click(object sender, RoutedEventArgs e)
        {
            log.DebugFormat("RemoveContact_Click with SelectedIndex = {0}", contacts.SelectedIndex);
            try
            {
                this.Model.RemoveContact(contacts.SelectedItem as Contact);
            }
            catch (Exception exc)
            {
                log.Error("Can't remove contact", exc);
                throw;
            }
        }

        private async void saveBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            log.Debug(nameof(saveBtn_ClickAsync));
            try
            {
                this.Model.Changable = false;
                await this.Model.SavePersonAsync();
            }
            catch (Exception exc)
            {
                log.Error("Can't save person", exc);
                throw;
            }
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Debug(nameof(editBtn_Click));
            this.Model.Changable = true;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            log.Debug(nameof(CloseWindow));
            this.Close();
        }

        private void NewContact_Click(object sender, RoutedEventArgs e)
        {
            log.Debug(nameof(NewContact_Click));
            this.Model.AddNewContact();
        }
    }
}
