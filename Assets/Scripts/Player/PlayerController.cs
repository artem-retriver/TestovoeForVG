using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private List<PlayerGun> _playerGuns;
        private PlayerUI _playerUI;
        
        private float _maxHealth = 100;
        private float _currentHealth;
        private int _currentIndexGun;
        private bool _isChangeWeapon;

        private void Start()
        {
            _playerGuns = new List<PlayerGun>(GetComponentsInChildren<PlayerGun>());
            _playerUI = GetComponentInChildren<PlayerUI>();
        
            _currentHealth = _maxHealth;
            _playerUI.ChangeHealthBarScore(_currentHealth, _maxHealth);
            ChooseGun(_currentIndexGun);
        }

        public void CheckToChangeWeapon(int indexWeapon)
        {
            if (_currentIndexGun != indexWeapon && _playerGuns[_currentIndexGun].isReloading == false)
            {
                StartCoroutine(ShowNewGun(indexWeapon));
            
                _currentIndexGun = indexWeapon;
            }
        }
    
        private IEnumerator ShowNewGun(int indexWeapon)
        {
            _playerGuns[_currentIndexGun].gunAnimator.SetTrigger("HideGun");
            StartCoroutine(_playerGuns[_currentIndexGun].ChangeCameraZoom(60, false));
        
            yield return new WaitForSeconds(0.2f);
        
            ChooseGun(indexWeapon);
        }

        private void ChooseGun(int indexWeapon)
        {
            ChangeActiveAllGuns(false);
        
            _playerGuns[indexWeapon].gameObject.SetActive(true);
            _playerUI.ChangeAmmoScore(_playerGuns[indexWeapon].currentAmmo, _playerGuns[indexWeapon].currentMaxAmmo);
        }

        private void ChangeActiveAllGuns(bool active)
        {
            foreach (var playerGun in _playerGuns)
            {
                playerGun.gameObject.SetActive(active);
            }
        }
    
        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
        
            _playerUI.InstantiateGetHit();
            _playerUI.ChangeHealthBarScore(_currentHealth, _maxHealth);
        
            if (_currentHealth <= 0)
            {
                Debug.Log("YOU DIE!!!");
            }
        }
    }
}