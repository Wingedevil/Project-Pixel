using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Spellbook {
    public static Dictionary<string, Tuple<string, Spell>> Spells = new Dictionary<string, Tuple<string, Spell>> {
        { "Arcane Bolt", new Tuple<string, Spell>("OnhandSpell", new ArcaneBolt()) }
    };

    public static Spell GetSpell(string name) {
        Spells.TryGetValue(name, out Tuple<string, Spell> rtv);
        return rtv.Item2;
    }

    public static string GetType(string name) {
        Spells.TryGetValue(name, out Tuple<string, Spell> rtv);
        return rtv.Item1;
    }
}