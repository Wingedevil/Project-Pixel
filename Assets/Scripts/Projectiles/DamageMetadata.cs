using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMetadata {
    public int Damage {
        get;
        private set;
    }
    
    public bool IsMagical {
        get;
        private set;
    }

    public bool IsPhysical {
        get;
        private set;
    }

    public bool IsHybrid {
        get => IsMagical && IsPhysical;
    }

    public DamageMetadata(int dmg, bool phy, bool mag) {
        Damage = dmg;
        IsPhysical = phy;
        IsMagical = mag;
    }
}
