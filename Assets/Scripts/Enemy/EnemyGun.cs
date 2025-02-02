using DG.Tweening;
using UnityEngine;

namespace Enemy
{
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
        }

        public void Reload()
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
}