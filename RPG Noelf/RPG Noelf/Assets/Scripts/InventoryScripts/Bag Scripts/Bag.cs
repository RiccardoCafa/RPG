﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace RPG_Noelf.Assets.Scripts.Inventory_Scripts
{
    public class BagEventArgs : EventArgs
    {
        public Bag Bag { get; set; }
    }

    /// <summary>
    /// Bag is the main Class to the entire Inventory System. 
    /// There are managment function to manipulate the bag of an entity.
    /// </summary>
    public class Bag
    {
        /// <summary>
        /// A event that is called everytime that a item has been removed or added from the bag, updating the interface.
        /// </summary>
        public delegate void BagEventHandler(object sender, BagEventArgs bag);
        public event BagEventHandler BagUpdated;

        /// <summary>
        /// Slot is a class that contains two atributes.
        /// <para>Contains the Item ID that will be use to search in Encyclopedia</para>
        /// <para>Contains the amount of items in this specific slot</para>
        /// </summary>
        public List<Slot> Slots;

        /// <value>Quantity of free slots in the bag.</value>
        public int FreeSlots { get; set; }
        /// <value>Number of max items that the bag can hold.</value>
        public int MaxSlots { get; set; }
        /// <value>Quantity of gold.</value>
        /// <summary>
        /// See <see cref="AddGold(int)"/> to increment gold
        /// </summary>
        public int Gold { get; set; }

        /// <value>Number of Max Stackable Items</value>
        public const uint MaxStack = 9999;

        /// <summary>
        /// Simple constructor of the class.
        /// Definition of MaxSlots setted here.
        /// </summary>
        public Bag()
        {
            Slots = new List<Slot>();
            MaxSlots = 30;
            FreeSlots = MaxSlots;
        }

        /// <summary>
        /// Verify if is possible to add more items.
        /// </summary>
        /// <returns>True if can be added more and false if not</returns>
        public bool CanAddMore()
        {
            return FreeSlots > 0;
        }

        /// <summary>
        /// Calculate how many items of this ID are in the bag.
        /// </summary>
        /// <param name="itemID">The Item ID to look for</param>
        /// <returns>The amount of items found</returns>
        public uint AmountInBag(uint itemID)
        {
            uint total = 0;
            var itensEncontrados = from item in Slots
                                   where item.ItemID == itemID
                                   select item;
            foreach (MobSlot ps in itensEncontrados)
            {
                total += ps.ItemAmount;
            }
            return total;
        }
        // (y) é tao legal mexer no pc dos outros kaopsdkoaskpdoakodkopaskop
        /// <summary>
        /// Increments a X amount of coins in the bag. With a maximium of 9.999.999
        /// </summary>
        /// <param name="coins">The amount of coins to be added</param>
        public virtual void AddGold(int coins)
        {
            if (Gold <= 9999999)
            {
                Gold += coins;
            }
        }

        /// <summary>
        /// This function add a item to the bag.
        /// It look for space and if there is some item that can be 'stacked' with space to do that.
        /// Then, if the item is stackable and it will overflow the max of items, then will calculate how many left and look for more space
        /// If there is more space, then it will recursivly add a new item in case the item overflow 2x the limite of max
        /// If is a non-stackable then will only add to the list if can be added
        /// See also <seealso cref="CanAddMore"/> 
        /// </summary>
        /// <param name="itemID">The item ID to be added</param>
        /// <param name="amount">The amount of that item</param>
        /// <returns></returns>
        public virtual bool AddToBag(Slot slot, ref Slot slotOffset)
        {
            Slot playerSlot = Slots.Find(x => x.ItemID == slot.ItemID && x.ItemAmount < MaxStack && Encyclopedia.SearchStackID(x.ItemID));
            
            if (playerSlot != null)
            {
                if (playerSlot.ItemAmount + slot.ItemAmount > MaxStack)
                {
                    uint offset = playerSlot.ItemAmount + slot.ItemAmount - MaxStack;
                    playerSlot.ItemAmount += offset;
                    if (Slots.Count < MaxSlots)
                    {
                        slot.ItemAmount -= offset;
                        FreeSlots--;
                        AddToBag(slot, ref slotOffset);
                        //OnBagUpdate();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    playerSlot.ItemAmount += slot.ItemAmount;
                    slot.ItemAmount = 0;
                    //OnBagUpdate();
                    return true;
                }
            }
            else if(CanAddMore())
            {
                Slots.Add(new Slot(slot.ItemID, slot.ItemAmount));
                slot = null;
                FreeSlots--;
                //OnBagUpdate();
                return true;
            } else
            return false;
        }

        /// <summary>
        /// This function add a item to the bag.
        /// It look for space and if there is some item that can be 'stacked' with space to do that.
        /// Then, if the item is stackable and it will overflow the max of items, then will calculate how many left and look for more space
        /// If there is more space, then it will recursivly add a new item in case the item overflow 2x the limite of max
        /// If is a non-stackable then will only add to the list if can be added
        /// See also <seealso cref="CanAddMore"/> 
        /// </summary>
        /// <param name="itemID">The item ID to be added</param>
        /// <param name="amount">The amount of that item</param>
        /// <returns></returns>
        public virtual bool AddToBag(Slot slot)
        {
            if (slot == null) return false;
            Slot playerSlot = Slots.Find(x => x.ItemID == slot.ItemID && x.ItemAmount < MaxStack && Encyclopedia.SearchStackID(x.ItemID));
            if (playerSlot != null)
            {
                if (playerSlot.ItemAmount + slot.ItemAmount > MaxStack)
                {
                    uint offset = playerSlot.ItemAmount + slot.ItemAmount - MaxStack;
                    playerSlot.ItemAmount = MaxStack;
                    slot.ItemAmount = offset;
                    if (Slots.Count < MaxSlots)
                    {
                        FreeSlots--;
                        AddToBag(slot);
                        //OnBagUpdate();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    playerSlot.ItemAmount += slot.ItemAmount;
                    slot.ItemAmount = 0;
                    //OnBagUpdate();
                    return true;
                }
            }
            else if (CanAddMore())
            {
                Slots.Add(new Slot(slot.ItemID, slot.ItemAmount));
                slot.ItemAmount = 0;
                //OnBagUpdate();
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Get a slot by his index
        /// </summary>
        /// <param name="index">The index to look for</param>
        /// <returns>Returns the slot of the specified index</returns>
        public Slot GetSlot(int index)
        {
            if (index < Slots.Count)
            {
                return Slots[index];
            }
            return null;
        }

        /// <summary>
        /// Get a slot by his ID
        /// </summary>
        /// <param name="itemID">The ID to look for</param>
        /// <returns>Returns the slot of the specified ID</returns>
        public Slot GetSlot(uint itemID)
        {
            return Slots.Find(x => x.ItemID == itemID);
        }

        /// <summary>
        /// Get the ID of a item in the index
        /// </summary>
        /// <param name="index">The index to look for</param>
        /// <returns>Returns the ID in the list at the index</returns>
        public uint GetSlotItemID(int index)
        {
            if (index < Slots.Count)
            {
                return Slots[index].ItemID;
            }
            return 0;
        }

        /// <summary>
        /// This only works with non-stackable items.
        /// </summary>
        /// <param name="s">The slot to be removed</param>
        /// <param name="amount">How many of them</param>
        private bool RemoveNonStackableItem(Slot s, uint amount)
        {
            var slotFound = from slot in Slots
                       where slot.ItemID == s.ItemID
                       select slot;
            if(slotFound != null)
            {
                if(slotFound.Count() >= amount)
                {
                    Slots.Remove(s);
                    FreeSlots--;
                    //OnBagUpdate();
                    if (amount > 1)
                    {
                        s = Slots.Find(x => x.ItemID == s.ItemID);
                        if (s != null) return RemoveNonStackableItem(s, --amount);
                    }
                    return true;
                } 
            }
            return false;
            
        }

        /// <summary>
        /// Remove from bag the specified amount of the item ID
        /// </summary>
        /// <param name="itemID">The item ID to look for and remove</param>
        /// <param name="amount">How many to be removed</param>
        public bool RemoveFromBag(uint itemID, uint amount)
        {
            Slot s = Slots.Find(x => x.ItemID == itemID);
            if(s != null)
            {
                if (s.ItemAmount < amount) return false;
                if(Encyclopedia.SearchStackID(s.ItemID))
                {
                    s.ItemAmount -= amount;
                    if(s.ItemAmount <= 0)
                    {
                        Slots.Remove(s);
                        FreeSlots++;
                        //OnBagUpdate();
                    }
                    return true;
                } else
                {
                    return RemoveNonStackableItem(s, amount);
                }
            }
            return false;
        }

        /// <summary>
        /// Remove from bag the specified amount of the index
        /// </summary>
        /// <param name="index">The index to look for and remove</param>
        /// <param name="amount">How many to be removed</param>
        public void RemoveFromBag(int index, uint amount)
        {
            Slot s = Slots.ElementAt(index);
            if (index < Slots.Count)
            {
                if(Encyclopedia.SearchStackID(s.ItemID))
                {
                    s.ItemAmount -= amount;
                    if(s.ItemAmount <= 0)
                    {
                        Slots.RemoveAt(index);
                        FreeSlots++;
                        //OnBagUpdate();
                    }
                } else
                {
                    RemoveNonStackableItem(s, amount);
                }
            }
        }

        /// <summary>
        /// Remove from bag the specified amount of the Slot
        /// </summary>
        /// <param name="slot">The slot to look for and remove</param>
        /// <param name="amount">How many to be removed</param>
        public void RemoveFromBag(Slot slot, uint amount)
        {
            if(Encyclopedia.SearchStackID(slot.ItemID))
            {
                slot.ItemAmount -= amount;
                if(slot.ItemAmount <= 0)
                {
                    Slots.Remove(slot);
                    FreeSlots++;
                    //OnBagUpdate();
                }
            } else
            {
                RemoveNonStackableItem(slot, amount);
            }
        }

        /// <summary>
        /// Remove from bag one unit of the Slot
        /// DEPRECEATED NOT USE YET PLEASE
        /// </summary>
        /// <param name="itemID">The slot to look for and remove</param>
        public void RemoveFromBag(Slot slot)
        {
            if (Encyclopedia.SearchStackID(slot.ItemID))
            {
                if (-- slot.ItemAmount <= 0)
                {
                    Slots.Remove(slot);
                    FreeSlots++;
                    //OnBagUpdate();
                }
            }
            else
            {
                RemoveNonStackableItem(slot, 1);
            }
        }
        
        public void IncreaseSizeOfBag(int size)
        {

            if (size > 0)
            {
                FreeSlots = FreeSlots + size;
            }
        }

        public virtual void OnBagUpdated()
        {
            BagUpdated?.Invoke(this, new BagEventArgs() { Bag = this });
        }
    }
}
