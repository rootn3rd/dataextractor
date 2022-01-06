using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractor
{
    record StockEntry(string ISIN, string CFICode, string Venue, string AlgoParams);

    record StockOutputEntry(string ISIN, string CFICode, string Venue, string PriceMultiplier);
}
