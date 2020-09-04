using System;

namespace HegaCore
{
    public static class TextEmitterExtensions
    {
        public static TextEmission GetEmission<T>(this ITextEmitter self, T key) where T : struct, Enum
            => self.GetEmission(key.ToString());
    }
}