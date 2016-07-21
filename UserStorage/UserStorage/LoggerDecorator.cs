using System;
using System.Collections.Generic;
using System.Diagnostics;
using UserStorage.UserEntities;
using UserStorage.Validator;

namespace UserStorage.UserStorage
{
    public class LoggerDecorator : IUserStorage
    {

        private readonly IUserStorage userStorage;
        private readonly BooleanSwitch traceSwitch;
        private readonly TraceSource traceSource;

        public LoggerDecorator(IUserStorage userStorage)
        {
            if (userStorage == null)
            {
                if (traceSwitch.Enabled)
                    traceSource.TraceEvent(TraceEventType.Error, 0, "User service is null!");
                throw new ArgumentNullException(nameof(userStorage));
            }
            traceSwitch = new BooleanSwitch("traceSwitch", "");
            traceSource = new TraceSource("traceSource");
            this.userStorage = userStorage;
        }

        public int Add(User user, IUserValidator validator = null)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0,
                    $"Add methodethod works with user whos name is {user.LastName}.");
            return userStorage.Add(user, validator);
        }

        public IEnumerable<User> SearchForUser(params Func<User, bool>[] predicates)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0, "Search method works.");
            return userStorage.SearchForUser(predicates);
        }

        public void Delete(User user)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0,
                    $"Delete method works with user whos name is {user.LastName}.");
            userStorage.Delete(user);
        }

        public void Delete(int id)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Delete method works with user whos id is {id}.");
            userStorage.Delete(id);
        }
    }
}
