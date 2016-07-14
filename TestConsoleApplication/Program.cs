using System;
using System.Collections.Generic;
using System.Configuration;
using UserStorage;
using UserStorage.Entities;
using UserStorage.Interfacies;

namespace TestConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartupFoldersConfigSection section = (StartupFoldersConfigSection)ConfigurationManager.GetSection("StartupFolders");

            if (section != null)
            {
                System.Diagnostics.Debug.WriteLine(section.FolderItems[0].FolderType);
                System.Diagnostics.Debug.WriteLine(section.FolderItems[0].Path);
            }
            //TestStorage();

            Console.ReadKey();
        }

        static void TestStorage()
        {
            IUserStorage storage = new UserStorageInMemory();
            IRepository<User> repository = new XmlRepository();

            storage.Add(new User("Name", "Surname", 12345, DateTime.Now, Gender.Female, null));
            storage.Add(new User("Name2", "Surname2", 54321, DateTime.Now, Gender.Female, null));

            try
            {
                repository.Save("list.xml", storage.GetAllUsers());
                Console.WriteLine("Saved!");
                IEnumerable<User> users = repository.Load("list.xml");
                storage.DeleteAllUsers();
                storage.AddUsers(users);
                foreach (var user in storage.GetAllUsers())
                {
                    Console.WriteLine(user.Id + " " + user.LastName);
                }
            }
            catch (ApplicationException e)
            {
            }
        }
    }
}
