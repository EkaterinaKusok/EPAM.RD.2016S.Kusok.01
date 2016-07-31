using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using Configurator;
using UserStorage.Interfacies.Services;
using UserStorage.Interfacies.UserEntities;
using UserStorage.Service;
using UserStorage.StateSaver;
using UserStorage.Interfacies.ServiceInfo;

namespace TestConsoleApplication
{
    public class Program
    {
        private static volatile bool endWork;

        private static void Main(string[] args)
        {
            SaveExample();

            List<Thread> threads = new List<Thread>();
            var configurator = new ServiceConfigurator();
            configurator.Start();

            threads.Add(new Thread(() => WorkMaster(configurator.MasterService)));
            threads.AddRange(configurator.SlaveServices.Select(slave => new Thread(() => WorkSlave(slave))));

            foreach (Thread thread in threads)
            {
                thread.Start();
            }

            Console.WriteLine("Press any key to end work of services.");
            Console.ReadKey();
            endWork = true;

            foreach (var thread in threads)
            {
                thread.Join();
            }

            configurator.End();

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static void WorkMaster(IService master)
        {
            while (!endWork)
            {
                master.Add(new User {FirstName = "Test", LastName = "LTest"});
                User firstUser = master.SearchForUser(new Func<User, bool>[] {u => true}).FirstOrDefault();
                master.Delete(firstUser.Id);
                Thread.Sleep(1000);
            }
        }

        private static void WorkSlave(IService slave)
        {
            while (!endWork)
            {
                slave.SearchForUser(new Func<User, bool>[] {u => u.FirstName == "Test"});
                Thread.Sleep(1000);
            }
        }

        private static void SaveExample()
        {
            var saver = new XmlStateSaver();

            var users = new List<User>
            {
                new User
                {
                    PersonalId = "1",
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1996, 5, 5),
                    Gender = Gender.Female,
                    VisaRecords = new VisaRecord[]
                    {
                        new VisaRecord("Austria", new DateTime(2015, 08, 06), new DateTime(2015, 09, 16)),
                        new VisaRecord("Italy", new DateTime(2016, 01, 05), new DateTime(2016, 02, 22))
                    }
                },
                new User
                {
                    PersonalId = "2",
                    FirstName = "Unnamed",
                    LastName = "Person",
                    DateOfBirth = new DateTime(1995, 7, 3),
                    Gender = Gender.Male,
                    VisaRecords = new VisaRecord[]
                    {
                        new VisaRecord("Italy", new DateTime(2016, 01, 05), new DateTime(2016, 02, 22))
                    }
                },
                new User
                {
                    PersonalId = "3",
                    FirstName = "Test",
                    LastName = "User",
                    DateOfBirth = new DateTime(1995, 4, 13),
                    Gender = Gender.Female,
                    VisaRecords = null
                }
            };

            var state = new StorageState
            {
                CurrentId = 3,
                Users = users
            };

            saver.SaveState(state);
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
                ShowUsers(slave.SearchForUser(u => true).ToList());
                Console.WriteLine();
            }
        }

        static void ShowUsers(IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                Console.WriteLine(
                    $"{user.PersonalId})\t{user.FirstName} {user.LastName}; {user.Gender}; {user.DateOfBirth}");
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
