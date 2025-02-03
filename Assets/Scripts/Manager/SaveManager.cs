using System.Collections.Generic;
using System.IO;
using Player;
using UnityEngine;

[System.Serializable]
public class SaveDataWrapper
{
    public List<CellData> cells;
}

public class SaveManager : MonoBehaviour
{
    [Header("Controller")]
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
                itemName = cell.GetItemName(),
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
        
        Debug.Log("Данные загружены!");
    }
}