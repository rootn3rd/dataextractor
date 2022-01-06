using System.Collections.Generic;

namespace DataExtractor
{
    interface IWriter
    {
        void Write(string fileName, IEnumerable<StockOutputEntry> records);
    }

}
