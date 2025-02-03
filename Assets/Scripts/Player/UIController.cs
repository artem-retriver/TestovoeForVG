using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("AIM")]
    public GameObject aimTarget;
    public Image[] aimImages;

    [Space(10)] 
    [Header("AMMO")] 
    public GameObject ammoPanel;
    public Animator ammoAnimator;
    public Text ammoScore;
    
    [Space(10)] 
    [Header("HP")] 
    public GameObject getHealth;
    public Image healthBar;
    public Text healthBarScore;
    
    [Space(10)] 
    [Header("HIT")] 
    public GameObject getHit;
    
    [Space(10)] 
    [Header("Backpack")] 
    public GameObject backpackPanel;
    public Backpack backpack;
    
    [Space(10)] 
    [Header("Show/Hide")] 
    //public GameObject showGameAnimation;
    public GameObject hideGameAnimation;
    
    private Color _currentColorAim;
    private bool _isBackpackPanelActive;

    private void Start()
    { 
        _currentColorAim = aimImages[0].color;
    }

    public IEnumerator ShowNextScene(bool isStartScene, bool isWin)
    {
        hideGameAnimation.SetActive(true);
        
        yield return new WaitForSeconds(2f);

        var saveManager = FindObjectOfType<SaveManager>();
        saveManager.SaveObjects();

        if (isStartScene && isWin)
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
        else if (isStartScene)
        {
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
        else
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }

    public void PlayAmmoAnimation(string nameAnimation)
    { 
        ammoAnimator.SetTrigger(nameAnimation);
    }

    public void InstantiateGetHit()
    { 
        var newGetHit = Instantiate(getHit, transform);
        newGetHit.SetActive(true);
        Destroy(newGetHit, 2f);
    }
    
    public void InstantiateGetHealth()
    { 
        var newGetHealth = Instantiate(getHealth, transform);
        newGetHealth.SetActive(true);
        Destroy(newGetHealth, 2f);
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

    public bool CheckIsBackpackPanelActive()
    {
        return _isBackpackPanelActive;
    }

    public void ChangeActiveAmmoPanel(bool isActive)
    {
        ammoPanel.SetActive(isActive);
    }

    public void ChangeActiveBackpackPanel(bool active)
    {
        _isBackpackPanelActive = active;
        backpackPanel.SetActive(active);
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
            
            aimTarget.transform.DOScale(1.5f, 0.2f).SetEase(Ease.OutBounce);
        });
        
        sequence.AppendInterval(0.2f);
        
        sequence.AppendCallback(() =>
        {
            foreach (var aim in aimImages)
            {
                aim.color = _currentColorAim;
            }
            
            aimTarget.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
        });
    }
}