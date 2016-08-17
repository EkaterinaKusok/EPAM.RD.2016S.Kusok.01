using System.Diagnostics;

namespace UserStorage.Interfacies.Logger
{
    /// <summary>
    /// Provides functionality for logging.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the specified trace event type.
        /// </summary>
        /// <param name="traceEventType">Type of the trace event.</param>
        /// <param name="message">The message.</param>
        void Log(TraceEventType traceEventType, string message);
    }
}
