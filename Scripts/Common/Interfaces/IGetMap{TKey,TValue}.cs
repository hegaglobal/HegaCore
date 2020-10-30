using System.Collections.Generic;

namespace HegaCore
{
    public interface IGetMap<TKey, TValue>
    {
        void GetMap(Dictionary<TKey, TValue> map);
    }
}