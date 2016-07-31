using System;
using System.Collections.Generic;
using System.Diagnostics;
using UserStorage.Interfacies.Storages;
using UserStorage.Interfacies.UserEntities;

namespace UserStorage.UserStorage
{
    public class UserStorageLogger : IUserStorage
    {
        private readonly IUserStorage userStorage;
        private readonly BooleanSwitch traceSwitch;
        private readonly TraceSource traceSource;

        public UserStorageLogger(IUserStorage userStorage)
        {
            if (userStorage == null)
            {
                if (traceSwitch.Enabled)
                    traceSource.TraceEvent(TraceEventType.Error, 0, "User storage is null!");
                throw new ArgumentNullException(nameof(userStorage));
            }
            traceSwitch = new BooleanSwitch("traceSwitch", "");
            traceSource = new TraceSource("traceSource");
            this.userStorage = userStorage;
        }

        public int Add(User user)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0,
                    $"Add method works with user whos name is {user.LastName}.");
            return userStorage.Add(user);
        }

        public IEnumerable<User> SearchForUser(params Func<User, bool>[] predicates)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0, "Search method works.");
            return userStorage.SearchForUser(predicates);
        }

        //public void Delete(User user)
        //{
        //    if (traceSwitch.Enabled)
        //        traceSource.TraceEvent(TraceEventType.Information, 0,
        //            $"Delete method works with user whos name is {user.LastName}.");
        //    userStorage.Delete(user);
        //}

        public void Delete(int id)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Delete method works with user whos id is {id}.");
            userStorage.Delete(id);
        }

        public void Save()
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0, "Save user list.");
            userStorage.Save();
        }

        public void Load()
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0, "Load user list.");
            userStorage.Load();
        }
    }
}
