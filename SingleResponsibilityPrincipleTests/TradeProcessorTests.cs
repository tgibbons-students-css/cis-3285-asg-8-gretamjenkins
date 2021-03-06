﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleResponsibilityPrinciple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;

namespace SingleResponsibilityPrinciple.Tests
{
    [TestClass()]
    public class TradeProcessorTests
    {
        private int CountDbRecords()
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\greta\OneDrive\Senior Year\Software Design\Unit 8\Participation\tradedatabase.mdf"";Integrated Security=True;Connect Timeout=30;"))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string myScalarQuery = "SELECT COUNT(*) FROM trade";
                SqlCommand myCommand = new SqlCommand(myScalarQuery, connection);
                int count = (int)myCommand.ExecuteScalar();
                connection.Close();
                return count;
            }
        }

        [TestMethod()]
        public void TestNormalFile()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.goodtrades.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore + 4, countAfter);
        }

        // Negative trade amounts should not be valid
        [TestMethod()]
        public void TestNegativeTradeAmount()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.negativeTradeAmount.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }

        // Trade amounts must be between 500 and 15000
        [TestMethod()]
        public void TestAmountTradesInterval()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.invalidTradeAmounts.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }
        
        // Trades with dollar signs should still be valid
        [TestMethod()]
        public void TestPriceWithDollarSign()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.priceDollarSign.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore + 1, countAfter);
        }

        // Decimal trade amounts should still be valid and should be rounded to the nearest integer
        [TestMethod()]
        public void TestDecimalTradeAmount()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.demicalTradeAmount.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore + 1, countAfter);
        }

    }
}
