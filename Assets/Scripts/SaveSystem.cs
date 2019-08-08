using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    static string path;

    public static void SavePopulation(PlayerController playerController, ScoreManager scoreManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        path = Application.persistentDataPath + "/population.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(playerController, scoreManager);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Successfully saved to path: " + path);
    }

    public static GameData LoadPopulation()
    {
        path = Application.persistentDataPath + "/population.fun";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            Debug.Log("Successfully loaded data from path: " + path);

            return data;
        } else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
