using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{
    //Class for one interaction between two chars 
    public Sentence[] dialogues;
    //Who other than player is participating
    public string NPCname;
}

[System.Serializable]
public class Sentence
{
    public string Text;
    public bool IsPlayerSpeaking;
}