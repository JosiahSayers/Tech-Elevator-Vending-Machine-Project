using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        public decimal CurrentMoney { get; private set; }

        private List<VendingMachineItem> items = new List<VendingMachineItem>();
        private static string filePath = @"C:\VendingMachine";
        private string inventory = Path.Combine(filePath, "vendingmachine.csv");
        private string logFile = Path.Combine(filePath, "Log.txt");

        public string LoadInventory()
        {
            string[] itemInfo = new string[3];
            using (StreamReader sr = new StreamReader(inventory))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    itemInfo = line.Split('|');
                    items.Add(new VendingMachineItem(itemInfo[1], Decimal.Parse(itemInfo[2]), itemInfo[0]));
                }
            }
            return inventory;
        }


        public override string ToString()
        {
            string output = "";
            foreach (VendingMachineItem item in items)
            {
                output += item.ToString() + "\n";
            }
            return output;
        }

        public string DispenseChange()
        {
            int quarters = 0;
            int dimes = 0;
            int nickels = 0;

            while (CurrentMoney > 0)
            {
                if (CurrentMoney - 0.25M >= 0)
                {
                    CurrentMoney -= 0.25M;
                    quarters++;
                }
                else if (CurrentMoney - 0.10M >= 0)
                {
                    CurrentMoney -= 0.10M;
                    dimes++;
                }
                else if (CurrentMoney - 0.05M >= 0)
                {
                    CurrentMoney -= 0.05M;
                    nickels++;
                }
            }

            return $"Your change is: {quarters} quarters, {dimes} dimes, {nickels} nickels";
        }

        public void FeedMoney(int money)
        {
            CurrentMoney += money;
            string logLine = $"FEED MONEY: {money.ToString("c2")}    {CurrentMoney.ToString("c2")}";
            WriteToLog(logLine);
        }

        public bool WriteToLog(string logLine)
        {
            DateTime dateTime = DateTime.Now;
            string output = dateTime + " " + logLine;

            try
            {
                using (StreamWriter sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(output);
                    return true;
                }
            }
            catch (IOException e)
            {
                return false;
            }
        }

        public string DispenseProduct(string itemSlot)
        {
            string output = "Product code does not exist";
            itemSlot = itemSlot.ToUpper();

            for (int i = 0; i < items.Count; i++)
            {
                if (itemSlot == items[i].Slot.ToUpper())
                {
                    if (CurrentMoney >= items[i].Price)
                    {
                        if (items[i].PurchaseItem())
                        {
                            string log = $"{items[i].Name} {items[i].Slot}  {CurrentMoney.ToString("c2")}     {(CurrentMoney - items[i].Price).ToString("c2")}";
                            WriteToLog(log);
                            CurrentMoney -= items[i].Price;
                            output = items[i].ConsumedMessage;
                        }
                        else
                        {
                            output = "SOLD OUT";
                        }
                    }
                    else
                    {
                        output = "Please enter more money to purchase this item.";
                    }
                }
            }
            return output;
        }

        public string SalesReport()
        {
            DateTime dateTime = DateTime.Now;
            string otherDateTime = dateTime.ToString();
            otherDateTime = otherDateTime.Replace('/', '-');
            otherDateTime = otherDateTime.Replace(':', '-');

            string output = @"SalesReport-" + otherDateTime + @".txt";

            string salesReport = Path.Combine(filePath, output);
            try
            {
                using (StreamWriter sw = new StreamWriter(salesReport, false))
                {
                    decimal totalSales = 0;
                    foreach (VendingMachineItem item in items)
                    {
                        int totalSold = (item.QuantityRemaining - 5) * -1;
                        totalSales += totalSold * item.Price;
                        sw.WriteLine($"{item.Name}|{totalSold}");
                    }
                    sw.WriteLine();
                    sw.Write($"**TOTAL SALES** {totalSales}");
                }
            }
            catch (Exception e)
            {

            }
            return "";
        }
    }
}
