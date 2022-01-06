using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace DataExtractor.Tests
{
    [TestClass]
    public class ReaderTests
    {
        [TestMethod]
        public void AbleToReadProperly()
        {
            var reader = new CSVReader();
            var inputFileName = @"Data\\input.csv";
            var results = reader.Read(inputFileName);

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.Count() == 2);
        }

        [TestMethod]
        public void HandlesFileNotFoundErrors()
        {
            var reader = new CSVReader();
            var inputFileName = @"Data\\input_notpresent.csv";
            Assert.ThrowsException<FileNotFoundException>(() => reader.Read(inputFileName));
        }
    }

}
