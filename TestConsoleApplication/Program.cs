using System;
using System.Collections.Generic;
using System.Configuration;
using UserStorage;
using UserStorage.Entities;
using UserStorage.Interfacies;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace TestConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int currentId = int.Parse(ConfigurationManager.AppSettings.Get("CurrentId"));
            string path = ConfigurationManager.AppSettings.Get("FilePath");
            Console.WriteLine(currentId);
            Console.WriteLine(path);
            currentId = 13;
            path = "file2.xml";

            Console.WriteLine("----------------");
            WriteInConfig(currentId, path);
            currentId = int.Parse(ConfigurationManager.AppSettings.Get("CurrentId"));
            path = ConfigurationManager.AppSettings.Get("FilePath");
            Console.WriteLine(currentId);
            Console.WriteLine(path);
            //TestCustomConfig();
            //TestStorage();
            Console.ReadKey();
        }

        static void TestCustomConfig()
        {
            StartupFoldersConfigSection section = (StartupFoldersConfigSection)ConfigurationManager.GetSection("StartupFolders");

            if (section != null)
            {
                System.Diagnostics.Debug.WriteLine(section.FolderItems[0].FolderType);
                System.Diagnostics.Debug.WriteLine(section.FolderItems[0].Path);
            }
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
    }
}
