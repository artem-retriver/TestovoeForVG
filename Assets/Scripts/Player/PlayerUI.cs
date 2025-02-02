using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerUI : MonoBehaviour
    {
        [Header("AIM")]
        public GameObject aimTarget;
        public Image[] aimImages;

        [Space(10)] 
        [Header("AMMO")] 
        public Animator ammoAnimator;
        public Text ammoScore;
    
        [Space(10)] 
        [Header("HP")] 
        public Image healthBar;
        public Text healthBarScore;
    
        [Space(10)] 
        [Header("HIT")] 
        public GameObject getHit;
    
        private Color _currentColorAim;

        private void Start()
        {
            _currentColorAim = aimImages[0].color;
        }

        public void PlayReloadAnimation()
        {
            ammoAnimator.SetTrigger("Reload");
        }

        public void InstantiateGetHit()
        {
            var newGetHit = Instantiate(getHit, transform);
            newGetHit.SetActive(true);
            Destroy(newGetHit, 2f);
        }

        public void ChangeAmmoScore(int currentAmmo, int maxAmmo)
        {
            ammoScore.text = currentAmmo + "/" + maxAmmo;
        }

        public void ChangeHealthBarScore(float currentHealth, float maxHealth)
        {
            var health = currentHealth / maxHealth;

            if (health <= 0)
            {
                health = 0;
            }
        
            healthBarScore.text = currentHealth + "/" + maxHealth;
            healthBar.transform.DOScaleX(health, 0);
        }

        public void ChangeActiveAim(bool active)
        {
            aimTarget.SetActive(active);
        }
    
        public void ChangeColorAndScaleAim(Color color)
        {
            var sequence = DOTween.Sequence();

            sequence.AppendCallback(() =>
            {
                foreach (var aim in aimImages)
                {
                    aim.color = new Color(color.r, color.g, color.b);
                }
            
                aimTarget.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
            });
        
            sequence.AppendInterval(0.2f);
        
            sequence.AppendCallback(() =>
            {
                foreach (var aim in aimImages)
                {
                    aim.color = _currentColorAim;
                }
            
                aimTarget.transform.DOScale(0.5f, 0.2f).SetEase(Ease.OutBounce);
            });
        }
    }
}