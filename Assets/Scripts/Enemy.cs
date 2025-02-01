using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [Header("Controllers")]
    public PlayerController player;
    
    [Space(10)]
    [Header("Parameters")]
    public float maxHealth = 50;
    public float currentHealth;
    public float speed = 2f;
    public float pursuitRange = 25f;
    
    private EnemyUI _enemyUI;
    private EnemyGun _enemyGun;
    private Animator _enemyAnimator;
    private Sequence _sequence;

    private bool _isTakeDamage;
    private bool _isDistanceToAttack;
    private bool _isDead;
    
    private void Start()
    {
        currentHealth = maxHealth;
        
        _enemyUI = GetComponentInChildren<EnemyUI>();
        _enemyGun = GetComponentInChildren<EnemyGun>();
        _enemyAnimator = GetComponent<Animator>();
        
        _enemyUI.ChangeHealth(currentHealth, maxHealth);
    }

    private void FixedUpdate()
    {
        if (_isDead == false)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, 0, player.transform.position.z), speed * Time.deltaTime);

        if (_isTakeDamage && _isDistanceToAttack == false)
        {
            ChangeEnemyAnimation("Walk", 4);
        }
        
        var distance = Vector3.Distance(transform.position, player.transform.position);
        
        if (distance <= pursuitRange && distance > _enemyGun.range)
        {
            _isDistanceToAttack = false;
            
            ChangeEnemyAnimation("Walk", 4);
        }
        else if (distance <= _enemyGun.range)
        {
            _isDistanceToAttack = true;
            
            ChangeEnemyAnimation("Shoot", 0);
            
            if (Time.time >= _enemyGun.nextTimeToFire && _enemyGun.currentAmmo > 0)
            {
                _enemyGun.nextTimeToFire = Time.time + _enemyGun.fireRate;
                StartCoroutine(AttackPlayer());
            }
        }
    }

    private void ChangeEnemyAnimation(string enemyAnimation, float currentSpeed)
    {
        _enemyAnimator.SetTrigger(enemyAnimation);
        transform.LookAt(player.transform);
        speed = currentSpeed;
    }

    private IEnumerator AttackPlayer()
    {
        //if (_enemyGun.currentAmmo <= 0)
        //{
        //    return;
        //}
        
        yield return new WaitForSeconds(1f);
        
        _enemyGun.Shoot(); 
        player.TakeDamage(_enemyGun.damage);
    }
    
    public void TakeDamage(float amount, string nameCollider)
    {
        Debug.Log("Taking Damage " + nameCollider);
        _isTakeDamage = true;
        
        switch (nameCollider)
        {
            case "Head":
            {
                amount += 20;
                break;
            }
            case "Body":
            {
                amount += 10;
                break;
            }
            case "Arm":
            case "Leg":
            {
                amount += 5;
                break;
            }
        }
        
        currentHealth -= amount;
        
        if (currentHealth <= 0)
        {
            _isDead = true;
            speed = 0;
            currentHealth = 0;
            
            _enemyAnimator.SetTrigger("Death");
            _enemyUI.ChangeActiveHealthBar(false);
            Destroy(gameObject,5);
        }
        
        _enemyUI.ChangeHealth(currentHealth, maxHealth);
    }

    public bool CheckIsDead()
    {
        return _isDead;
    }
}