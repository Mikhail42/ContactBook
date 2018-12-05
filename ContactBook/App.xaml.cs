using ContactBook.Model;
using ContactBook.View;
using log4net;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ContactBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ILog log = LogManager.GetLogger(typeof(App));

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            log4net.Config.XmlConfigurator.Configure();
            log.Info("Initialising...");
            /*IEnumerable<Contact> contacts = new List<Contact>()
            {
                new Contact(ContactType.Email, "i.m@gmail.com"),
                new Contact(ContactType.Email, "i.m2@gmail.com")
            };
            Person person = new Person("pName pFamily", DateTime.Now, "note n", contacts);
            PersonViewModel.InsertAsync(person);*/
        }
    }
}
