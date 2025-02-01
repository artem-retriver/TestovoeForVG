using DG.Tweening;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerController _playerController;
    private MoveController _moveController;
    private Camera _playerCamera;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _moveController = GetComponent<MoveController>();
        _playerCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _moveController.speed = 10f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveController.speed = 5f;
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 30f), 0.2f);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 0f), 0.2f);
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, -30f), 0.2f);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 0f), 0.2f);
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            _playerCamera.transform.DOMoveY(1.5f, 0.2f);
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            _playerCamera.transform.DOMoveY(3f, 0.2f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _playerController.CheckToChangeWeapon(0);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _playerController.CheckToChangeWeapon(1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _playerController.CheckToChangeWeapon(2);
        }
    }
}