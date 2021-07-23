using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRune : Consumable
{
    public float duration = 30;
    public Sprite sprite;
    public PlayerCombatScript combat;
    public ParticleSystem shield;

    protected override void Effect()
    {
        base.Effect();
        sprite = GetComponent<SpriteRenderer>().sprite;
        combat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatScript>();
        shield = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ParticleSystem>();
        StartCoroutine(EffectDur());
    }

    IEnumerator EffectDur()
    {
        float endTime = Time.time + duration;
        var thisBuff = new Buff(sprite, endTime, duration);
        BuffManager.activeBuffs.Add(thisBuff);
        combat.shielded = true;
        shield.enableEmission = true;

        while (Time.time < endTime)
        {
            if (!combat.shielded)
                endTime = Time.time;
            yield return null;
        }
        BuffManager.activeBuffs.Remove(thisBuff);
        shield.enableEmission = false;
    }
}
