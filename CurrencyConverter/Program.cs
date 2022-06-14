// See https://aka.ms/new-console-template for more information

using CurrencyConverter.Converter;

Console.Clear();
Console.WriteLine("Welcom to conrrency converter");
Console.WriteLine("please tell me what currency you have to convert?");
var fromCurrency=Console.ReadLine();
double amount = 0;
do
{
    Console.WriteLine("and how much?");
} while (!Double.TryParse(Console.ReadLine(), out amount));
Console.WriteLine("which currency do you need?");
var toCurrency = Console.ReadLine();

ICurrencyConverter currencyConverter = new CurrencyConverterImpl();
var result=currencyConverter.Convert(fromCurrency?.ToUpper(), toCurrency?.ToUpper(),Convert.ToDouble(amount));
Console.WriteLine($"result is {result}");
Console.ReadKey();
