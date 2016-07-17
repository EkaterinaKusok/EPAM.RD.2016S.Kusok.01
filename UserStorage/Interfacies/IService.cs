using System;
using System.Collections.Generic;
using UserStorage.Entities;

namespace UserStorage.Interfacies
{
    public interface IService
    {
        int AddUser(User user);
        void DeleteUser(User user);
        void Delete(int id);
        IEnumerable<int> FindByPersonalId(string personalId);
        IEnumerable<int> FindByNameAndLastName(string firstName, string lastName);
        IEnumerable<int> FindByGender(Gender gender);
        void SaveState(string fileName);
        void LoadState(string fileName);

        //IEnumerable<User> GetAllUsers();
        //IEnumerable<int> AddUsers(IEnumerable<User> users);
        //void DeleteAllUsers();
        //void SetCurrentId(int currentId);
    }
}
