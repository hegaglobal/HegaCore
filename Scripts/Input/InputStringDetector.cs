using System;
using UnityEngine;

namespace HegaCore
{
    public class InputStringDetector : MonoBehaviour, IInput<string>
    {
        [SerializeField]
        private bool ignoreCase = true;

        private const string None = "";

        private string detectedInput = None;

        public void SetDetectedInput(string value)
            => this.detectedInput = string.IsNullOrEmpty(value) ? None : value;

        private StringComparison GetComparison()
            => this.ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        public bool Get(string input)
            => this.detectedInput != None && string.Equals(input, this.detectedInput, GetComparison());

        public void ResetInput()
            => this.detectedInput = None;
    }
}
