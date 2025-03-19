using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float playerHealth = 100f;

    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private int enemyCount = 100;
    [SerializeField] private float enemyHealth = 50f;
    [SerializeField] private float enemyDamage = 10f;

    [Header("Game Settings")]
    [SerializeField] private GameObject finishPointPrefab;

    [Header("References")]
    [SerializeField] private MazeGenerator mazeGenerator; // Make this serializable

    private List<GameObject> enemies = new List<GameObject>();
    private GameObject player;
    private GameObject finishPoint;

    void Start()
    {
        if (mazeGenerator == null)
        {
            mazeGenerator = GetComponent<MazeGenerator>();
            if (mazeGenerator == null)
            {
                Debug.LogError("MazeGenerator component not found! Please attach it to the same GameObject or assign it in the inspector.");
                return;
            }
        }
        StartCoroutine(StartGameSequence());
    }

    IEnumerator StartGameSequence()
    {
        SpawnPlayer();
        yield return new WaitForSeconds(2f);
        SpawnEnemies();
        yield return new WaitForSeconds(2f);
        SpawnFinishPoint();
    }

    void SpawnPlayer()
    {
        // Spawn at first cell (0,0)
        Vector3 spawnPos = new Vector3(0, 1, 0);
        player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        if (player == null)
        {
            Debug.LogError("Failed to spawn player. Make sure playerPrefab is assigned in the inspector.");
        }
    }

    void SpawnEnemies()
    {
        if (mazeGenerator == null || enemyPrefab == null || healthBarPrefab == null)
        {
            Debug.LogError("Required components are missing. Check inspector assignments.");
            return;
        }

        int mazeWidth = mazeGenerator._mazeWidth;
        int mazeDepth = mazeGenerator._mazeDepth;

        for (int i = 0; i < enemyCount; i++)
        {
            // Generate random position (avoiding 0,0)
            Vector3 randomPos;
            do
            {
                randomPos = 3f * new Vector3(
                    Random.Range(0, mazeWidth),
                    0.5f / 3f,
                    Random.Range(0, mazeDepth)
                );
            } while (randomPos.x == 0 && randomPos.z == 0);

            GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
            GameObject healthBar = Instantiate(healthBarPrefab,
                enemy.transform.position + Vector3.up * 2,
                Quaternion.identity,
                enemy.transform);

            Enemy enemyScript = enemy.AddComponent<Enemy>();
            enemyScript.Initialize(enemyHealth, enemyDamage, healthBar.GetComponent<HealthBar>());
            enemies.Add(enemy);
        }
    }

    void SpawnFinishPoint()
    {
        if (mazeGenerator == null || finishPointPrefab == null)
        {
            Debug.LogError("Required components are missing for finish point spawn.");
            return;
        }

        Vector3 finishPos = new Vector3(
            3 * mazeGenerator._mazeWidth - 1,
            0.5f,
            3 * mazeGenerator._mazeDepth - 1
        );
        finishPoint = Instantiate(finishPointPrefab, finishPos, Quaternion.identity);
        finishPoint.SetActive(false);
    }

    public void OnEnemyDefeated(GameObject enemy)
    {
        if (enemy != null)
        {
            enemies.Remove(enemy);
            Destroy(enemy);

            if (enemies.Count == 0 && finishPoint != null)
            {
                finishPoint.SetActive(true);
            }
        }
    }
}