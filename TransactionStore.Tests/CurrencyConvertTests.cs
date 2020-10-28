using NUnit.Framework;
using System;
using TransactionStore.Repository;

namespace TransactionStore.Tests
{
    [TestFixture]
    public class Tests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            CurrencyRates.ActualCurrencyRates = CurrencyConvertMock.ActualCurrencyRates;
        }

        [TestCase(73162, 2, 1, 1000)] // RUB to USD
        [TestCase(924, 3, 1, 1000)] // EUR to USD
        [TestCase(107149, 4, 1, 1000)] // JPY to USD
        [TestCase(1000, 1, 2, 73162)] // USD to RUB
        [TestCase(1000, 3, 2, 79179.6537)] // EUR to RUB
        [TestCase(1000, 4, 2, 682.8062)] // JPY to RUB
        [TestCase(1000, 1, 3, 924)] // USD to EUR
        [TestCase(1000, 2, 3, 12.6295)] // RUB to EUR
        [TestCase(1000, 4, 3, 8.6235)] // JPY to EUR 
        [TestCase(1000, 1, 4, 107149)] // USD to JPY
        [TestCase(1000, 2, 4, 1464.5444)] // RUB to JPY
        [TestCase(1000, 3, 4, 115962.1212)] // USD to JPY 
        public void ConvertCurrencyTest(decimal amount, int initialCurrencyId, int targetCurrencyId, decimal expected)
        {
            var actual = CurrencyConverter.ConvertCurrency(amount, initialCurrencyId, targetCurrencyId);
            Assert.AreEqual(Math.Round(actual,4), expected);
        }
    }
}