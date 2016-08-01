using System;
using System.Collections.Generic;
using System.Threading;
using UserStorage.Interfacies.ServiceInfo;
using UserStorage.Interfacies.Services;
using UserStorage.Interfacies.UserEntities;
using UserStorage.StateSaver;
using WcfServiceLibrary.WcfConfigurator;

namespace WcfService
{
    public class Program
    {
        private static volatile bool endWork;

        private static void Main(string[] args)
        {
            List<User> users = InitialUsers();
            
            var configurator = new WcfServiceConfigurator();
            configurator.Start();
            
            Console.WriteLine("Press <Enter> to stop the services.");
            Console.ReadLine();

            configurator.End();
            }

        private static void WorkMaster(IService master)
        {
            while (!endWork)
            {
                master.Add(new User
                {
                    FirstName = "Unnamed",
                    LastName = "Person2"
                });
                var users = master.SearchForUser(u => true);
                if (users != null || users.Count != 0)
                {
                    master.Delete(users[0].Id);
                }

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

            saver.SaveState(state);
            return users;
        }
    }
}
