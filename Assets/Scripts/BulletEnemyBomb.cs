using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyBomb : MonoBehaviour
{
	public float area;
    public float waittime;
    public GameObject EffectBomb;
    public int damage;
    // Use this for initialization
    void Start()
    {
		StartCoroutine(waitBomb());
        var bombFx = (GameObject)Instantiate(EffectBomb, transform.position, transform.rotation);
        Destroy(bombFx, 20);
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator waitBomb()
    {
        yield return new WaitForSeconds(waittime);
		
        var colls = Physics.OverlapSphere(transform.position,area);
        foreach (Collider enemy in colls)
        {
            if (enemy.tag == "Player")
            {
                var health = enemy.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }

		Destroy(this.gameObject);
    }


}
	

