using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.UserEntities;

namespace UserStorage.Service
{
    public class ServiceLogger : IService
    {
        private readonly IService service;
        private readonly BooleanSwitch traceSwitch;
        private readonly TraceSource traceSource;

        public string Name => this.service.Name;
        public void ListenForUpdates()
        {
            throw new NotImplementedException();
        }

        public ServiceMode Mode => this.service.Mode;

        public ServiceLogger(IService service)
        {
            if ((object)service == null)
            {
                if (traceSwitch.Enabled)
                    traceSource.TraceEvent(TraceEventType.Error, 0, "User service is null!");
                throw new ArgumentNullException(nameof(service));
            }
            traceSwitch = new BooleanSwitch("traceSwitch", "");
            traceSource = new TraceSource("traceSource");
            this.service = service;
        }

        public int Add(User user)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0,
                    $"Service adding method works with user whos name is {user.LastName}.");
            try
            {
                int id = this.service.Add(user);
                return id;
            }
            catch (Exception ex)
            {
                if (traceSwitch.Enabled)
                    traceSource.TraceEvent(TraceEventType.Error, 0, $"Adding of user ({user}) failed: {ex.Message}.");
                throw;
            }
        }

        public IEnumerable<User> SearchForUser(params Func<User, bool>[] predicates)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0, "Service search method works.");
            return service.SearchForUser(predicates);
        }

        public void Delete(int id)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Delete method works with user whos id is {id}.");
            try
            {
                this.service.Delete(id);
            }
            catch (Exception ex)
            {
                if (traceSwitch.Enabled)
                    traceSource.TraceEvent(TraceEventType.Error, 0, $"Deleting user failed: {ex.Message}.");
                throw;
            }
        }

        public void Save()
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0,
                    $"Saving service state");
            try
            {
                this.service.Save();
            }
            catch (Exception ex)
            {
                if (traceSwitch.Enabled)
                    traceSource.TraceEvent(TraceEventType.Error, 0, $"Saving service state failded: {ex.Message}");
                throw;
            }
        }

        public void Load()
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0,
                    $"Loading service state");
            try
            {
                this.service.Load();
            }
            catch (Exception ex)
            {
                if (traceSwitch.Enabled)
                    traceSource.TraceEvent(TraceEventType.Error, 0,$"Loading service state failded: {ex.Message}");
                throw;
            }
        }
    }
}
