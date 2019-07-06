using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthColl : MonoBehaviour {

    [SerializeField] GameObject gravityField;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "asteroid")
        {
            Destroy(collision.gameObject);
            transform.localScale = transform.localScale + new Vector3(0.2f, 0.2f, 0.2f);
            gravityField.transform.localScale = gravityField.transform.localScale + new Vector3(20, 20, 20);
        }
    }
}
