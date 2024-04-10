using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    public GameObject player;
    public int damage;
    public bool isDamage = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    void OnTriggerEnter(Collider other)
    {
        if (!isDamage)
        {
            int x = 0;
            isDamage = true;
            var colls = Physics.OverlapSphere(transform.position, 3.5f);
            foreach (Collider enemy in colls)
            {



                if (enemy.tag == "Enemy")
                {
                    Transform T = enemy.transform;
                    while (T.parent != null) T = T.parent;
                    x++;
                    print(colls.Length + " " + x);
                    var health = T.gameObject.GetComponent<EnemyHealth>();
                    if (health != null)
                    {
                        health.TakeDamage(damage, player);
                    }
                }
            }
            Destroy(gameObject);
        }

    }
    /* void OnCollisionEnter(Collision other) {
		 
        var colls = Physics.OverlapSphere(transform.position, 3.5f);
 		foreach(Collider enemy in colls ){
			if(enemy.tag == "Enemy"){
			var health = enemy.gameObject.GetComponent<EnemyHealth>();
				if (health  != null)
				{
					health.TakeDamage(damage,player);
				}
			}
         }
		 Destroy(gameObject);
		
     }*/
}
