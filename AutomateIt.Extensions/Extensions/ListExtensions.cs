namespace AutomateIt.Extensions.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ListExtensions
    {
        public static T RandomItem<T>(this List<T> list, params T[] exclude)
        {
            return list.RandomItem(exclude.ToList());
        }

        public static T RandomItem<T>(this List<T> list, Func<T, bool> excludeCondition)
        {
            T randomItem;
            var rnd = new Random();
            do
            {
                if (list.Count == 0) throw new Exception("Unable to get random item. There are no available items in list.");
                randomItem = list[rnd.Next(list.Count)];
                if (excludeCondition.Invoke(randomItem))
                {
                    list.Remove(randomItem);
                    continue;
                }
                break;
            }
            while (true);
            return randomItem;
        }

        public static T RandomItem<T>(this List<T> list, List<T> exclude)
        {
            return list.RandomItem(exclude.Contains);
        }

        public static List<string> TrimAll(this List<string> list)
        {
            return list.Select(i => i.Trim()).ToList();
        }

        public static void Shuffle<T>(this List<T> list)
        {
            var random = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string AsString(this IEnumerable<string> list, string delimiter = ",")
        {
            if (!list.Any()) return string.Empty;
            var s = list.Aggregate(string.Empty, (current, item) => current + item + delimiter);
            return s.Substring(0, s.Length - 1);
        }
    }
}