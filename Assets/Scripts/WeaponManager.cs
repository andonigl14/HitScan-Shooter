using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Parameters")]
    [SerializeField] int weaponAmmo = 60;// total ammo
    [SerializeField] int weaponMagazine = 20; // current magazine ammo
    [SerializeField] int magazineSize = 20; // size of each magazine
    [SerializeField] float weaponDamage = 30; //Weapon Damage to damageable objects
    [SerializeField] float weaponSpeed = 3750; //Glock 375 m/s
    [SerializeField] float bulletCaliber = 6; // Spepherecast size
    [SerializeField] float recoil = 2f; //weapon recoil   
    [SerializeField] float deflectionRate = 0.015f;

    [Header("Assigned GameObjects")] // we need to add this objects using the inspector
    [SerializeField] GameObject bulletHole;   // bullethole decal
    [SerializeField] GameObject bulletDust;   // bullethole decal
    [SerializeField] Text bulletText;
    [SerializeField] GameObject gunFlash;
    [SerializeField] GameObject gunBarrel;
    [SerializeField] Animator gunAnimator;


    GameObject bulletAux;
    RaycastHit[] sphereCastHits;
  

    void Start()
    {       
        bulletText.text = weaponMagazine + "/" + weaponAmmo;        
    }

    void Update()
    {
        WeaponManagement();

        //Debug shooting
        if (Input.GetKey(KeyCode.K))
        {
            FireWeapon();
        }
    }

    void WeaponManagement()
        {
        //Reload the weapon
        if (Input.GetButtonDown("Reload") && weaponMagazine != magazineSize && weaponAmmo > 0)
        {            
            if (weaponMagazine <= 0)
                gunAnimator.SetTrigger("ReloadEmpty");
            else
                gunAnimator.SetTrigger("Reload");

            // we call ReloadWeapon() in animation event
        }

        //Fire the weapon
        if (Input.GetButtonDown("Fire1"))
        {
            //if we have ammo but the magazine is empty we reload
            if (weaponMagazine <= 0 && weaponAmmo > 0)
            {
                gunAnimator.SetTrigger("ReloadEmpty");
                // we call ReloadWeapon() in animation event
            }
            else
            {
                if (weaponMagazine > 0)
                {
                    gunAnimator.SetTrigger("FireGun");
                    //FireWeapon();
                    // we call Fireweapon()in animation event

                }
                //IF we have no ammo play no bullet audio
                if (weaponMagazine <= 0 && weaponAmmo <= 0)
                {
                    gunAnimator.SetTrigger("EmptyFire");
                    
                }
            }
        }        
    }

  
    public void FireWeapon()
    {
        weaponMagazine--;

        bulletText.text = weaponMagazine + "/" + weaponAmmo;
        StartCoroutine(PlayGunflash());

        Vector3 deflection = new Vector3(0, Random.Range(-deflectionRate, deflectionRate), Random.Range(-deflectionRate, deflectionRate));
        // Bit shift the index of the "Damageable " (9), "hard" layer (10) and "soft" layer (11) to get a bit mask
        int layerMask = (1 << 9) | (1 << 10) | (1 << 11);

        // layerMask = ~layerMask; // This would cast rays against all colliders but layer 9,10,11.

        // Does the ray intersect any objects in the "Damageable","Hard" or "Soft" layer
        RaycastHit[] sphereCastHits = Physics.SphereCastAll(gunBarrel.transform.position, bulletCaliber, gunBarrel.transform.TransformDirection(Vector3.forward + deflection), weaponSpeed * Time.deltaTime, layerMask, QueryTriggerInteraction.UseGlobal);

        Debug.DrawRay(gunBarrel.transform.position, gunBarrel.transform.TransformDirection(Vector3.forward + deflection + new Vector3(0, 0, weaponSpeed * Time.deltaTime)), Color.red, 10f, true);

        if (sphereCastHits.GetLength(0) > 0)
        {
            Debug.Log("Collision detected");

            // FIRST OBJECT COLLIDED
            //bullet decal to see where we have shot
            bulletAux = Instantiate(bulletHole, sphereCastHits[0].point, Quaternion.FromToRotation(Vector3.up, sphereCastHits[0].normal));
            bulletAux.transform.parent = sphereCastHits[0].transform;
            Instantiate(bulletDust, sphereCastHits[0].point, Quaternion.FromToRotation(Vector3.up, sphereCastHits[0].normal));

            Debug.Log("all instaciated");
            //Do weapon Damage to the enemy in layer 9
            if (sphereCastHits[0].transform.gameObject.layer == 9)
            {
                Debug.Log("Collision detected in layer 9");
                sphereCastHits[0].transform.gameObject.GetComponent<vp_DamageHandler>().Damage(1f);
            }

            //SECOND+ OBJECT COLLIDED
            //in this for we just check if the object can be passed throught (layer 11 " soft"), if not we break the for.
            for (int i = 0; i <= sphereCastHits.GetLength(0) - 1; i++)
            {
                if (sphereCastHits[i].transform.gameObject.layer == 11)
                {
                    bulletAux = Instantiate(bulletHole, sphereCastHits[i + 1].point, Quaternion.FromToRotation(Vector3.up, sphereCastHits[0].normal));
                    bulletAux.transform.parent = sphereCastHits[i + 1].transform;
                    Debug.Log("Collision detected in layer 11");

                    //sphereCastHits[i].transform.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(1000, 0, 0));

                    if (sphereCastHits[i + 1].transform.gameObject.layer == 9)
                        sphereCastHits[i + 1].transform.gameObject.GetComponent<vp_DamageHandler>().Damage(1f);
                }
                else
                {
                    break;
                }
            }
        }       
    }

    public void ReloadWeapon()
    {
        Debug.Log("Reloading...");

        if (weaponAmmo >= (magazineSize - weaponMagazine))
        {
            weaponAmmo = weaponAmmo - (magazineSize - weaponMagazine);
            weaponMagazine = magazineSize;

        }
        else
        {
            weaponMagazine = weaponMagazine + weaponAmmo;
            weaponAmmo = 0;
        }

        if (weaponAmmo >= 0)
        {
            bulletText.text = weaponMagazine + "/" + weaponAmmo;

        }

    }

    IEnumerator PlayGunflash()
    {
        gunFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gunFlash.SetActive(false);
    }

    void OnDrawGizmosSelected()// spherecast debug
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(gunBarrel.transform.position + gunBarrel.transform.TransformDirection(Vector3.forward) * weaponSpeed * Time.deltaTime, bulletCaliber);
    }
}
