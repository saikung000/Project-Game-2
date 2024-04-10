using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
public class EnemySpawner : NetworkBehaviour
{
    public int wave;

    public List<WaveData> listWave = new List<WaveData>();
    public GameObject enemyPrefab;
    public float timeSpawner;

    public bool spawnFinish;



    public override void OnStartServer()
    {
        //wave = 1;
       // StartWave();

    }

    IEnumerator SpawnEnemy()
    {
        /*while(true){
		
		   var enemy = (GameObject)Instantiate(enemyPrefab,transform.position, transform.rotation);
            NetworkServer.Spawn(enemy);
		}*/

      
        spawnFinish= false;

        for (int i = 0; i < listWave[wave - 1].listEnemy.Count; i++)
        {

            for (int x = 0; x < listWave[wave - 1].listEnemy[i].Count; x++)
            {
               // print(i + "" + "" + x);
                if(!GameManager.isGameOver){
                    if(listWave[wave - 1].listEnemy[i].enemyPrefab!=null){
                        var enemy = (GameObject)Instantiate(listWave[wave - 1].listEnemy[i].enemyPrefab, transform.position, transform.rotation);
                        NetworkServer.Spawn(enemy);
                    }
                yield return new WaitForSeconds(listWave[wave - 1].listEnemy[i].wait);
                }
            }

        }
        spawnFinish= true;

    }

    public void StartWave()
    {
        if (!isServer)
        {
        return;           
        }
        StartCoroutine(SpawnEnemy());

    }
}



