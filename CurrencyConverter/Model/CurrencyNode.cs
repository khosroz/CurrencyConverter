using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Model
{
    internal class CurrencyNode
    {
        private readonly Dictionary<string, double> currencies = new();

        public string Title { get; private set; }

        public CurrencyNode(string title, string neighbor, double rate)
        {
            Title = title;
            currencies.TryAdd(neighbor, rate);
        }
        public Task AddNeighber(string currency, double rate)
        {
            if (currencies.ContainsKey(currency))
                currencies[currency] = rate;
            else
                currencies.Add(currency, rate);
            return Task.FromResult(currencies.Count);
        }
        public Task RemoveNeighber(string currency)
        {
            currencies.Remove(currency);
            return Task.FromResult(currencies.Count);
        }

        internal (bool, double) ThereIsDirctRate(string toCurrency)
        {
            if (currencies.TryGetValue(toCurrency, out double rate))
            {
                return (true, rate);
            }
            return (false, 0);
        }

        internal Dictionary<string, double> GetNeighborCurrencies()
        {
            return currencies;
        }
    }
}
