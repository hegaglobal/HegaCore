using UnityEngine;

namespace HegaCore
{
    public interface IComponentView : IView
    {
        Transform transform { get; }

        GameObject gameObject { get; }
    }
}