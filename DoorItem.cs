using System;
using System.Collections.Generic;
using System.Linq;

namespace kirun9.scpsl.plugins.CoinBlocksDoors
{
    public class DoorItem : IEquatable<DoorItem>
    {
        public Door Door { get; set; }
        private int maxUses = 0;
        public int MaxUses
        {
            get
            {
                return maxUses;
            }
            set
            {
                maxUses = value;
            }
        }
        private int used = 0;
        public int Used
        {
            get
            {
                return used;
            }
            set
            {
                used = value;
            }
        }

        public DoorItem(Door door, int maxUses)
        {
            Door = door;
            Used = 0;
            MaxUses = maxUses;
        }

        public bool Equals(DoorItem obj)
        {
            return obj.Door.GetInstanceID() == Door.GetInstanceID();
        }
    }
}
