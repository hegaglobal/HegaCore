namespace HegaCore.AutoGen.Templates
{
    internal static class DatabaseTemplate
    {
        public const string Template = @"using HegaCore;

namespace #_NAMESPACE_#
{
    public sealed partial class #_TYPE_PREFIX_#Database : #_BASE_TYPE_#<#_TYPE_PREFIX_#Database, #_TYPE_PREFIX_#Tables>
    {
    }
}
";
    }
}