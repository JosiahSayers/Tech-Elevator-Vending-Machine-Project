using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class UserInterface
    {
        private VendingMachine vendingMachine = new VendingMachine();

        public void RunInterface()
        {

            try
            {
                vendingMachine.LoadInventory();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Unable to read inventory file.\n{e.Message}\nPress enter to quit.");
                Console.ReadLine();
                Environment.Exit(-1);
            }
            bool done = false;
            while (!done)
            {
                int choice = 0;
                Console.Clear();
                Console.WriteLine("Welcome to Vendo-Matic 7000!\n");
                Console.WriteLine("(1) Display Vending Machine Items");
                Console.WriteLine("(2) Purchase");
                Console.WriteLine("(3) End");
                Console.Write("\nPlease choose an option: ");

                string userInput = Console.ReadLine();
                choice = IsValidInt(userInput);

                if(choice == 1)
                {
                    PrintAllItems();
                    Console.WriteLine("\nPress enter to return to the Main Menu");
                    Console.ReadLine();
                }
                else if(choice == 2)
                {
                    PurchaseMenu();
                }
                else if(choice == 3)
                {
                    done = true;
                }
                else if (choice == 9)
                {
                    try
                    {
                        vendingMachine.SalesReport();
                        Console.WriteLine("Your report has been created.");
                        Console.ReadLine();
                    }
                    catch
                    {
                        Console.WriteLine("Error writing Sales Report to disk.\nPress enter to return to the main menu.");
                        Console.ReadLine();
                    }
                }
            }

        }

        void PurchaseMenu()
        {
            bool done = false;
            int choice = 0;
            while(!done)
            {
                Console.Clear();
                Console.WriteLine("Purchase Menu\n");
                Console.WriteLine("(1) Feed Money");
                Console.WriteLine("(2) Select Product");
                Console.WriteLine("(3) Finish Transaction");
                Console.WriteLine($"\nCurrent Money Provided: {(vendingMachine.CurrentMoney).ToString("c2")}");
                Console.Write("\nPlease choose an option: ");

                string userInput = Console.ReadLine();
                choice = IsValidInt(userInput);

                if (choice == 1)
                {
                    FeedMoney();
                }
                else if (choice == 2)
                {
                    SelectProductMenu();
                }
                else if (choice == 3)
                {
                    FinishTransaction();
                    Console.WriteLine("\nPress enter to go back to the previous menu.");
                    Console.ReadLine();
                    done = true;
                }

            }
        }

        void FeedMoney()
        {
            Console.Clear();
            Console.Write("Enter bill amount (1, 2, 5, 10, 20): ");
            string userInput = Console.ReadLine();
            int money = IsValidInt(userInput);

            if(money == 1 || money == 2 || money == 5 || money == 10 || money == 20)
            {
                vendingMachine.FeedMoney(money);
            }
            else
            {
                Console.WriteLine("That doesn't appear to be a valid bill.\nPress enter to return to the Purchase Menu");
                Console.ReadLine();
            }
        }

        void SelectProductMenu()
        {
            PrintAllItems();
            Console.WriteLine("\nEnter the item you would like to purchase (ex. A1): ");
            string userInput = Console.ReadLine();
            Console.WriteLine(vendingMachine.DispenseProduct(userInput));
            Console.WriteLine("Press enter to return to the Purchase Menu.");
            Console.ReadLine();
        }

        void FinishTransaction()
        {
            Console.WriteLine(vendingMachine.DispenseChange());
        }

        void PrintAllItems()
        {
            Console.Clear();
            string slot = "Slot";
            string itemName = "Item Name";
            string quantityRemaining = "Quantity Remaining";
            string price = "Price";
            Console.WriteLine("{0,-6}{1,-21}{2,-22}{3,-5}",slot,itemName,quantityRemaining,price);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine(vendingMachine.ToString());
        }

        int IsValidInt(string userInput)
        {
            int choice = 0;
            if(userInput.Contains('.'))
            {
                userInput = userInput.Remove(userInput.IndexOf('.'));
            }
            try
            {
                choice = int.Parse(userInput);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Invalid input\n{e.Message}");
                Console.WriteLine("Press enter to return to the previous menu");
                Console.ReadLine();
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Invalid input\n{e.Message}");
                Console.WriteLine("Press enter to return to the previous menu");
                Console.ReadLine();
            }
            catch (OverflowException e)
            {
                Console.WriteLine($"Invalid input\n{e.Message}");
                Console.WriteLine("Press enter to return to the previous menu");
                Console.ReadLine();
            }
            return choice;
        }
    }
}
