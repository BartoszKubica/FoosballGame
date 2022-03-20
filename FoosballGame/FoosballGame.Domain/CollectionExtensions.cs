namespace FoosballGame.Domain
{
    public static class CollectionExtensions
    {
        public static void AddIfNotNull<TValue>(this IList<TValue> list, TValue? value)
        {
            if (value != null)
                list.Add(value);
        }
    }
}