using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : Consumable
{
    public Afterimage image;
    public PlayerMovement_MysteryKnight move;
    public float duration = 10;
    public Sprite sprite;

    protected override void Effect()
    {
        base.Effect();
        image = GameObject.FindGameObjectWithTag("Player").GetComponent<Afterimage>();
        move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_MysteryKnight>();
        sprite = GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(EffectDur());
    }

    IEnumerator EffectDur()
    {
        float endTime = Time.time + duration;
        var run = move.runSpeed;
        image.lifetime = 0.5f;
        move.runSpeed = run * 1.5f;
        var thisBuff = new Buff(sprite, endTime, duration);
        BuffManager.activeBuffs.Add(thisBuff);
        yield return new WaitForSeconds(duration);
        BuffManager.activeBuffs.Remove(thisBuff);
        move.runSpeed = run;
        image.lifetime = 0f;
    }
}
