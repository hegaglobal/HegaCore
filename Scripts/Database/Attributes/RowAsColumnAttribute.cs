using System;

namespace HegaCore.Database.Csv
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class RowAsColumnAttribute : Attribute
    {
    }
}
