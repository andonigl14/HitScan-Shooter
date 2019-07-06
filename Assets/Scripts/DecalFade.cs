using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalFade : MonoBehaviour {

    [SerializeField] GameObject decal;
    Color color;
	void Awake()
    {
        
        color = decal.GetComponent<MeshRenderer>().material.color;
        color.a = 1f;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        
        yield return new WaitForSeconds(5f);
        while (color.a > 0)
        {           
            color.a = color.a - 0.01f;
            decal.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(this.gameObject);    
                
    }
}
