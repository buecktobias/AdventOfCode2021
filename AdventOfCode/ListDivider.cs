using System.Collections.Generic;

namespace AdventOfCode
{
    public class ListDivider
    {
        internal static List<T> GetListAtIndexFrom2DList<T>(List<List<T>> twoDimensionalList, int index)
        {
            var newListDividedAtIndex = new List<T>();
            foreach (var chars in twoDimensionalList) newListDividedAtIndex.Add(chars[index]);

            return newListDividedAtIndex;
        }
    }
}