using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private UIController _uiController;
        private List<PlayerGun> _playerGuns;
        
        private float _maxHealth = 100;
        private float _currentHealth;
        private int _currentIndexGun;
        private bool _isStartShowGun;
        private bool _isChangeWeapon;
        private bool _isHaveMachineGun;
        private bool _isHaveHeavyMachineGun;
        private bool _isHaveSniperGun;

        private void Start()
        {
            _playerGuns = new List<PlayerGun>(GetComponentsInChildren<PlayerGun>());
            _uiController = FindObjectOfType<UIController>();
        
            _currentHealth = _maxHealth;
            _uiController.ChangeHealthBarScore(_currentHealth, _maxHealth);
            ChangeActiveAllGuns(false);
        }

        public void CheckToChangeWeapon(int indexWeapon)
        {
            if (_isStartShowGun == false)
            {
                _isStartShowGun = true;
                
                StartCoroutine(ShowNewGun(indexWeapon));
            
                _currentIndexGun = indexWeapon;
            }
            
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

        public void ChooseGun(int indexWeapon)
        {
            ChangeActiveAllGuns(false);
            _playerGuns[indexWeapon].gameObject.SetActive(true);
            _uiController.ChangeActiveAmmoPanel(true);
            _uiController.ChangeAmmoScore(_playerGuns[indexWeapon].currentAmmo, _playerGuns[indexWeapon].currentMaxAmmo);
        }

        public bool CheckToHaveGun(string nameGun)
        {
            switch (nameGun)
            {
                case "MachineGun":
                {
                    if (_isHaveMachineGun)
                    {
                        return true;
                    }
                    break;
                }
                case "HeavyMachineGun":
                {
                    if (_isHaveHeavyMachineGun)
                    {
                        return true;
                    }
                    break;
                }
                case "SniperGun":
                {
                    if (_isHaveSniperGun)
                    {
                        return true;
                    }
                    break;
                }
            }
            return false;
        }

        public void ChangeHaveGun(string nameGun, bool isNeedChooseGun = true)
        {
            switch (nameGun)
            {
                case "MachineGun":
                {
                    _isHaveMachineGun = true;

                    if (isNeedChooseGun)
                    {
                        ChooseGun(0);
                    }
                    break;
                }
                case "HeavyMachineGun":
                {
                    _isHaveHeavyMachineGun = true;
                    
                    if (isNeedChooseGun)
                    {
                        ChooseGun(1);
                    }
                    break;
                }
                case "SniperGun":
                {
                    _isHaveSniperGun = true;
                    
                    if (isNeedChooseGun)
                    {
                        ChooseGun(2);
                    }
                    break;
                }
            }
        }

        public bool CheckHaveAllGun()
        {
            if (_isHaveMachineGun && _isHaveHeavyMachineGun && _isHaveSniperGun)
            {
                var tutorialManager = FindObjectOfType<TutorialManager>();

                if (tutorialManager != null)
                {
                    tutorialManager.ShowOffWall();
                }
                return true;
            }
            return false;
        }

        private void ChangeActiveAllGuns(bool active)
        {
            if (_playerGuns == null)
            {
                return;
            }
            
            foreach (var playerGun in _playerGuns)
            {
                playerGun.gameObject.SetActive(active);
            }
        }

        public void TakeHealth(int amount)
        {
            _currentHealth += amount;

            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
        
            _uiController.InstantiateGetHealth();
            _uiController.ChangeHealthBarScore(_currentHealth, _maxHealth);
        }

        public void TakeAmmo(int amount)
        {
            _playerGuns[_currentIndexGun].IncreaseCurrentMaxAmmo(amount);
        }
    
        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
        
            _uiController.InstantiateGetHit();
            _uiController.ChangeHealthBarScore(_currentHealth, _maxHealth);
        
            if (_currentHealth <= 0)
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    StartCoroutine(_uiController.ShowNextScene(true, false));
                }
                else
                {
                    StartCoroutine(_uiController.ShowNextScene(false, false));
                }
            }
        }
    }
}