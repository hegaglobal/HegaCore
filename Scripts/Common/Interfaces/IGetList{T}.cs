using System.Collections.Generic;

namespace HegaCore
{
    public interface IGetList<T>
    {
        List<T> GetList();

        void GetList(List<T> list);
    }
}