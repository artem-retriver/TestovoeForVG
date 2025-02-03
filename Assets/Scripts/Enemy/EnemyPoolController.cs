using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enemy
{
    public class EnemyPoolController : MonoBehaviour
    {
        [Header("UI")]
        public UIController uiController;
    
        private List<EnemyController> _enemyPool;
        private int _countEnemy;
        public int _indexScene;

        private void Start()
        {
            if (PlayerPrefs.HasKey("SaveScene"))
            {
                _indexScene = PlayerPrefs.GetInt("SaveScene");

                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    var saveManager = FindObjectOfType<SaveManager>();
                    saveManager.SaveObjects();
                    
                    SceneManager.LoadScene(sceneBuildIndex: _indexScene);
                }
            }
            
            _enemyPool = new List<EnemyController>(GetComponentsInChildren<EnemyController>());
            _countEnemy = _enemyPool.Count;
        }

        public void DecreaseCount()
        {
            _countEnemy--;

            StartCoroutine(CheckCurrentEnemyCount());
        }

        private IEnumerator CheckCurrentEnemyCount()
        {
            yield return new WaitForSeconds(3);
            
            foreach (var enemy in _enemyPool.ToList())
            {
                if (enemy == null)
                {
                    _enemyPool.Remove(enemy);
                }
            }
            
            if (_countEnemy <= 0)
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    StartCoroutine(uiController.ShowNextScene(true, true));
                }
                else
                {
                    StartCoroutine(uiController.ShowNextScene(false, true));
                }
                
                PlayerPrefs.SetInt("SaveScene", 1);
            }
        }
    }
}
