using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachineItem
    {
        public string Name { get; }
        public decimal Price { get; }
        public string Slot { get; }
        public int QuantityRemaining { get; private set; }
        public string ConsumedMessage
        {
            get
            {
                if (Slot.Contains('A'))
                {
                    return "Crunch Crunch, Yum";
                }
                else if (Slot.Contains('B')) 
                {
                    return "Munch Munch, Yum";
                }
                else if (Slot.Contains('C'))
                {
                    return "Glug Glug, Yum";
                }
                else if (Slot.Contains('D'))
                {
                    return "Chew Chew, Yum";
                }
                return "unknown";
            }
        }

        public VendingMachineItem(string name, decimal price, string slot)
        {
            Name = name;
            Price = price;
            Slot = slot;
            QuantityRemaining = 5;
        }

        public bool PurchaseItem()
        {
            if (QuantityRemaining > 0)
            {
                QuantityRemaining--;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            if (QuantityRemaining < 1)
            {
                return $"{Slot}: SOLD OUT";
            }
            else
            {
                string slot = Slot + ":";
                string quantityRemaining = $"{QuantityRemaining} Remaining";
                return $"{slot.PadRight(5)} {Name.PadRight(20)} {quantityRemaining.PadRight(21)} {Price.ToString().PadRight(6)}";
            }
        }
    }
}

