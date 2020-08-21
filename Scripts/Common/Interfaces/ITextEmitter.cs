using UnityEngine;

namespace HegaCore
{
    public interface ITextEmitter
    {
        void Emit(string value, Vector3 position, Color color, float? size = null);
    }
}