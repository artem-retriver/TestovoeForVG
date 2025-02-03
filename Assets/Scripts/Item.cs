using Player;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Properties")]
    public Cell cellPrefab;
    public int countItems;
    public string itemName;
    
    private UIController _uiController;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _uiController = FindObjectOfType<UIController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && _uiController.backpack.CheckIsFullBackpack() == false)
        {
            var newCell = Instantiate(cellPrefab, _uiController.backpack.gameObject.transform);
            newCell.ActiveItemInBackpack(countItems, itemName);

            var player = other.gameObject.GetComponent<PlayerController>();
            player.ChangeHaveGun(itemName);
            player.CheckHaveAllGun();
            _uiController.backpack.IncreaseCountCells(newCell);
            Destroy(gameObject);
        }
    }

    public void ChangeActiveRigidbody(bool active)
    {
        _rigidbody.isKinematic = active;   
    }
}