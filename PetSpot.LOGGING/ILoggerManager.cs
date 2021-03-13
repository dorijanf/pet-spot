namespace PetSpot.LOGGING
{
    /// <summary>
    /// Logger manager class defines 4 methods which are used
    /// for storing messages in logs. The methods are divided
    /// by message severity (information, warning, debug, error)
    /// </summary>
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
    }
}
