using System.Collections;
using DataBase;
using NTC.Global.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace EnemySystem
{
    public class Wave : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private GameObject[] enemies;
        [SerializeField] private GameObject[] bossEnemies;
        
        [SerializeField] private Transform[] spawnPoints;

        [Header("UI")]
        [SerializeField] private BossSystem bossSystem;
        [SerializeField] private FadeAnimation waveCleared;

        [SerializeField] private GameObject alert;
        [SerializeField] private TMP_Text waveCounter;
        [SerializeField] private Image indicator;

        [SerializeField] private float waveTime;
        private float _currentWaveTime;

        private byte _wavePast; //in range 0 - 10
        private ushort _difficult = 1; //waves
        public short enemiesFactor = 1;
        
        private int _enemiesCount;
        
        private bool _isAlert;

        public static Wave Instance;

        private void Awake() => Instance = this;

        private void Start()
        {
            switch (DB.Access.gameDifficult)
            {
                case 0:
                    waveTime = 50;
                    break;
                case 1:
                    waveTime = 42;
                    break;
                case 2:
                    waveTime = 35;
                    enemiesFactor++;
                    break;
                case 3:
                    waveTime = 30;
                    enemiesFactor += 2;
                    break;
            }

            UpdateIndicators();
            StartCoroutine(WaveSystem());
        }

        public ushort GetCurrentWavesCount() => _difficult;

        private IEnumerator WaveSystem()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                _currentWaveTime -= 0.1f;
                indicator.fillAmount = ((_currentWaveTime * 100) / waveTime) / 100;
                    
                if (indicator.fillAmount <= 0.15f && !_isAlert)
                {
                    _isAlert = true;
                    StartCoroutine(StartAlert());
                }

                if (_currentWaveTime <= 0)
                {
                    MusicSystem.Instance.SwitchAudio(true);
                    
                    if (_difficult == 10 || _difficult == 20 || _difficult == 30) SpawnBoss();
                    else SpawnEnemies();
                    
                    yield return new WaitForSeconds(1f);
                    StopCoroutine(StartAlert());
                    alert.SetActive(false);
                    _isAlert = false;
                    break;
                }
            }
        }

        private void UpdateIndicators()
        {
            waveCounter.text = _difficult.ToString();

            if (_wavePast == 5 || _wavePast == 10) waveTime += 3;
            
            _currentWaveTime = waveTime;
        }

        private IEnumerator StartAlert()
        {
            while (_isAlert)
            {
                alert.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                alert.SetActive(false);
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void SpawnEnemies()
        {
            if (_difficult >= 5) _enemiesCount = Random.Range(_difficult / 3, (_difficult / 2) + 1);
            else _enemiesCount = Random.Range(1, _difficult + 1);
            
            for (int i = 0; i < _enemiesCount; i++)
            {
                int enemyID = Random.Range(0, enemies.Length);

                while ((DB.Access.gameData.ChosenPlanet == 1 && enemyID == 5)
                       || (DB.Access.gameData.ChosenPlanet == 2 && enemyID == 0))
                {
                    enemyID = Random.Range(0, enemies.Length);
                }

                Enemy enemy = NightPool.Spawn(enemies[enemyID], GetRandomPosition(),
                    Quaternion.identity).GetComponent<Enemy>();
                
                enemy.hp *= enemiesFactor;
            }
        }

        private void SpawnBoss()
        {
            _enemiesCount = 1;

            if (_difficult == 10)
            {
                Instantiate(bossEnemies[0], GetRandomPosition(),
                    Quaternion.identity);
                bossSystem.Begin(0);
            }
            else if (_difficult == 20)
            {
                Instantiate(bossEnemies[1], GetRandomPosition(),
                    Quaternion.identity);
                bossSystem.Begin(1);
            }
            else if (_difficult == 30)
            {
                Instantiate(bossEnemies[2], GetRandomPosition(),
                    Quaternion.identity);
                bossSystem.Begin(2);
            }
        }

        private Vector2 GetRandomPosition()
        {
            int id = Random.Range(0, spawnPoints.Length);
            return new Vector2(spawnPoints[id].position.x + Random.Range(-2.9f, 2.9f), spawnPoints[id].position.y);
        }
        
        public void ChangeBossHpBarFill(float newHp, float defaultHp)
        {
            bossSystem.ChangeHpValue((newHp * 100) / defaultHp / 100);
        }

        public void DecreaseEnemyCount()
        {
            _enemiesCount--;

            if (_enemiesCount <= 0)
            {
                StartCoroutine(WaveClearedAlert());
                
                _difficult++;
                UpdateIndicators();
                _wavePast++;

                bossSystem.End();

                if (_wavePast >= 10)
                {
                    _wavePast = 0;
                    enemiesFactor++;
                }
                
                UpgradesSystem.Instance.CheckForNewAchievement((ushort)(_difficult - 1));

                StartCoroutine(WaveSystem());
                MusicSystem.Instance.SwitchAudio(false);
            }
        }

        private IEnumerator WaveClearedAlert()
        {
            waveCleared.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            waveCleared.EndFadeAnimCanvasGroup();
        }
    }
}
