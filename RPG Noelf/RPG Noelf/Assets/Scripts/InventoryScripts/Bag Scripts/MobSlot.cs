﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Noelf.Assets.Scripts.InventoryScripts
{
    class MobSlot : Slot
    {
        public Random dr0p = new Random();
        public double chanceDrop { get; set; }
        
        
        public MobSlot(uint IDkey, uint amount, double chanceDrop) : base(IDkey, amount)
        {
            this.chanceDrop = chanceDrop;
        }

        public bool Dropou()
        {
           if(dr0p.NextDouble() <= chanceDrop)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}