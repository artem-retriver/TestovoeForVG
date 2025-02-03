using DG.Tweening;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class CellData
{
    public string itemName;
    public int itemCount;
}

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item UI")]
    public Image healthIcon;
    public Image ammoIcon;
    public Image machineGunIcon;
    public Image heavyMachineGunIcon;
    public Image sniperGunIcon;
    public Text cellText;

    private PlayerController _playerController;
    private Backpack _backpack;
    private Button _cellButton;
    
    private string _nameItem;
    private int _countItem;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _backpack = GetComponentInParent<Backpack>();
        _cellButton = gameObject.GetComponent<Button>();
        _cellButton.onClick.AddListener(CheckUseCurrentItem);
        
        //_playerController.ChangeHaveGun(_nameItem);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.2f, 0.2f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.2f);
    }

    public void ActiveItemInBackpack(int count, string itemName)
    {
        switch (itemName)
        {
            case "Health":
            {
                healthIcon.gameObject.SetActive(true);
                break;
            }
            case "Ammo":
            {
                ammoIcon.gameObject.SetActive(true);
                break;
            }
            case "MachineGun":
            {
                machineGunIcon.gameObject.SetActive(true);
                break;
            }
            case "HeavyMachineGun":
            {
                heavyMachineGunIcon.gameObject.SetActive(true);
                break;
            }
            case "SniperGun":
            {
                sniperGunIcon.gameObject.SetActive(true);
                break;
            }
        }
        
        cellText.gameObject.SetActive(true);
        cellText.text = count.ToString();
        
        _nameItem = itemName;
        _countItem = count;
    }

    private void CheckUseCurrentItem()
    {
        switch (_nameItem)
        {
            case "Health":
            {
                _playerController.TakeHealth(_countItem);
                StartCoroutine(_backpack.DecreaseCountCells());
                Destroy(gameObject);
                break;
            }
            case "Ammo":
            {
                _playerController.TakeAmmo(_countItem);
                StartCoroutine(_backpack.DecreaseCountCells());
                Destroy(gameObject);
                break;
            }
            case "MachineGun":
            {
                _playerController.CheckToChangeWeapon(0);
                transform.DOScale(1f, 0.2f);
                break;
            }
            case "HeavyMachineGun":
            {
                _playerController.CheckToChangeWeapon(1);
                transform.DOScale(1f, 0.2f);
                break;
            }
            case "SniperGun":
            {
                _playerController.CheckToChangeWeapon(2);
                transform.DOScale(1f, 0.2f);
                break;
            }
        }
    }
    
    public string GetItemName()
    {
        return _nameItem;
    }

    public int GetItemCount()
    {
        return _countItem;
    }
}
