namespace HegaCore.AutoGen.Templates
{
    internal static class DataCommandRegistererTemplate
    {
        public const string Template = @"using HegaCore.Events.Commands;

namespace #_NAMESPACE_#.Events.Commands
{
    public sealed partial class #_TYPE_PREFIX_#DataCommandRegisterer : DataCommandRegisterer
    {
    }
}
";
    }
}