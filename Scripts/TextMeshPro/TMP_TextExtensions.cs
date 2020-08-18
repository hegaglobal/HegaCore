using UnityEngine;
using TMPro;

namespace HegaCore
{
    public static class TMP_TextExtensions
    {
        public static void Set(this TMP_Text self, string value, Color? color = null, float? size = null, bool? raycastTarget = null)
        {
            self.SetText(value ?? string.Empty);

            if (color.HasValue)
                self.color = color.Value;

            if (size.HasValue)
                self.fontSize = size.Value;

            if (raycastTarget.HasValue)
                self.raycastTarget = raycastTarget.Value;
        }
    }
}