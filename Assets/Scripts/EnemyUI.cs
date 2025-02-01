using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private GameObject healthBarPanel;
    [SerializeField] private Text healthScore;
    [SerializeField] private Image healthBar;
    
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

    public void ChangeHealth(float health, float maxHealth)
    {
        healthScore.text = health.ToString(CultureInfo.InvariantCulture);
        healthBar.fillAmount = health / maxHealth;
    }
}
