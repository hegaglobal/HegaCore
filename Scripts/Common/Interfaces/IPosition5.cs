using UnityEngine;

namespace HegaCore
{
    public interface IPosition5
    {
        Vector3 Above { get; }

        Vector3 Below { get; }

        Vector3 Ahead { get; }

        Vector3 Behind { get; }

        Vector3 Center { get; }
    }
}
