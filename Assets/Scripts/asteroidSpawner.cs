using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroidSpawner : MonoBehaviour {

    [SerializeField] GameObject[] asteroids;
    GameObject asteroid;
    [SerializeField] GameObject target;
    [SerializeField] float force = 8000;
    bool pause = false;
	
	void Start () {
        StartCoroutine(Spawner());
    }
		
    IEnumerator Spawner()
    {
        while (!pause)
        {
            spawnAsteroid();
            yield return new WaitForSeconds(4);
        }
    }

    // we spawn a random asteriod and we add force to slowly go to the earth, simuting space's lack of gravity
    void spawnAsteroid()
    {
        asteroid = Instantiate(asteroids[Random.Range(0,2)], this.transform);
        asteroid.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position)* force);
    }

}
