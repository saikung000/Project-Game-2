using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveData : MonoBehaviour {

	public List<EnemyWave> listEnemy = new List<EnemyWave>();



}

[System.Serializable]
public class EnemyWave {
	[SerializeField]
	public GameObject enemyPrefab;
	[SerializeField]
	public int Count , wait;

}
