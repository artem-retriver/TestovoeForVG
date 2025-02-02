using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyUI : MonoBehaviour
    {
        [Header("Health Bar")]
        public GameObject healthBarPanel;
        public GameObject healthBarAnimation;
        public Text healthDecreaseScore;
        public Text healthScore;
        public Image healthBar;
    
        private Camera _playerCamera;

        private void Start()
        {
            _playerCamera = Camera.main;
        }

        private void FixedUpdate()
        {
            if (_playerCamera != null)
            {
                gameObject.transform.LookAt(_playerCamera.transform);
            }
            else
            {
                Debug.LogError("Player Camera is null");
            }
        }

        public void ChangeActiveHealthBar(bool active)
        {
            healthBarPanel.SetActive(active);
        }

        public void InstantiateDecreaseHealthBarAnimation(float healthDecrease, string bodyPart)
        {
            healthDecreaseScore.text = bodyPart + "\n" + "-" + healthDecrease;
            var newHealthBarAnimation = Instantiate(healthBarAnimation, transform);
            newHealthBarAnimation.SetActive(true);
            Destroy(newHealthBarAnimation, 3);
        }

        public void ChangeHealth(float health, float maxHealth)
        {
            healthScore.text = health.ToString(CultureInfo.InvariantCulture);
            healthBar.fillAmount = health / maxHealth;
        }
    }
}
