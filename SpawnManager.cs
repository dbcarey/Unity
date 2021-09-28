using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] powerups; 


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(3.0f);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy =  Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
            
        }
    }
    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(3.0f);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(10.0f);

        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    } 
}
