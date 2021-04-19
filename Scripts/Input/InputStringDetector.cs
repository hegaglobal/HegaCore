using System;
using UnityEngine;

namespace HegaCore
{
    public sealed class InputStringDetector : InputDetectorSimple<string>
    {
        [SerializeField]
        private bool ignoreCase = false;

        protected override string None => string.Empty;

        public override void SetDetectedInput(string value)
            => base.SetDetectedInput(string.IsNullOrEmpty(value) ? this.None : value);

        private StringComparison GetComparison()
            => this.ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        public override bool Get(string input)
            => this.DetectedInput != this.None && string.Equals(input, this.DetectedInput, GetComparison());
    }
}
