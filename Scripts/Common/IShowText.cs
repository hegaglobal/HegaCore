using UnityEngine;

namespace HegaCore
{
    public interface IShowText
    {
        void Show(string value, Vector3 position, Color color, float? size = null);
    }
}