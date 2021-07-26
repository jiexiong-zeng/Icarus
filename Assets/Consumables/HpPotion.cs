using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : Consumable
{
    public PlayerCombatScript combat;
    protected override void Effect()
    {
        base.Effect();
        combat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatScript>();

        combat.health += 50;
        combat.healthBar.Set(combat.health / combat.maxHealth);
    }
}
