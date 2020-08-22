using UnityEngine;

namespace HegaCore
{
    public interface ITextEmitter
    {
        void Emit(object value, in Vector3 position, in TextEmitterParams @params);

        void Emit(string value, in Vector3 position, in TextEmitterParams @params);

        void Emit(object value, in Vector3 position, in Vector3 offset, in Color color, float? size = null);

        void Emit(string value, in Vector3 position, in Vector3 offset, in Color color, float? size = null);
    }
}