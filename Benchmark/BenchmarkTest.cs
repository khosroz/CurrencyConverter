using BenchmarkDotNet.Attributes;
using CurrencyConverter.Converter;

public class BenchmarkTest
{
    private ICurrencyConverter sut;
    public BenchmarkTest()
    {
        sut = new CurrencyConverterImpl();
    }
    [Benchmark]
    public double ConvertUSDtoGBP() => sut.Convert("USD", "GBP", 5000);
}