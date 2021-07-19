using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : Consumable
{
    public Afterimage image;
    public PlayerMovement_MysteryKnight move;
    public float duration = 10;

    protected override void Effect()
    {
        base.Effect();
        image = GameObject.FindGameObjectWithTag("Player").GetComponent<Afterimage>();
        move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_MysteryKnight>();
        StartCoroutine(EffectDur());
        //var run = move.runSpeed;
        //image.lifetime = 0.5f;
        //move.runSpeed = run * 1.5f;
    }

    IEnumerator EffectDur()
    {
        var run = move.runSpeed;
        image.lifetime = 0.5f;
        move.runSpeed = run * 1.5f;
        yield return new WaitForSeconds(duration);
        move.runSpeed = run;
        image.lifetime = 0f;
    }
}
