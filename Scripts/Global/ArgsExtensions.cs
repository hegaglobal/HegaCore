public static class ArgsExtensions
{
    public static object[] Get<T>(this object[] self, out T value, int index)
    {
        value = default;

        if (self.Length > index && self[index] is T val)
            value = val;

        return self;
    }

    public static object[] GetAndNext<T>(this object[] self, out T value, ref int index)
    {
        value = default;

        if (self.Length > index && self[index] is T val)
            value = val;

        index += 1;

        return self;
    }
}