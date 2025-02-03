using System.Collections.Generic;
using System.IO;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public List<Cell> cellPool;
}

[System.Serializable]
public class SaveDataWrapper
{
    public List<CellData> cells;
}

public class SaveManager : MonoBehaviour
{
    public PlayerController playerController;
    public Backpack backpack;
    private string _savePath;

    private void Awake()
    {
        _savePath = Application.persistentDataPath + "/saveData.json";
        LoadObjects();
    }

    public void SaveObjects()
    {
        List<CellData> saveData = new List<CellData>();

        foreach (var cell in backpack.cellPool)
        {
            CellData data = new CellData
            {
                itemName = cell.GetItemName(), // Получаем данные из Cell
                itemCount = cell.GetItemCount()
            };
            saveData.Add(data);
        }

        string json = JsonUtility.ToJson(new SaveDataWrapper { cells = saveData }, true);
        File.WriteAllText(_savePath, json);
        Debug.Log("Данные сохранены в: " + _savePath);
    }

    private void LoadObjects()
    {
        if (!File.Exists(_savePath))
        {
            Debug.LogWarning("Файл сохранения не найден!");
            return;
        }

        string json = File.ReadAllText(_savePath);
        SaveDataWrapper saveData = JsonUtility.FromJson<SaveDataWrapper>(json);

        foreach (CellData data in saveData.cells)
        {
            Cell newCell = backpack.CreateCell();
            newCell.ActiveItemInBackpack(data.itemCount, data.itemName);
            playerController.ChangeHaveGun(newCell.GetItemName(), false);
        }
        
        //backpack.CheckHaveGun();
        Debug.Log("Данные загружены!");
    }
}

/*public class SaveManager : MonoBehaviour
{
    public Backpack backpack;
    public List<Cell> _cellsToSave;
    private string _savePath;

    private void Awake()
    {
        _savePath = Application.persistentDataPath + "/saveData.json";

        LoadObjects();
    }

    public void SaveObjects(List<Cell> cellPool)
    {
        _cellsToSave = cellPool;
        SaveData saveData = new SaveData { cellPool = cellPool };
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_savePath, json);
        Debug.Log("Backpack count: " + cellPool.Count);
        Debug.Log("SaveData count: " + saveData.cellPool.Count);
        Debug.Log("Данные сохранены в: " + _savePath);
    }

    private void LoadObjects()
    {
        if (!File.Exists(_savePath))
        {
            Debug.LogWarning("Файл сохранения не найден!");
            return;
        }

        string json = File.ReadAllText(_savePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        backpack.cellPool = saveData.cellPool;
        Debug.Log("Backpack count: " + backpack.cellPool.Count);
        Debug.Log("SaveData count: " + saveData.cellPool.Count);
        Debug.Log("Данные загружены!");
    }

    public List<Cell> LoadObjects()
    {
        if (!File.Exists(_savePath))
        {
            Debug.LogWarning("Файл сохранения не найден!");
            return null;
        }

        string json = File.ReadAllText(_savePath);
        var saveManager = JsonUtility.FromJson<SaveManager>(json);
        Debug.Log("Данные загружены!");
        return saveManager._cellsToSave;
        //var backpack = FindObjectOfType<Backpack>();
        //backpack.cellPool = saveManager.cellsToSave;
    }
}*/
