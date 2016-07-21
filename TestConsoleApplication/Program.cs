using System;
using System.Collections.Generic;
using System.Configuration;
using UserStorage;
using UserStorage.UserEntities;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using UserStorage.Configurator;
using UserStorage.StateSaver;
using UserStorage.UserStorage;

namespace TestConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            ServicesConfigSection servicesSection = (ServicesConfigSection)ConfigurationManager.GetSection("Services");

            Console.ReadLine();

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

        static void TestCustomConfig()
        {
            StartupFoldersConfigSection section = (StartupFoldersConfigSection)ConfigurationManager.GetSection("StartupFolders");

            if (section != null)
            {
                System.Diagnostics.Debug.WriteLine(section.FolderItems[0].FolderType);
                System.Diagnostics.Debug.WriteLine(section.FolderItems[0].Path);
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
