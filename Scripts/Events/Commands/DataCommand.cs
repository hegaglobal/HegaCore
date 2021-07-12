using System;
using System.Collections.Generic;
using System.Text;
using VisualNovelData;
using VisualNovelData.Commands;

namespace HegaCore.Events.Commands
{
    [Serializable]
    public abstract class DataCommand : BaseCommand, IEventCommand
    {
        public abstract string Key { get; }

        public abstract bool Ignorable { get; }

        private readonly StringBuilder sb = new StringBuilder();

        protected bool ValidateParameters(in Segment<object> parameters, int paramCount, string command, bool silent = false)
        {
            if (parameters.Count < paramCount)
            {
                if (!silent)
                    UnuLogger.LogError($"{command} with key={this.Key} needs {paramCount} parameters.");

                return false;
            }

            return true;
        }

        protected void Log()
        {
            UnuLogger.Log($"Invoke Data Command: {this.Key}");
        }

        protected void Log(object value)
        {
            UnuLogger.Log($"Invoke Data Command: {this.Key}::{value}");
        }

        protected void Log(params object[] values)
        {
            this.sb.Clear();
            this.sb.Append($"Invoke Data Command: {this.Key}::");

            var last = values.Length - 1;

            for (var i = 0; i < values.Length; i++)
            {
                this.sb.Append(values[i]);

                if (i < last)
                    this.sb.Append("::");
            }

            UnuLogger.Log(this.sb.ToString());
        }

        public override void Invoke(in Metadata metadata, in Segment<object> parameters)
            => Invoke(parameters);

        public virtual void Invoke(in Segment<object> parameters) { }
    }
}