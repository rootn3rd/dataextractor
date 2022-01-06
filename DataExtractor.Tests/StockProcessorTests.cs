using DataExtractor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataExtractor.Tests
{
    [TestClass]
    public class StockProcessorTests
    {
        [TestMethod]
        public void AbleToProcessFilesCorrectly()
        {
            var stockEntries = new List<StockEntry>()
            {
                new StockEntry (ISIN: "DE000ABCDEFG", CFICode: "FFICSX", Venue: "XEUR", AlgoParams: "InstIdentCode:DE000ABCDEFG|;InstFullName:DAX|;InstClassification:FFICSX|;NotionalCurr:EUR|;PriceMultiplier:20.0|;UnderlInstCode:DE0001234567|;UnderlIndexName:DAX PERFORMANCE-INDEX|;OptionType:OTHR|;StrikePrice:0.0|;OptionExerciseStyle:|;ExpiryDate:2021-01-01|;DeliveryType:PHYS|" ),
                new StockEntry ( ISIN: "PL0ABCDEFGHI", CFICode: "FFICSX", Venue:"WDER", AlgoParams :"InstIdentCode:PL0ABCDEFGHI|;InstFullName:Wig 20 Index|;InstClassification:FFICSX|;NotionalCurr:PLN|;PriceMultiplier:25.0|;UnderlInstCode:PL9991234567|;UnderlIndexName:WIG20 PLN|;OptionType:OTHR|;StrikePrice:0.0|;OptionExerciseStyle:|;ExpiryDate:2021-01-01|;DeliveryType:PHYS|")
            };
            var readerMock = new Mock<IReader>();

            readerMock.Setup(x => x.Read(It.IsAny<string>())).Returns(stockEntries);

            var responses = Enumerable.Empty<StockOutputEntry>();
            var writerMock = new Mock<IWriter>();
            writerMock.Setup(x => x.Write(It.IsAny<string>(), It.IsAny<IEnumerable<StockOutputEntry>>()))
                .Callback<string, IEnumerable<StockOutputEntry>>((s, res) => responses = res);

            var processor = StockProcessor.Create(readerMock.Object, writerMock.Object, (s) => s.ToOutputEntry());

            var outputFileName = "output.csv";
            var inputFileName = "input.csv";

            processor.Process(inputFileName, outputFileName);

            readerMock.Verify(t => t.Read(It.Is<string>(x => x == inputFileName)), Times.Once);

            writerMock.Verify(t => t.Write(It.Is<string>(x => x == outputFileName), It.IsAny<IEnumerable<StockOutputEntry>>()), Times.Once);
            Assert.IsTrue(responses.Any());
            Assert.IsTrue(responses.Count() == 2);
            Assert.IsTrue(responses.Any(t => t.Venue == "XEUR" && t.ISIN == "DE000ABCDEFG" && t.CFICode == "FFICSX" && t.PriceMultiplier == "20.0"));
            Assert.IsTrue(responses.Any(t => t.Venue == "WDER" && t.ISIN == "PL0ABCDEFGHI" && t.CFICode == "FFICSX" && t.PriceMultiplier == "25.0"));

        }

        [TestMethod]
        public void AbleToUseDelegateVersion()
        {
            var stockEntries = new List<StockEntry>()
            {
                new StockEntry (ISIN: "DE000ABCDEFG", CFICode: "FFICSX", Venue: "XEUR", AlgoParams: "InstIdentCode:DE000ABCDEFG|;InstFullName:DAX|;InstClassification:FFICSX|;NotionalCurr:EUR|;PriceMultiplier:20.0|;UnderlInstCode:DE0001234567|;UnderlIndexName:DAX PERFORMANCE-INDEX|;OptionType:OTHR|;StrikePrice:0.0|;OptionExerciseStyle:|;ExpiryDate:2021-01-01|;DeliveryType:PHYS|" ),
                new StockEntry ( ISIN: "PL0ABCDEFGHI", CFICode: "FFICSX", Venue:"WDER", AlgoParams :"InstIdentCode:PL0ABCDEFGHI|;InstFullName:Wig 20 Index|;InstClassification:FFICSX|;NotionalCurr:PLN|;PriceMultiplier:25.0|;UnderlInstCode:PL9991234567|;UnderlIndexName:WIG20 PLN|;OptionType:OTHR|;StrikePrice:0.0|;OptionExerciseStyle:|;ExpiryDate:2021-01-01|;DeliveryType:PHYS|")
            };

            var responses = Enumerable.Empty<StockOutputEntry>();
            var resultOutputFileName = string.Empty;
            var resultInputFileName = string.Empty;

            Func<string, IEnumerable<StockEntry>> readerMock = (s) =>
            {
                resultInputFileName = s;
                return stockEntries;
            };

            Action<string, IEnumerable<StockOutputEntry>> writerMock = (s, obj) =>
            {
                resultOutputFileName = s;
                responses = obj;
            };

            Func<StockEntry, StockOutputEntry> converter = (s) => s.ToOutputEntry();

            var processor = StockProcessor.Create(readerMock, writerMock, converter);

            var outputFileName = "output.csv";
            var inputFileName = "input.csv";

            processor.Process(inputFileName, outputFileName);

            Assert.AreEqual(inputFileName, resultInputFileName);
            Assert.AreEqual(outputFileName, resultOutputFileName);

            Assert.IsTrue(responses.Any());
            Assert.IsTrue(responses.Count() == 2);
            Assert.IsTrue(responses.Any(t => t.Venue == "XEUR" && t.ISIN == "DE000ABCDEFG" && t.CFICode == "FFICSX" && t.PriceMultiplier == "20.0"));
            Assert.IsTrue(responses.Any(t => t.Venue == "WDER" && t.ISIN == "PL0ABCDEFGHI" && t.CFICode == "FFICSX" && t.PriceMultiplier == "25.0"));
        }

        [TestMethod]
        public void ProcessCreationParamsHandling()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StockProcessor.Create(null, new CSVWriter(), (a) => a.ToOutputEntry()));
            Assert.ThrowsException<ArgumentNullException>(() => StockProcessor.Create(new CSVReader(), null, (a) => a.ToOutputEntry()));
            Assert.ThrowsException<ArgumentNullException>(() => StockProcessor.Create(new CSVReader(), new CSVWriter(), null));
        }
    }

}
