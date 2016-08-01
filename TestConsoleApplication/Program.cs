using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using Configurator;
using UserStorage.Interfacies.ServiceInfo;
using UserStorage.Interfacies.Services;
using UserStorage.Interfacies.UserEntities;
using UserStorage.StateSaver;

namespace TestConsoleApplication
{
    public class Program
    {
        private static volatile bool endWork;

        private static void Main(string[] args)
        {
            List<User> users = InitialUsers();

            List<Thread> threads = new List<Thread>();
            var configurator = new ServiceConfigurator();
            configurator.Start();
            foreach (var user in users)
            {
                configurator.MasterService.Add(user);
            }

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
                master.Add(new User
                {
                    FirstName = "Test",
                    LastName = "LTest"
                });
                var users = master.SearchForUser(u => true);
                if (users != null || users.Count != 0)
                {
                    master.Delete(users[0].Id);
                }

                Thread.Sleep(1000);
            }
        }

        private static void WorkSlave(IService slave)
        {
            while (!endWork)
            {
                // ShowUsers(slave.SearchForUser(u => true).ToList());
                slave.SearchForUser(u => u.FirstName == "Test");
                Thread.Sleep(1000);
            }
        }

        private static List<User> InitialUsers()
        {
            var saver = new XmlStateSaver();

            var users = new List<User>
            {
                new User
                {
                    Id = 1,
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
                    Id = 2,
                    PersonalId = "2",
                    FirstName = "Unnamed",
                    LastName = "Person",
                    DateOfBirth = new DateTime(1989, 7, 3),
                    Gender = Gender.Male,
                    VisaRecords = new VisaRecord[]
                    {
                        new VisaRecord("Italy", new DateTime(2016, 01, 05), new DateTime(2016, 02, 22))
                    }
                },
                new User
                {
                    Id = 3,
                    PersonalId = "3",
                    FirstName = "Test",
                    LastName = "User",
                    DateOfBirth = new DateTime(1905, 4, 13),
                    Gender = Gender.Female,
                    VisaRecords = null
                }
            };

            var state = new StorageState
            {
                CurrentId = 5,
                Users = users
            };

            // saver.SaveState(state);
            return users;
        }

        private static void WriteInConfig(int currentId, string filePath)
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains("CurrentId"))
            {
                // открываем текущий конфиг специальным обьектом
                System.Configuration.Configuration currentConfig =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
                // добавляем позицию в раздел AppSettings
                currentConfig.AppSettings.Settings.Add("CurrentId", currentId.ToString());
                
                // сохраняем
                currentConfig.Save(ConfigurationSaveMode.Full);
                
                // принудительно перезагружаем соотвествующую секцию
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

        private static void ShowSlaves(IEnumerable<IService> slaves)
        {
            foreach (var slave in slaves)
            {
                ShowUsers(slave.SearchForUser(u => true).ToList());
                Console.WriteLine();
            }
        }

        private static void ShowUsers(IEnumerable<User> users)
        {
            if (users == null || users.Count() == 0)
            {
                Console.WriteLine("No users.");
                return;
            }

            foreach (var user in users)
            {
                Console.Write(user.FirstName + " " + user.LastName + "; ");
            }

            Console.WriteLine();
        }
    }
}