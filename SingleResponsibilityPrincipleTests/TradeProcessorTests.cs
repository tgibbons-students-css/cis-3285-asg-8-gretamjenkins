using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleResponsibilityPrinciple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SqlClient;

namespace SingleResponsibilityPrinciple.Tests
{
    [TestClass()]
    public class TradeProcessorTests
    {
        private int CountDbRecords()
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\greta\OneDrive\Senior Year\Software Design\Unit 8\Participation\tradedatabase.mdf"";Integrated Security=True;Connect Timeout=30;"))
            {
                connection.Open();
                string myScalarQuery = "SELECT COUNT(*) FROM trade";
                SqlCommand myCommand = new SqlCommand(myScalarQuery, connection);
                myCommand.Connection.Open();
                int count = (int)myCommand.ExecuteScalar();
                connection.Close();
                return count;
            }
        }

        [TestMethod()]
        public void ProcessTradesTest()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.goodtrades.txt");
            var tradeProcessor = new TradeProcessor();

            // Act
            tradeProcessor.ProcessTrades(tradeStream);

            // Assert
            int count = CountDbRecords();
            Assert.AreEqual(count, 4);
        }

        // Process trades test 1
        // Process trades test 2

        // ReadTradeData trades test 1
        // ReadTradeData trades test 2

        // ParseTrades trades test 1
        // ParseTrades trades test 2

        // StoreTrades trades test 1
        // StoreTrades trades test 2

    }
}