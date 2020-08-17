using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HegaCore.Commands
{
    public abstract class UICommand : MonoBehaviour, ICommand
    {
        [SerializeField]
        private string key = string.Empty;

        public string Key
            => this.key;

        protected Converter converter { get; } = new Converter();

        private readonly StringBuilder sb = new StringBuilder();

        public abstract void Invoke(in Segment<object> parameters);

        protected bool ValidateParams(in Segment<object> parameters, int paramCount, string command, bool silent = false)
        {
            if (parameters.Count < paramCount)
            {
                if (!silent)
                    UnuLogger.LogError($"{command} with key={this.key} needs {paramCount} parameters.", this);

                return false;
            }

            return true;
        }

        protected void Log()
        {
            UnuLogger.Log($"<color=#4a148c>Invoke UI Command:</color> {this.key}", this);
        }

        protected void Log(object value)
        {
            UnuLogger.Log($"<color=#4a148c>Invoke UI Command:</color> {this.key}::{value}", this);
        }

        protected void Log(params object[] values)
        {
            this.sb.Clear();
            this.sb.Append($"<color=#0d47a1>Invoke UI Command:</color> {this.key}::");

            var last = values.Length - 1;

            for (var i = 0; i < values.Length; i++)
            {
                this.sb.Append(values[i]);

                if (i < last)
                    this.sb.Append("::");
            }

            UnuLogger.Log(this.sb.ToString());
        }
    }
}