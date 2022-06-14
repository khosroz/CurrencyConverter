using CurrencyConverter.Converter;

namespace UnitTest
{
    public class CovnerterUnitTest
    {
        private ICurrencyConverter _sut;
        public CovnerterUnitTest()
        {
            _sut = new CurrencyConverterImpl();
        }

        [Fact]
        public void Convert_USD_To_CAD()
        {
            var result=_sut.Convert("USD", "CAD", 2000);
            Assert.NotEqual(0, result);
            Assert.Equal(2680 , result);
        }        
        [Fact]
        public void Convert_CAD_To_USD()
        {
            var result=_sut.Convert("CAD", "USD", 2000);
            Assert.NotEqual(0, result);
            Assert.Equal(1492.54, result);
        }
        [Fact]
        public void Convert_USD_GBP()
        {
            var result=_sut.Convert("USD", "GBP", 2000);
            Assert.NotEqual(0, result);
            Assert.Equal(1554.4, result);
        }  
        [Fact]
        public void Convert_EUR_GBP()
        {
            var result=_sut.Convert("EUR", "GBP", 2000);
            Assert.NotEqual(0, result);
            Assert.Equal(1807.44, result);
        }
        [Fact]
        public void UpdateRateOfCurrency()
        {
            var result = _sut.Convert("USD", "CAD", 2000);
            Assert.NotEqual(0, result);
            Assert.Equal(2680, result);

            var newRate = Tuple.Create("USD","CAD",1.25);
            var newRates= new List<Tuple<string, string, double>>();
            newRates.Add(newRate);
            _sut.UpdateConfiguration(newRates);

            result = _sut.Convert("USD", "CAD", 2000);
            Assert.NotEqual(0, result);
            Assert.Equal(2500, result);

        }
    }
}