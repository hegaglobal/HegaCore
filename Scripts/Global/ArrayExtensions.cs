public static class ArrayExtensions
{
    public static object[] Get<T>(this object[] self, int index, out T value)
    {
        value = default;

        if (self.Length > index && self[index] is T val)
            value = val;

        return self;
    }

    public static object[] GetThenMoveNext<T>(this object[] self, ref int index, out T value)
    {
        value = default;

        if (self.Length > index && self[index] is T val)
            value = val;

        index += 1;

        return self;
    }
}