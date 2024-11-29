namespace AoC24.Common
{
    public static class ListUtils
    {
        public static IEnumerable<T[]> Windowed<T>(this IEnumerable<T> enumerable, int size, int step = 1)
        {
            var list = enumerable.ToList();

            for (var i = 0; i <= list.Count - size; i += step)
            {
                yield return list.Skip(i).Take(size).ToArray();
            }
        }
    }
}
