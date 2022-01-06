using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataExtractor.Tests
{
    [TestClass]
    public class StockEntryExtensionTests
    {
        [TestMethod]
        public void AbleToConvertStockEntryToOutputType()
        {
            var input = new StockEntry(ISIN: "DE000ABCDEFG", CFICode: "FFICSX", Venue: "XEUR", AlgoParams: "InstIdentCode:DE000ABCDEFG|;InstFullName:DAX|;InstClassification:FFICSX|;NotionalCurr:EUR|;PriceMultiplier:20.0|;UnderlInstCode:DE0001234567|;UnderlIndexName:DAX PERFORMANCE-INDEX|;OptionType:OTHR|;StrikePrice:0.0|;OptionExerciseStyle:|;ExpiryDate:2021-01-01|;DeliveryType:PHYS|");

            var output = input.ToOutputEntry();

            Assert.IsNotNull(output);
            Assert.AreEqual(output.Venue, input.Venue);
            Assert.AreEqual(output.ISIN, input.ISIN);
            Assert.AreEqual(output.CFICode, input.CFICode);
            Assert.AreEqual(output.PriceMultiplier, "20.0");

        }

        [TestMethod]
        public void AbleToConvertAlgoParams()
        {
            var algoParams = "InstIdentCode:DE000ABCDEFG|;InstFullName:DAX|;InstClassification:FFICSX|;NotionalCurr:EUR|;PriceMultiplier:20.0|;UnderlInstCode:DE0001234567|;UnderlIndexName:DAX PERFORMANCE-INDEX|;OptionType:OTHR|;StrikePrice:0.0|;OptionExerciseStyle:|;ExpiryDate:2021-01-01|;DeliveryType:PHYS|";

            var result = StockEntryExtensions.ConvertToContractSize(algoParams);

            Assert.AreEqual(result, "20.0");
        }


        [TestMethod]
        public void AbleToFallbackWhenNullInputIsGivenConvertAlgoParams()
        {
            var result = StockEntryExtensions.ConvertToContractSize(null);

            Assert.AreEqual("", result);
        }
    }

}
