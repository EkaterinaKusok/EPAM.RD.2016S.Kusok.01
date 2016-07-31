using System.Diagnostics;

namespace UserStorage.Interfacies.Logger
{
    public interface ILogger
    {
        void Log(TraceEventType traceEventType, string message);
    }
}
