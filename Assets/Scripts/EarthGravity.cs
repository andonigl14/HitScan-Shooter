using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthGravity : MonoBehaviour {

    // Use this for initialization
    [SerializeField] GameObject target;
    float force = 10000;	

    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag == "asteroid")
        {
            Debug.Log("hola");
            collision.gameObject.GetComponent<Rigidbody>().AddForce((target.transform.position - collision.transform.position) * force);

        }
    }

    }
