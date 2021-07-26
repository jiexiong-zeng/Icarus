using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public static List<ObeliskData> savedObelisks = new List<ObeliskData>();

    //it's static so can call it without any instance
    public static void SaveObelisk(ObeliskData info)
    {
        //Add new obelisk to list of fast travel points
        savedObelisks.Add(info);
        Debug.Log(savedObelisks.Count);
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log(Application.persistentDataPath);
        FileStream file = File.Create(Application.persistentDataPath + "/travelpoints.info"); 
        bf.Serialize(file, savedObelisks);
        file.Close();
    }

    public static void LoadObelisks()
    {
        if (File.Exists(Application.persistentDataPath + "/travelpoints.info"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/travelpoints.info", FileMode.Open);
            savedObelisks = (List<ObeliskData>)bf.Deserialize(file);
            file.Close();
        }
    }
}


[System.Serializable]
public class ObeliskData
{
    public string scene;
    public float[] position = new float[3];
    public int num;
    public string pathToImg;


    public ObeliskData(string sceneName, Vector3 pos, int number)
    {
        scene = sceneName;
        position[0] = pos.x;
        position[1] = pos.y;
        position[2] = pos.z;
        num = number;
    }
}