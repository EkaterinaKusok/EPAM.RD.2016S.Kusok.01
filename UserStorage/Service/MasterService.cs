using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UserStorage.UserEntities;
using UserStorage.Generator;
using UserStorage.StateSaver;
using UserStorage.UserStorage;
using UserStorage.Validator;

namespace UserStorage.Service
{
    public class MasterService :IService
    {
        private int _lastId = 0;
        private readonly IUserStorage _storage = new MemoryUserStorage();
        private readonly IStateSaver _stateSaver = new XmlStateSaver();
        private readonly IUserValidator _validator = new CustomUserValidator();
        private readonly IGenerator<int> _generator = new PrimeIdGenerator();
        public event EventHandler<StorageEventArgs> UserChanged = delegate { };
        private static readonly BooleanSwitch boolSwitch = new BooleanSwitch("logSwitch", string.Empty);

        public State InitSlaveRepository()
        {
            return new State() { Users = _storage.Load(), LastId = _lastId };
        }

        public int AddUser(User user)
        {
            //if (!_validator.ValidateUser(user))
            //{
            //    throw new ArgumentException(nameof(user));
            //}
            _lastId = _generator.GenerateNewId(_lastId);
            user.Id = _lastId;
            _storage.Add(user);
            OnUserChanged(new StorageEventArgs(user, _lastId, false));
            if (boolSwitch.Enabled)
            {
                Trace.TraceInformation($"Add user. {user.ToString()}");
            }
            return _lastId;
        }

        public void DeleteUser(User user)
        {
            _storage.Delete(user);
            OnUserChanged(new StorageEventArgs(user, _lastId, true));
            if (boolSwitch.Enabled)
            {
                Trace.TraceInformation("Delete user. {user.ToString()}");
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> FindByGender(Gender gender)
        {
            return _storage.SearchForUser(u => u.Gender == gender).Select(u => u.Id);
        }

        public IEnumerable<int> FindByNameAndLastName(string firstName, string lastName)
        {
            return _storage.SearchForUser(u => u.FirstName == firstName && u.LastName == lastName).Select(u => u.Id);
        }

        public IEnumerable<int> FindByPersonalId(string personalId)
        {
            return _storage.SearchForUser(u => u.PersonalId == personalId).Select(u => u.Id);
        }

        public void SaveState(string fileName)
        {
            State state = new State() { Users = _storage.Load(), LastId = _lastId };
            _stateSaver.SaveState(fileName, state);
            if (boolSwitch.Enabled)
            {
                Trace.TraceInformation("Save state. {fileName}");
            }
        }

        public void LoadState(string fileName)
        {
            State state = _stateSaver.LoadState(fileName);
            _storage.Save(state.Users);
            _lastId = state.LastId;
            if (boolSwitch.Enabled)
            {
                Trace.TraceInformation("Load state. {fileName}");
            }
        }
        protected virtual void OnUserChanged(StorageEventArgs e)
        {
            UserChanged.Invoke(this, e);
        }
    }
}
