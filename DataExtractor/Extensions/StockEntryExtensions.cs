using System;
using System.Collections.Generic;
using System.Linq;

namespace DataExtractor
{
    static class StockEntryExtensions
    {
        internal static StockOutputEntry ToOutputEntry(this StockEntry stockEntry)
        {
            if (stockEntry == null) return null;

            return new StockOutputEntry(stockEntry.ISIN, stockEntry.CFICode, stockEntry.Venue, ConvertToContractSize(stockEntry.AlgoParams));
        }
        internal static string ConvertToContractSize(string str) => Convert(str, "PriceMultiplier", s => s.TrimEnd('|'));

        internal static string Convert(string str, string key, Func<string, string> selector = default)
        {
            if (selector == default) selector = s => s;
            if (string.IsNullOrEmpty(str)) return string.Empty;

            var elem = from element in str.AsLookup()
                       where element.Key == key
                       select selector(element.Value);

            return elem.DefaultIfEmpty("").FirstOrDefault();

        }

        internal static IEnumerable<KeyValuePair<string, string>> AsLookup(this string s, char pairDelimiter = ';', char valueDelimiter = ':')
        {
            var q = from element in s.Split(pairDelimiter, StringSplitOptions.RemoveEmptyEntries)
                    let kvp = element.Split(valueDelimiter, StringSplitOptions.RemoveEmptyEntries)
                    select new KeyValuePair<string, string>(kvp.First(), kvp.Skip(1).FirstOrDefault());

            return q;
        }
    }

}
