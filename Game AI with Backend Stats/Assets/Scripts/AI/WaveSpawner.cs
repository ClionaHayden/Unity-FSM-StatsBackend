using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns waves of enemies at random spawn points, waits for current enemies to die before starting next wave.
/// </summary>
public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;          // Number of enemies to spawn in this wave
        public float spawnInterval;     // Delay between each enemy spawn
        public GameObject enemyPrefab;  // Prefab used for spawning enemies
    }

    public Wave[] waves;                   // Array of predefined waves
    public Transform[] spawnPoints;       // Locations where enemies can spawn

    private int currentWaveIndex = 0;     // Index of the current wave
    private int enemiesRemainingToSpawn = 0;  // How many enemies still need to be spawned
    private int enemiesRemainingAlive = 0;    // How many enemies are still alive

    public float timeBetweenWaves = 5f;   // Delay between waves
    private float countdown = 0f;         // Timer until next wave

    private WaveStatusUI waveStatusUI;    // UI component to show wave updates
    private int currentWaveCountUI = 0;   // Used to track wave number for the UI

    void Start()
    {
        // Initialize countdown and find the WaveStatusUI component
        countdown = timeBetweenWaves;
        waveStatusUI = GetComponent<WaveStatusUI>();
    }

    void Update()
    {
        // Don't start the next wave until all current enemies are dead
        if (enemiesRemainingAlive > 0)
            return;

        if (countdown <= 0f)
        {
            // Play end-of-wave sound and begin spawning the next wave
            AudioManager.Instance.PlaySFX(AudioManager.Instance.waveEndSound);
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        // Countdown to the next wave
        countdown -= Time.deltaTime;
    }

    /// <summary>
    /// Spawns all enemies in a wave, one at a time with a delay between each.
    /// </summary>
    IEnumerator SpawnWave()
    {
        Wave wave = waves[currentWaveIndex];
        enemiesRemainingToSpawn = wave.enemyCount;
        enemiesRemainingAlive = wave.enemyCount;

        Debug.Log($"Wave {currentWaveIndex} started!");
        AudioManager.Instance.PlaySFX(AudioManager.Instance.waveStartSound);

        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        // Update UI for wave start
        waveStatusUI.ShowWaveStart(currentWaveCountUI + 1);

        // Loop back to first wave if we've reached the end
        currentWaveIndex = (currentWaveIndex + 1) % waves.Length;
        currentWaveCountUI++;
    }

    /// <summary>
    /// Instantiates an enemy at a random spawn point and connects death event.
    /// </summary>
    /// <param name="enemyPrefab">The enemy prefab to spawn.</param>
    void SpawnEnemy(GameObject enemyPrefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Subscribe to the enemy's death event to track remaining enemies
        EnemyAI enemyScript = enemy.GetComponent<EnemyAI>();
        if (enemyScript != null)
        {
            enemyScript.OnEnemyDeath += HandleEnemyDeath;
        }
    }

    /// <summary>
    /// Called when an enemy dies, updates the count of alive enemies.
    /// </summary>
    void HandleEnemyDeath()
    {
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive < 0)
            enemiesRemainingAlive = 0;
    }
}
