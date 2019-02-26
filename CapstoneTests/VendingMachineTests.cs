using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes.Tests
{
    [TestClass]
    public class VendingMachineTests
    {
        private VendingMachine test = new VendingMachine();

        [TestInitialize]
        public void TestInitialize()
        {
            test.LoadInventory();
        }

        [TestMethod]
        public void Constructor_Tests()
        {
            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void Test_First_Results_of_To_String()
        {
            string[] outputLines = test.ToString().Split("\n");
            Assert.AreEqual("A1:   Potato Crisps        5 Remaining           3.05  ", outputLines[0]);
            Assert.AreEqual("B1:   Moonpie              5 Remaining           1.80  ", outputLines[4]);
        }

        [TestMethod]
        public void Test_Feed_Money()
        {
            Assert.AreNotEqual(1, test.CurrentMoney);
            test.FeedMoney(1);
            Assert.AreEqual(1, test.CurrentMoney);
            test.FeedMoney(5);
            Assert.AreEqual(6, test.CurrentMoney);
        }

        [TestMethod]
        public void DispenseChangeTest_1_Dollar_Should_Be_4_Quarters()
        {
            test.FeedMoney(1);
            Assert.AreEqual("Your change is: 4 quarters, 0 dimes, 0 nickels", test.DispenseChange());
        }

        [TestMethod]
        public void DispenseChangeTest_15_Cents_Should_Be_1_Dime_And_1_Nickel()
        {
            test.FeedMoney(1);
            test.DispenseProduct("D1");
            Assert.AreEqual("Your change is: 0 quarters, 1 dimes, 1 nickels", test.DispenseChange());
        }

        [TestMethod]
        public void DispenseChangeTest_1_Dollar_15_Cents_Should_Be_1_Dime_And_1_Nickel()
        {
            test.FeedMoney(1);
            test.FeedMoney(1);
            test.DispenseProduct("D1");
            Assert.AreEqual("Your change is: 4 quarters, 1 dimes, 1 nickels", test.DispenseChange());
        }

        [TestMethod]
        public void Test_Log_File_Is_Growing_After_Each_Log()
        {
            string[] startingLines = File.ReadAllLines(@"C:\VendingMachine\Log.txt");
            int startingLinesCount = startingLines.Length;

            test.WriteToLog("Test Log Item");

            string[] newLines = File.ReadAllLines(@"C:\VendingMachine\Log.txt");
            int newLinesCount = newLines.Length;

            Assert.AreNotEqual(startingLinesCount, newLinesCount);
        }

        [TestMethod]
        public void DispenseProduct_Tests_Should_Return_Not_Exists()
        {
            Assert.AreEqual("Product code does not exist", test.DispenseProduct("A0"));
            Assert.AreEqual("Product code does not exist", test.DispenseProduct("A9"));
        }

        [TestMethod]
        public void DispenseProduct_Tests_Should_Return_Sold_Out()
        {
            test.FeedMoney(20);
            Assert.AreNotEqual("SOLD OUT", test.DispenseProduct("A1"));
            test.DispenseProduct("A1");
            test.DispenseProduct("A1");
            test.DispenseProduct("A1");
            test.DispenseProduct("A1");
            test.DispenseProduct("A1");
            Assert.AreEqual("SOLD OUT", test.DispenseProduct("A1"));
        }

        [TestMethod]
        public void DispenseProduct_Tests_Should_Return_Not_Enough_Money()
        {
            Assert.AreEqual("Please enter more money to purchase this item.", test.DispenseProduct("A1"));
            Assert.AreEqual("Please enter more money to purchase this item.", test.DispenseProduct("A2"));
        }

        [TestMethod]
        public void DispenseProduct_Tests_Should_Return_Dispense_Message()
        {
            test.FeedMoney(20);
            Assert.AreEqual("Crunch Crunch, Yum", test.DispenseProduct("A1"));
            Assert.AreEqual("Munch Munch, Yum", test.DispenseProduct("b1"));
            Assert.AreEqual("Glug Glug, Yum", test.DispenseProduct("c1"));
            Assert.AreEqual("Chew Chew, Yum", test.DispenseProduct("d1"));
        }

        [TestMethod]
        public void DispenseProduct_Tests_Should_Add_New_Line_To_Log_File()
        {

            string[] startingLines = File.ReadAllLines(@"C:\VendingMachine\Log.txt");
            int startingLinesCount = startingLines.Length;

            test.FeedMoney(20);
            test.DispenseProduct("A1");

            string[] newLines = File.ReadAllLines(@"C:\VendingMachine\Log.txt");
            int newLinesCount = newLines.Length;

            Assert.AreNotEqual(startingLinesCount, newLinesCount);
        }
    }
}
