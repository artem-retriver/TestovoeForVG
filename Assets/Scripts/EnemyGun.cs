using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyGun : Gun
{
    public GameObject shootVFX;
    public Transform shootPosition;
    
    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    public void Shoot()
    {
        currentAmmo--;
        
        var newShootVFX = Instantiate(shootVFX, shootPosition);
        newShootVFX.SetActive(true);
        
        if (currentAmmo == 0)
        {
            Reload();
        }
    }

    private void Reload()
    {
        var sequence = DOTween.Sequence();

        sequence.AppendCallback(() => { isReloading = true; });

        sequence.AppendInterval(reloadTime);

        sequence.AppendCallback(() =>
        {
            currentAmmo = maxAmmo;
            isReloading = false;
        });
    } 
}