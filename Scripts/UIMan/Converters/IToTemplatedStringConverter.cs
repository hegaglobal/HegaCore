using UnityEngine;
using UnuGames.MVVM;

namespace HegaCore.MVVM
{
    [System.Serializable]
    public sealed class IToTemplatedStringConverter : Converter<IToTemplatedString, IToTemplatedStringAdapter>
    {
        public IToTemplatedStringConverter(string label) : base(label) { }

        protected override IToTemplatedString ConvertWithoutAdapter(object value, Object context)
            => IToTemplatedStringAdapter.Convert(value, Data.None, context);

        protected override object ConvertWithoutAdapter(IToTemplatedString value, Object context)
            => IToTemplatedStringAdapter.Convert(value);
    }
}