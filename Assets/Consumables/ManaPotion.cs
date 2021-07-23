using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : Consumable
{
    public PlayerCombatScript combat;

    protected override void Effect()
    {
        base.Effect();
        combat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatScript>();

        combat.ChangeMana(50);
        //combat.manaBar.Gain(combat.mana / combat.maxMana);
    }
}
