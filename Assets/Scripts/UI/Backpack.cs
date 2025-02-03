using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [Header("Cells")]
    public List<Cell> cellPool;
    public Cell cellPrefab;
    
    private int _countCells;
    
    public Cell CreateCell()
    {
        var cellObj = Instantiate(cellPrefab, transform);
        cellPool.Add(cellObj);
        return cellObj.GetComponent<Cell>();
    }

    public void IncreaseCountCells(Cell cell)
    {
        _countCells++;
        
        cellPool.Add(cell);
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
    }

    public bool CheckIsFullBackpack()
    {
        return _countCells == 24;
    }
}