using System;
using System.Collections.Generic;
using System.Linq;
using UserStorage.Entities;
using UserStorage.Interfacies;

namespace UserStorage.Concrete
{
    public class SlaveService : IService
    {
        private int _lastId = 0;
        private readonly IUserStorage _storage = new MemoryUserStorage();
        public SlaveService(MasterService masterService)
        {
            masterService.UserChanged += OnUsersChanged;
            var state = masterService.InitSlaveRepository();
            _storage.Save(state.Users);
            _lastId = state.LastId;
        }
        public int AddUser(User user)
        {
            throw new NotSupportedException();
        }

        public void DeleteUser(User user)
        {
            throw new NotSupportedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> FindByGender(Gender gender)
        {
            return _storage.SearchForUser(u => u.Gender == gender).Select(u=>u.Id);
        }

        public void SaveState(string fileName)
        {
            throw new NotSupportedException();
        }

        public void LoadState(string fileName)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<int> FindByNameAndLastName(string firstName, string lastName)
        {
            return _storage.SearchForUser(u => u.FirstName == firstName && u.LastName == lastName).Select(u => u.Id);
        }

        public IEnumerable<int> FindByPersonalId(string personalId)
        {
            return _storage.SearchForUser(u => u.PersonalId == personalId).Select(u => u.Id);
        }
        private void OnUsersChanged(object sender, StorageEventArgs e)
        {
            if (e.IsDelete)
                _storage.Delete(e.User);
            else
            {
                _storage.Add(e.User);
                _lastId = e.LastId;
            }
        }
    }
}
