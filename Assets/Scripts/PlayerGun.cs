using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerGun : Gun
{
    public Animator gunAnimator;
    public GameObject shootVFX;
    public GameObject hitVFX;
    public Transform shootPosition;
    
    public float cameraShake;
    
    private Camera _playerCamera;
    private PlayerUI _playerUI;
    
    
    private void Start()
    {
        _playerCamera = Camera.main;
        _playerUI = FindObjectOfType<PlayerUI>();
        //_gunAnimator = GetComponent<Animator>();
        
        currentAmmo = maxAmmo;
        currentMaxAmmo = maxAmmo;
        _playerUI.ChangeAmmoScore(currentAmmo, currentMaxAmmo);
    }

    private void Update()
    {
        if (isReloading)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && currentMaxAmmo > 0)
        {
            StartCoroutine(ChangeCameraZoom(normalZoom, false));
            Reload();
            
            return;
        }
        
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
            
            return;
        }

        if (Input.GetButtonDown("Fire2") && isAiming == false)
        {
            ChangeAim(true, "WithAim", aimZoom);
            
        }
        else if (Input.GetButtonUp("Fire2") && isAiming)
        {
            ChangeAim(false, "WithoutAim", normalZoom);
        }
    }
    
    private void ChangeAim(bool aimActive, string aimAnimation, float cameraZoom)
    {
        isAiming = aimActive;
            
        gunAnimator.SetTrigger(aimAnimation);
        StartCoroutine(ChangeCameraZoom(cameraZoom, isAiming));
    }

    public IEnumerator ChangeCameraZoom(float newZoom, bool isAim)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 0.1f)
        {
            _playerCamera.fieldOfView = Mathf.Lerp(_playerCamera.fieldOfView, newZoom, elapsedTime / 1.5f);
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        _playerCamera.fieldOfView = newZoom;
        _playerUI.ChangeActiveAim(isAim);
    }

    private void Shoot()
    {
        currentAmmo--;

        var newShootVFX = Instantiate(shootVFX, shootPosition);
        newShootVFX.SetActive(true);
        
        _playerCamera.transform.DOShakePosition(0.1f, cameraShake);
        _playerUI.ChangeAmmoScore(currentAmmo, currentMaxAmmo);
        
        RaycastHit hit;

        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, range))
        {
            Enemy enemy = hit.transform.GetComponentInParent<Enemy>();

            if (enemy != null && enemy.CheckIsDead() == false)
            {
                var newHitVFX = Instantiate(hitVFX, hit.transform);
                newHitVFX.SetActive(true);
                
                enemy.TakeDamage(damage, hit.transform.tag);
                _playerUI.ChangeColorAndScaleAim(Color.red);
            }
        }
    }

    private void Reload()
    {
        var sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            isReloading = true;
            
            _playerUI.PlayReloadAnimation();
            gunAnimator.SetTrigger(isAiming ? "ReloadWithAim" : "ReloadWithoutAim");
        });
        
        sequence.AppendInterval(reloadTime);

        sequence.AppendCallback(() =>
        {
            isReloading = false;
            isAiming = false;
        
            var shootAmmo = maxAmmo - currentAmmo;

            if (currentMaxAmmo >= shootAmmo)
            {
                currentAmmo += shootAmmo;
                currentMaxAmmo -= shootAmmo;
            }
            else
            {
                currentAmmo += currentMaxAmmo;
                currentMaxAmmo = 0;
            }
            
            //currentAmmo += shootAmmo;
            //maxAmmo -= shootAmmo;
            _playerUI.ChangeAmmoScore(currentAmmo, currentMaxAmmo);
        });
    }
}