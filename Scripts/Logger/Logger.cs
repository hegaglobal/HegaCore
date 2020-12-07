using UnityEngine;

namespace Game
{
    public sealed class Logger : IUnuLogger
    {
        public bool EnableAllLog { get; set; } = true;

        public bool EnableLogInfo { get; set; } = true;

        public bool EnableLogWarning { get; set; } = true;

        public bool EnableLogError { get; set; } = true;

        public void Log(object message)
        {
            if (this.EnableAllLog || this.EnableLogInfo)
                Debug.Log(message);
        }

        public void Log(object message, Object context)
        {
            if (this.EnableAllLog || this.EnableLogInfo)
                Debug.Log(message, context);
        }

        public void LogFormat(string message, params object[] args)
        {
            if (this.EnableAllLog || this.EnableLogInfo)
                Debug.LogFormat(message, args);
        }

        public void LogFormat(Object context, string message, params object[] args)
        {
            if (this.EnableAllLog || this.EnableLogInfo)
                Debug.LogFormat(context, message, args);
        }

        public void LogError(object message)
        {
            if (this.EnableAllLog || this.EnableLogError)
                Debug.LogError(message);
        }

        public void LogError(object message, Object context)
        {
            if (this.EnableAllLog || this.EnableLogError)
                Debug.LogError(message, context);
        }

        public void LogErrorFormat(string message, params object[] args)
        {
            if (this.EnableAllLog || this.EnableLogError)
                Debug.LogErrorFormat(message, args);
        }

        public void LogErrorFormat(Object context, string message, params object[] args)
        {
            if (this.EnableAllLog || this.EnableLogError)
                Debug.LogErrorFormat(context, message, args);
        }

        public void LogWarning(object message)
        {
            if (this.EnableAllLog || this.EnableLogWarning)
                Debug.LogWarning(message);
        }

        public void LogWarning(object message, Object context)
        {
            if (this.EnableAllLog || this.EnableLogWarning)
                Debug.LogWarning(message, context);
        }

        public void LogWarningFormat(string message, params object[] args)
        {
            if (this.EnableAllLog || this.EnableLogWarning)
                Debug.LogWarningFormat(message, args);
        }

        public void LogWarningFormat(Object context, string message, params object[] args)
        {
            if (this.EnableAllLog || this.EnableLogWarning)
                Debug.LogWarningFormat(context, message, args);
        }

        public void LogException(System.Exception exception)
        {
            if (this.EnableAllLog || this.EnableLogError)
                Debug.LogException(exception);
        }

        public void LogException(System.Exception exception, Object context)
        {
            if (this.EnableAllLog || this.EnableLogError)
                Debug.LogException(exception, context);
        }
    }
}