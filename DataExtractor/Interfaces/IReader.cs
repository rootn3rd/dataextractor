using System.Collections.Generic;

namespace DataExtractor
{
    interface IReader
    {
        IEnumerable<StockEntry> Read(string fileName);
    }

}
