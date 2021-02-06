using System;

namespace HegaCore.Database.Csv
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class RowAsColumnAttribute : Attribute
    {
    }
}
