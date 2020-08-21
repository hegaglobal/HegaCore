using UnityEngine;

namespace HegaCore
{
    public interface IGetCellPosition
    {
        Vector3 GetCellPosition(in GridVector index);
    }
}