## DataExtractor

This is a command line utility that processes a csv stockinput file and translates to another csv output file.

# Usage 

This can be invoked from command line as follows - 

> DataExtractor.exe input_file_path_csv output_file_path_csv

input_file_path_csv : Full file path to input file

output_file_path_csv : Save location

_eg. DataExtractor.exe Data\\input.csv Data\\output.csv_

# Details

**StockProcessingEngine** is the entry point of the application.

```
StockProcessingEngine.Execute(string inputFile, string outputFile);

```

This class uses an instance of **StockProcessor** which is created by factory method, **StockProcessor.Create(...)**.

**StockProcessor** class takes in 3 dependencies - 

* **IReader** - Instance of this class is used to read from CSV file (Implemented by _CSVReader_ )
* **IWriter** - Instance of this class is used to write to output file (Implemented by _CSVWriter_ )
* **Func<input_type, output_type>** - Delegate that converts an input type to output type 



Core algorithm of the engine is as follows - 

1. Read from input file
2. Do the conversion from input type to output type
3. Write to output file


