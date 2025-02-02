using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Controllers")]
        public PlayerController player;
    
        [Space(10)]
        [Header("Parameters")]
        public float maxHealth = 50;
        public float currentHealth;
        public float maxSpeed;
        public float currentSpeed;
        public float pursuitRange = 25f;
        public float smashRange = 1.5f;
        public bool isSoldier;
        public bool isSniper;
    
        private EnemyUI _enemyUI;
        private EnemyGun _enemyGun;
        private Animator _enemyAnimator;
        private NavMeshAgent _agent;

        private Vector3 _randomDestination;
        private float _wanderRadius = 10f;
        private float _wanderTimer = 3f;
        private float _timer;

        private bool _isNeedPatrol;
        private bool _isTakeDamage;
        private bool _isDistanceToShoot;
        private bool _isDistanceToSmash;
        private bool _isDead;
    
        private void Start()
        {
            currentHealth = maxHealth;
            _timer = _wanderTimer;
            _isNeedPatrol = isSoldier;
        
            _enemyUI = GetComponentInChildren<EnemyUI>();
            _enemyGun = GetComponentInChildren<EnemyGun>();
            _enemyAnimator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
        
            _enemyUI.ChangeHealth(currentHealth, maxHealth);
        }
        
        private void FixedUpdate()
        {
            var distance = Vector3.Distance(transform.position, player.transform.position);

            if (_isDead == false && isSoldier && _isNeedPatrol)
            {
                PatrolArea(distance);
            }
        
            if (_isDead == false && isSoldier && _isNeedPatrol == false)
            {
                MoveTowardsPlayer(distance);
            }
        
            if (_isDead == false && isSniper)
            {
                CheckToShootPlayer(distance, true);
            }
        }
    
        private void PatrolArea(float distance)
        {
            _timer += Time.deltaTime;
            ChangeEnemyAnimation("Walk", maxSpeed, false);
        
            if (_isTakeDamage && _isDistanceToShoot == false)
            {
                _isNeedPatrol = false;
            }
        
            if (distance <= pursuitRange && distance > _enemyGun.range && distance > smashRange)
            {
                _isNeedPatrol = false;
                _isDistanceToShoot = false;
                _isDistanceToSmash = false;
            }
            else if (_timer >= _wanderTimer)
            {
                WanderRandomly();
                _timer = 0;
            }
        }

        private void MoveTowardsPlayer(float distance)
        {
            _agent.SetDestination(player.transform.position);
        
            if (_isTakeDamage && _isDistanceToShoot == false)
            {
                ChangeEnemyAnimation("Walk", maxSpeed);
            }
        
            if (distance <= pursuitRange && distance > _enemyGun.range && distance > smashRange)
            {
                _isDistanceToShoot = false;
                _isDistanceToSmash = false;
            
                ChangeEnemyAnimation("Walk", maxSpeed);
            }
            else if (distance <= _enemyGun.range && distance > smashRange && _isDistanceToSmash == false)
            {
                CheckToShootPlayer(distance);
            }
            else if(distance <= smashRange)
            {
                CheckToSmashPlayer();
            }
        }

        private void WanderRandomly()
        {
            Vector3 randomDirection = Random.insideUnitSphere * _wanderRadius;
            randomDirection += transform.position;
        
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, _wanderRadius, NavMesh.AllAreas))
            {
                _randomDestination = hit.position;
                _agent.SetDestination(_randomDestination);
            }
        }

        private bool CheckToSeePlayer()
        {
            RaycastHit hit;
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
    
            if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out hit, _enemyGun.range))
            {
                var isPlayer = hit.transform.GetComponent<PlayerController>();
                
                if (isPlayer != null)
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckToShootPlayer(float distance, bool isNeedIdle = false)
        {
            if (distance <= _enemyGun.range && CheckToSeePlayer())
            {
                _isDistanceToShoot = true;
                _isDistanceToSmash = false;
            
                ChangeEnemyAnimation("Shoot");
            
                if (Time.time >= _enemyGun.nextTimeToFire && _enemyGun.currentAmmo > 0)
                {
                    _enemyGun.nextTimeToFire = Time.time + _enemyGun.fireRate;
                    StartCoroutine(AttackPlayer());
                }
            }
            else if(isNeedIdle)
            {
                _isDistanceToShoot = false;
                ChangeEnemyAnimation("Idle");
            }
            else if (CheckToSeePlayer() == false)
            {
                ChangeEnemyAnimation("Shoot", maxSpeed);
            }
        }

        private void CheckToSmashPlayer()
        {
            if (Time.time >= _enemyGun.nextTimeToSmash)
            {
                _enemyGun.nextTimeToSmash = Time.time + _enemyGun.smashRate;
                _isDistanceToSmash = true;
        
                ChangeEnemyAnimation("Smash");
                StartCoroutine(AttackPlayer());
            }
        }

        private void ChangeEnemyAnimation(string enemyAnimation, float currentEnemySpeed = 0, bool isNeedLookAtPlayer = true)
        {
            currentSpeed = currentEnemySpeed;
            _enemyAnimator.SetTrigger(enemyAnimation);

            if (isNeedLookAtPlayer)
            {
                transform.LookAt(player.transform);
            }
        
            _agent.speed = currentSpeed;
        }

        private IEnumerator AttackPlayer()
        {
            yield return new WaitForSeconds(1f);

            if (_isDistanceToSmash == false)
            {
                _enemyGun.Shoot(); 
            }
        
            if (_enemyGun.currentAmmo == 0)
            {
                ChangeEnemyAnimation("Reload");
                _enemyGun.Reload();
            }

            if (_isDead == false && CheckToSeePlayer())
            {
                player.TakeDamage(_enemyGun.damage);
            }
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
                currentHealth = 0;
            
                ChangeEnemyAnimation("Death",0,false);
                _enemyUI.ChangeActiveHealthBar(false);
                Destroy(gameObject,5);
            }
        
            _enemyUI.InstantiateDecreaseHealthBarAnimation(amount, nameCollider);
            _enemyUI.ChangeHealth(currentHealth, maxHealth);
        }

        public bool CheckIsDead()
        {
            return _isDead;
        }
    }
}