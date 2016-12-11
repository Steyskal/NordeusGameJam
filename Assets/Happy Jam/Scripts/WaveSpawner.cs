using UnityEngine;
using System.Collections;
using Happy;
using UnityEngine.Events;
using System;

[Serializable]
public class CountStringEvent : UnityEvent<string> { }
public class WaveSpawner : MonoSingleton<WaveSpawner>
{
    const float ENEMY_SEARCH_TIME_FREQUENCY = 1f;

    public enum SpawnState
    {
        Off,
        Spawning,
        Waiting,
        Counting
    }

    public Wave[] Waves;
    public float TimeBetweenWaves = 5f;
    public bool LoopAfterComplete = false;
    //public bool SpawnRandom = false;

    [Header("ReadOnly")]
    [SerializeField]
    private float __WaveCountDown;
    [SerializeField]
    private SpawnState _State = SpawnState.Off;

    [Header("Events")]
    public CountStringEvent WaveTotalCallback = new CountStringEvent();
    //public CustomUnityEvent OnWavesStart = new CustomUnityEvent();
    public CustomUnityEvent OnWaveStart = new CustomUnityEvent();
    public CountStringEvent OnWaveStartStringCount = new CountStringEvent();
    public CustomUnityEvent OnWavesCompleted = new CustomUnityEvent();
    public CustomUnityEvent OnWaveCompleted = new CustomUnityEvent();

    private int _nextWave = 0;
    private float _searchCountdown = ENEMY_SEARCH_TIME_FREQUENCY;
    private Transform _transform;

    [Header("Testing")]
    public int RemainingEnemies = 1;
    public bool StartOnStart = false;
    public bool SetEnemyRotation = true;

    void Awake()
    {
        __WaveCountDown = TimeBetweenWaves;
        _State = SpawnState.Off;
        _transform = transform;
        WaveTotalCallback.Invoke(Waves.Length.ToString());
        if (StartOnStart) _State = SpawnState.Counting;
    }

    void Update()
    {
        if (_State == SpawnState.Waiting)
        {
            if (!IsEnemyAlive())
            {
                WaveCompleted();
            }
        }
        else if (_State != SpawnState.Off)
        {
            if (__WaveCountDown <= 0)
            {
                if (_State != SpawnState.Spawning)
                {
                    OnWaveStartStringCount.Invoke((_nextWave + 1).ToString());
                    StartCoroutine(SpawnWave(Waves[_nextWave]));
                }
            }
            else
            {
                __WaveCountDown -= Time.deltaTime;
            }
        }
    }


    IEnumerator SpawnWave(Wave wave)
    {
        _State = SpawnState.Spawning;
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.GetWaveEnemy());
            yield return new WaitForSeconds(wave.delay);
        }
        _State = SpawnState.Waiting;
        yield break;
    }

    //TODO: Fix enemyScript.SetTarget(tempTarget);
    void SpawnEnemy(WaveEnemy waveEnemy)
    {
        Debug.Log("Spawning Enemy " + waveEnemy.enemy.name);
        Transform spawnTransform;
        spawnTransform = waveEnemy.GetRandomSpawn();

        if (spawnTransform == null)
            spawnTransform = _transform;

        Transform enemy = Instantiate(waveEnemy.enemy, spawnTransform.position, spawnTransform.rotation) as Transform;
        if (SetEnemyRotation)
        {
            /// Calculating rotation;
            GameObject obj = GameManager.Instance.GetPlayer(spawnTransform.position);
            float targetOrientation = 0;
            if (obj)
            {
                Vector3 direction = obj.transform.position - spawnTransform.position;
                targetOrientation = Mathf.Atan2(direction.y, direction.x);
                targetOrientation *= Mathf.Rad2Deg;

                AI.Agent2D agent = enemy.GetComponent<AI.Agent2D>();
                if (agent)
                    agent.rotation = targetOrientation;
                else
                    enemy.transform.eulerAngles = new Vector3(0, 0, targetOrientation);
            }
        }

    }
    void WaveCompleted()
    {
        Debug.Log("Wave Completed");
        if (OnWaveCompleted != null) OnWaveCompleted.Invoke();

        __WaveCountDown = TimeBetweenWaves;
        if (_nextWave + 1 > Waves.Length - 1)
        {
            Debug.Log("All Wave's Completed");
            if (OnWaveCompleted != null) OnWavesCompleted.Invoke();
            if (LoopAfterComplete)
            {
                _nextWave = 0;
            }
            else
            {
                _State = SpawnState.Off;
            }
        }
        else
        {
            _nextWave++;
            _State = SpawnState.Counting;
        }
    }

    bool IsEnemyAlive()
    {
        bool alive = true;
        _searchCountdown -= Time.deltaTime;

        if (_searchCountdown <= 0)
        {
            _searchCountdown = ENEMY_SEARCH_TIME_FREQUENCY;
            if (GetCurrentEnemiesCount() == 0)
                alive = false;
        }
        return alive;
    }

    // TODO REPLACE
    public int GetCurrentEnemiesCount()
    {
        return RemainingEnemies;
        //GameManager.Instance.Enemies.Length
    }

    public void EnableSpawning(bool enable)
    {
        if (enable)
        {
            _State = SpawnState.Counting;
        }
        else
        {
            _State = SpawnState.Off;
        }
    }


    [System.Serializable]
    public class Wave
    {
        public string name;
        public WaveEnemy[] enemies;
        [Tooltip("Number of enemies to spawn")]
        public int count;
        public bool spawnRandom = false;
        public float delay;

        public WaveEnemy GetWaveEnemy()
        {
            WaveEnemy enemy = null;
            if (!spawnRandom)
            {
                int maxDiff = -1;
                foreach (WaveEnemy e in enemies)
                {
                    if (e.Count - e.counter > maxDiff)
                    {
                        maxDiff = e.Count - e.counter;
                        enemy = e;
                    }
                }
            }
            if (spawnRandom || enemy == null)
            {
                int random = UnityEngine.Random.Range(0, 100);
                if (enemies != null && enemies.Length > 0)
                {
                    enemy = enemies[random % enemies.Length];
                }
            }
            if (enemy != null)
                enemy.counter++;
            return enemy;
        }
    }

    [System.Serializable]
    public class WaveEnemy
    {
        public Transform enemy;
        public Transform[] spawns;
        public int Count;
        [HideInInspector]
        public int counter = 0;

        public Transform GetRandomSpawn()
        {
            Transform transf = null;
            if (spawns != null && spawns.Length > 0)
            {
                transf = spawns[UnityEngine.Random.Range(0, spawns.Length)];
            }
            return transf;
        }
    }
}
