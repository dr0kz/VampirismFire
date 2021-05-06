using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
    private static readonly string path = Application.persistentDataPath + "/myGame.fun";

    public static void SaveData(WorkerData workerData, List<ItemData> items, WallData wallData, TowerData towerData, ArcaneVaultData arcaneVault, int diamondsAmount)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Create);

        SavedData data = new SavedData(workerData, items, wallData, towerData, arcaneVault, diamondsAmount);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SavedData LoadData()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SavedData data = formatter.Deserialize(stream) as SavedData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
    public static void DeleteData()
    {
        File.Delete(path);
    }
}
