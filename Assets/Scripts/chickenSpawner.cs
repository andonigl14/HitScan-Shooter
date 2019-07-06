using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
public class chickenSpawner : MonoBehaviour
{

    public GameObject casterMinion;
    public GameObject meleeMinion;
    public GameObject SiegeMinion;
    public GameObject SuperMinion;

    public GameObject spawner;   
    public GameObject inhibitor;

    public Stopwatch timeElapsed = new Stopwatch();
    public Text stopWatchText;
    public Text stopWatchText2;
    public Button destroyInhibitorButton;
    public bool inhibitorDestroyed = false;
    public bool pause = false;
    public int WaveCounter = 0;
    
    // we start the timer and the spawner and add a listener for the destroy button
    void Start()
    {
        timeElapsed.Start();
        StartCoroutine(Spawner());
        destroyInhibitorButton.onClick.AddListener(destroyInhibitor);
    }

    // this is just to show the time elapsed in the UI
    void Update()
    {
        stopWatchText.text = timeElapsed.Elapsed.Seconds.ToString();
        stopWatchText2.text = timeElapsed.Elapsed.Minutes.ToString();
    }

    //Spawn a wave every 30 seconds
    IEnumerator Spawner()
    {
        while (!pause) {
            WaveCounter++;
            StartCoroutine(waveCreator());
            yield return new WaitForSeconds(20);
        }
    }

    //here we defifine the minons of each wave taking into account the exercice's conditions
    IEnumerator waveCreator()
    {
        
        for (int minions = 0; minions < 7; minions++)
        {           
            switch (minions)
            {
                case 0:                 
                    
                        if (inhibitorDestroyed)
                        {
                            SuperMinion.transform.position = spawner.transform.position;
                            Instantiate(SuperMinion);
                        }
                        else
                        {
                            if (!(WaveCounter % 2 == 0))
                            {
                                SiegeMinion.transform.position = spawner.transform.position;
                                Instantiate(SiegeMinion);
                            }
                        }                     
                    break;

                case 1:
                case 2:
                case 3:
                    casterMinion.transform.position = spawner.transform.position;
                    Instantiate(casterMinion);
                    break;
                case 4:
                case 5:
                case 6:
                    meleeMinion.transform.position = spawner.transform.position;
                    Instantiate(meleeMinion);
                    break;

                default:
                    UnityEngine.Debug.Log("Something went wrong.");
                    break;
            }
                       
            yield return new WaitForSeconds(0.7f);
        }
               

    }
    //destroy inhibitor
    void destroyInhibitor()
    {
        Destroy(inhibitor);
        inhibitorDestroyed = true;
    }
}
