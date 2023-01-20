using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    

    private Coroutine SpawnRoutine;
    public float SpawnTimeDelay;
    private bool CheckSpawning;

    private const int maxEnemyCount = 3;
    private float currentEnemyCount;

	// Use this for initialization
	void Start () {
        Cannon_EventHandler.resetEnemyCountEvent += ResetEnemyCount;
        Cannon_EventHandler.updateEnemyCountEvent += UpdateEnemyCount;
        ResetEnemyCount();
	}
	
	void Update () {
        if (Cannon_Global.Instance.GameRunning)
        {
            if (SpawnRoutine == null && currentEnemyCount < maxEnemyCount)
            {
                int s = Random.Range(1, 10 * Cannon_Global.Instance.GamePhase);
                SpawnRoutine = StartCoroutine(SpawnEnemy(s));
            }
        }
	}

    private void ResetEnemyCount()
    {
        currentEnemyCount = 0;
    }

    private void UpdateEnemyCount(int i)
    {
        currentEnemyCount += i;
        if(currentEnemyCount > maxEnemyCount)
            currentEnemyCount = maxEnemyCount;
        if (currentEnemyCount < 0)
            currentEnemyCount = 0;
    }

    public IEnumerator SpawnEnemy(int size)
    {

        CheckSpawning = true;
        GameObject enemy;
        int spawnNo = 0;
        if (size <= (10 * Cannon_Global.Instance.GamePhase) / 3)
        {
            spawnNo = 0;
        }
        else if (size > (10 * Cannon_Global.Instance.GamePhase) / 3 && size <= 2*((10 * Cannon_Global.Instance.GamePhase) / 3))
        {
            spawnNo = 1;
        }
        else
        {
            spawnNo = 2;
        }
        enemy = Instantiate(Cannon_Global.Instance.Assets.EnemyPrefabList[spawnNo], Cannon_Global.Instance.Assets.EnemyParent, false);
        enemy.transform.GetChild(0).GetComponent<Enemy>().SetHealth(size);
        int p = Random.Range(1, 46); //Position From (+-)0.1<->4.5 after math
        float pScale = (p-1) / 10;
        int pORm = Random.Range(1, 2);
        if(pORm == 1)
            enemy.transform.position = new Vector3(pScale, 10, -10);
        else
            enemy.transform.position = new Vector3(-pScale, 10, -10);
        currentEnemyCount++;
        yield return new WaitForSeconds(SpawnTimeDelay);

        SpawnRoutine = null;

    }
}
