using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

public class Backpack : MonoBehaviour
{
    [Header("Manager")]
    public SaveManager saveManager;
    
    [Header("Controller")]
    public PlayerController playerController;
    
    [Space(10)]
    [Header("Cells")]
    public List<Cell> cellPool;
    public Cell cellPrefab;
    
    private int _countCells;

    private void Start()
    {
        //saveManager = FindObjectOfType<SaveManager>();

        //cellPool = saveManager.LoadObjects();
    }
    
    public Cell CreateCell()
    {
        var cellObj = Instantiate(cellPrefab, transform); // Создаём новый объект
        cellPool.Add(cellObj);
        return cellObj.GetComponent<Cell>(); // Возвращаем ссылку на Cell
    }

    public void CheckHaveGun()
    {
        Debug.Log("Checking havegun");
        foreach (var cell in cellPool)
        {
            Debug.Log(cell.GetItemName());
            playerController.ChangeHaveGun(cell.GetItemName());
        }
    }

    public void IncreaseCountCells(Cell cell)
    {
        _countCells++;
        
        cellPool.Add(cell);
        //saveManager.SaveObjects(cellPool);
    }
    
    public IEnumerator DecreaseCountCells()
    {
        _countCells--;
        
        yield return new WaitForSeconds(0.1f);

        foreach (var cell in cellPool.ToList())
        {
            if (cell == null)
            {
                cellPool.Remove(cell);
            }
        }
        //saveManager.SaveObjects(cellPool);
    }

    public bool CheckIsFullBackpack()
    {
        return _countCells == 24;
    }
}