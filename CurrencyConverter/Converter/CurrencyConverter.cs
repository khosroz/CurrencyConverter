using CurrencyConverter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Converter
{
    public class CurrencyConverterImpl : ICurrencyConverter
    {
        private Dictionary<string, CurrencyNode> currencies = new();
        private readonly object _lockCurrencies = new();
        public CurrencyConverterImpl()
        {
            Init();
        }
        private void Init()
        {
            var defaultList = new List<Tuple<string, string, double>>();
            defaultList.Add(Tuple.Create("USD", "CAD", 1.34));
            defaultList.Add(Tuple.Create("CAD", "GBP", 0.58));
            defaultList.Add(Tuple.Create("USD", "EUR", 0.86));
            AddOrUpdateCurrencyConfiguration(defaultList);
        }
        public void ClearConfiguration()
        {
            currencies.Clear();
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            if (!currencies.TryGetValue(fromCurrency, out CurrencyNode currencyNode))
            {
                throw new Exception($"We dont have {fromCurrency} currency");
            }
            var rate = Calculate(currencyNode, toCurrency, amount);
            return rate;
        } 

      
        private double Calculate(CurrencyNode fromCurrency, string toCurrency, double amount)
        {
            double res = 0;
            var (thereIs, rate) = fromCurrency.ThereIsDirctRate(toCurrency);
            if (thereIs)
            {
                res = rate * amount;
            }
            else
            {
                var neighbors = fromCurrency.GetNeighborCurrencies();
                foreach (var neighbor in neighbors)
                {
                    CurrencyNode nextNode = GetCurrencyNode(neighbor.Key);
                    res = Calculate(nextNode, toCurrency, amount * neighbor.Value);
                    if (res > 0)
                        break;
                }
            }
            return Math.Round(res, 2);
        }
        private double CalculateShortestPath(Dictionary<string, double> fromCurrencies, string toCurrency, double amount)
        {
            double res = 0;
            Dictionary<string, double> nextNodeNeedToCheck = new();
            foreach (var node in fromCurrencies)
            {
                CurrencyNode nextNode = GetCurrencyNode(node.Key);
                var (thereIs, rate) = nextNode.ThereIsDirctRate(toCurrency);
                if (thereIs)
                {
                    res = rate * node.Value;
                }
                else
                {
                    var neighbors = nextNode.GetNeighborCurrencies();
                    foreach (var item in neighbors)
                    {
                        nextNodeNeedToCheck.Add(item.Key,item.Value*amount);
                    }                   
                }
            }
            if (res == 0)
            {                
                    CalculateShortestPath(nextNodeNeedToCheck, toCurrency, amount);                
            }
            return Math.Round(res, 2);
        }

        private CurrencyNode? GetCurrencyNode(string currencyTitle)
        {
            if (currencies.TryGetValue(currencyTitle, out CurrencyNode currencyNode))
            {
                return currencyNode;
            }
            return null;
        }

        private void AddOrUpdateCurrencyConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            if (conversionRates is not null)
            {
                foreach (var item in conversionRates)
                {
                    AddOrUpdateConfig(item.Item1, item.Item2, item.Item3);
                    AddOrUpdateConfig(item.Item2, item.Item1, 1 / item.Item3);
                }
            }
        }

        private void AddOrUpdateConfig(string fromCurrency, string toCurrency, double rate)
        {
            lock (_lockCurrencies)
            {
                var currency = GetCurrencyNode(fromCurrency);
                if (currency is null)
                {
                    currencies.Add(fromCurrency, new CurrencyNode(fromCurrency, toCurrency, rate));
                }
                else
                {
                    currency.AddNeighber(toCurrency, rate);
                    currencies[fromCurrency] = currency;
                }
            }
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            AddOrUpdateCurrencyConfiguration(conversionRates);
        }
    }

}
