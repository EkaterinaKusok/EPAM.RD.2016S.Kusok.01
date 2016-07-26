using System;
using System.Collections.Generic;
using System.Configuration;
using UserStorage.UserEntities;
using System.IO;
using System.Linq;
using System.Threading;
using UserStorage.Configurator;
using UserStorage.Service;

namespace TestConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var configurator = new ServiceConfigurator();
            configurator.Start();
            configurator.MasterService.Add(new User(0,"TestName","TestLastName","PassportNo",DateTime.Now,
                Gender.Female, null));
            ShowUsers(configurator.MasterService.SearchForUser(u=>true).ToList());
            Console.WriteLine();
            Thread.Sleep(1000);
            ShowSlaves(configurator.SlaveServices);
            Console.ReadLine();
            var date = DateTime.Now;
            //var users = new List<User>()
            //{
            //    new User("Name", "Surname", "12345", DateTime.Now, Gender.Female, null),
            //    new User("Name2", "Surname2", "54321", DateTime.Now, Gender.Female, null)
            //};
            //State state = new State(users, 3);
            //string path = ConfigurationManager.AppSettings.Get("FilePath");
            //Console.WriteLine(path);
            //IStateSaver saver = new XmlStateSaver();

            //saver.SaveState(path, state);
            //State newState = saver.LoadState(path);


            //int currentId = int.Parse(ConfigurationManager.AppSettings.Get("CurrentId"));
            //string path = ConfigurationManager.AppSettings.Get("FilePath");
            //Console.WriteLine(currentId);
            //Console.WriteLine(path);
            //currentId = 13;
            //path = "file2.xml";

            //Console.WriteLine("----------------");
            //WriteInConfig(currentId, path);
            //currentId = int.Parse(ConfigurationManager.AppSettings.Get("CurrentId"));
            //path = ConfigurationManager.AppSettings.Get("FilePath");
            //Console.WriteLine(currentId);
            //Console.WriteLine(path);

            //TestCustomConfig();
            //TestStorage();
            Console.ReadKey();
        }

        static void WriteInConfig(int currentId, string filePath)
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains("CurrentId"))
            {
                // открываем текущий конфиг специальным обьектом
                System.Configuration.Configuration currentConfig =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                // добавляем позицию в раздел AppSettings
                currentConfig.AppSettings.Settings.Add("CurrentId", currentId.ToString());
                //сохраняем
                currentConfig.Save(ConfigurationSaveMode.Full);
                //принудительно перезагружаем соотвествующую секцию
                ConfigurationManager.RefreshSection("appSettings");
            }
            else if (!ConfigurationManager.AppSettings.AllKeys.Contains("FilePath"))
            {
                System.Configuration.Configuration currentConfig =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                currentConfig.AppSettings.Settings.Add("FilePath", filePath);
                currentConfig.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }
            else
            {
                System.Configuration.Configuration currentConfig =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                currentConfig.AppSettings.Settings["CurrentId"].Value = currentId.ToString();
                currentConfig.AppSettings.Settings["FilePath"].Value = filePath;
                currentConfig.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        static void ShowSlaves(IEnumerable<IService> slaves)
        {
            foreach (var slave in slaves)
            {
                ShowUsers(slave.SearchForUser(u=>true).ToList());
                Console.WriteLine();
            }
        }

        static void ShowUsers(IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                Console.WriteLine($"{user.PersonalId})\t{user.FirstName} {user.LastName}; {user.Gender}; {user.DateOfBirth}");
                Console.Write($"Visas: ");

                if (user.VisaRecords == null)
                {
                    Console.WriteLine("no visas");
                    return;
                }

                foreach (var visa in user.VisaRecords)
                {
                    Console.Write($"{visa.Country}  ");
                }
                Console.WriteLine();
            }
        }
    }
}
