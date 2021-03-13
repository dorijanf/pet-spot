using NLog;

namespace PetSpot.LOGGING
{
    /// <inheritdoc/>
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Logs the message to the log which is 
        /// concerned with debugging.
        /// </summary>
        /// <param name="message"></param>
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
        /// <summary>
        /// Logs the message to the log which is 
        /// concerned with errors.
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// Logs the message to the log which is 
        /// concerned with general information.
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// Logs the message to the log which is 
        /// concerned with warnings.
        /// </summary>
        /// <param name="message"></param>
        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
