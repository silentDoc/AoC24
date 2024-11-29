namespace AoC24.Common
{
    public static class ParseUtils
    {
        public static List<List<string>> SplitBy(List<string> elements, string splitValue)
        {
            List<string> set = new();
            List<List<string>> retVal = new();

            foreach (string element in elements)
            {
                if (element.Equals(splitValue))
                {
                    retVal.Add(set);
                    set = new();
                }
                else
                    set.Add(element);
            }
            retVal.Add(set);
            return retVal;
        }
    }
}
