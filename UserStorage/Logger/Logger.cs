using System.Diagnostics;
using System.Threading;
using UserStorage.Interfacies.Logger;

namespace UserStorage.Logger
{
    /// <summary>
    /// Implements functionality for logging.
    /// </summary>
    /// <seealso cref="UserStorage.Interfacies.Logger.ILogger" />
    public sealed class Logger : ILogger
    {
        private readonly BooleanSwitch boolSwitch;
        private readonly TraceSource source;
        private readonly Mutex mutex = new Mutex(false, "LogMutex");

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger()
        {
            boolSwitch = new BooleanSwitch("Switch", string.Empty);
            source = new TraceSource("Source");
        }

        /// <summary>
        /// Logs the specified trace event type.
        /// </summary>
        /// <param name="traceEventType">Type of the trace event.</param>
        /// <param name="message">The message.</param>
        public void Log(TraceEventType traceEventType, string message)
        {
            mutex.WaitOne();

            if (boolSwitch.Enabled)
            {
                source.TraceEvent(traceEventType, 0, message);
            }

            mutex.ReleaseMutex();
        }
    }
}
