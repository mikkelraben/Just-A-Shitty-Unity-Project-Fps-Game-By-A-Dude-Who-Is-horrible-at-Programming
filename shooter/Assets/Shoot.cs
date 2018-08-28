using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Put This On The Gun Nothing Else
/// </summary>
public class Shoot : MonoBehaviour
{

    RaycastHit Hit;
    bool Aiming, CanIHasBullet, PrimaryHeld;
    public float Damage = 25, RateOfFire = 10;
    public GameObject hole;
    public bool Auto, Single, Burst, Shotgun;
    public int AmmoInMagazine = 25, Ammo = 225, MagazineSize = 25, NumberOfshotInshell;
    float TimeBulletShot;
    public Text AmmoInMag, ModeText;
    bool ShotsBurst;
    int NumberShots;
    public string[] ModeNames;
    public Vector3 VectorOverride;
    // Mode 0:Safety 1:Single 2:Burst 3:Auto  4: Shotgunfire
    public int Mode = 3;
	private void Start()
	{
        ModeText.text = "Mode: " + ModeNames[Mode];
        AmmoInMag.text = (AmmoInMagazine + " / " + (Ammo + AmmoInMagazine));
	}
	// Update is called once per frame
	void Update()
    {

        //reload mechanics
        if (Input.GetKeyDown("r") && Ammo != 0)
        {
            Ammo = Ammo - 25 + AmmoInMagazine;
            AmmoInMagazine += MagazineSize - AmmoInMagazine;
            if(Ammo < 0)
            {
                AmmoInMagazine = AmmoInMagazine + Ammo;
                Ammo = 0;
            }
            AmmoInMag.text = (AmmoInMagazine + " / " + (Ammo + AmmoInMagazine));
        }

        //burst shoooting
        if(ShotsBurst && AmmoInMagazine != 0 && TimeBulletShot < Time.time && NumberShots < 3)
        {
            NumberShots++;
            if(NumberShots == 3)
            {
                ShotsBurst = false;
                NumberShots = 0;
            }
            fire(Aiming,false);
        }

        //switch modes
        if (Input.GetKeyDown("v"))
        {
            Mode++;
            if (Single == false && Mode == 1)
            {
                Mode = 2;
            }
            if (Burst == false && Mode == 2)
            {
                Mode = 3;
            }
            if (Auto == false && Mode == 3)
            {
                Mode = 4;
            }
            if (Shotgun == false && Mode == 4)
            {
                Mode = 0;
            }


            if (Mode > 4)
            {
                Mode = 0;
                ModeText.text = "Mode: " + ModeNames[Mode];
            }
            ModeText.text = "Mode: " + ModeNames[Mode];
        }

        if (Mode == 3 && AmmoInMagazine != 0 && TimeBulletShot < Time.time && PrimaryHeld)
        {
            CanIHasBullet = true;
        }
        else if (Mode == 2 && AmmoInMagazine != 0 && TimeBulletShot < Time.time && Input.GetButtonDown("Fire1"))
        {
            ShotsBurst = true;
        }
        else if (Mode == 1 && AmmoInMagazine != 0 && TimeBulletShot < Time.time && Input.GetButtonDown("Fire1"))
        {
            CanIHasBullet = true;
        }
        else if (Mode == 4 && AmmoInMagazine != 0 && TimeBulletShot < Time.time && Input.GetButtonDown("Fire1"))
        {
            fire(false, false);
            for (int i = 0; i < NumberOfshotInshell-1; i++)
            {
                fire(false,true);
            }
        }
        else
        {
            CanIHasBullet = false;
        }

        //Calls Fire when Primary button pressed
        if (Input.GetButton("Fire1"))
        {
            PrimaryHeld = true;
        }
        else
        {
            PrimaryHeld = false;
        }

        if (CanIHasBullet)
        {
            fire(Aiming,false);
        }
        //moves and detects if gun is aiming
        if (Input.GetButtonDown("Fire2"))
        {
            transform.Translate(new Vector3(0.25f, 0.05f, 0));
            Aiming = true;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            transform.Translate(new Vector3(-0.25f, -0.05f, 0));
            Aiming = false;
        }
    }
    //fires weapon 
    protected void fire(bool Aim, bool Shotgun)
    {
        if(Shotgun == false){
            AmmoInMagazine--;
            AmmoInMag.text = (AmmoInMagazine + " / " + (Ammo + AmmoInMagazine));
        }
        TimeBulletShot = 1 / RateOfFire + Time.time;
        Vector3 RandomDot;
        if (Aim == false)
        {
            RandomDot = Random.insideUnitSphere * 2;
        }
        else
        {
            RandomDot = Random.insideUnitSphere;
        }
        if (Physics.Raycast(transform.parent.transform.position, -transform.forward*100 + VectorOverride + RandomDot, out Hit, 1000f))
        {
            GameObject BulletHole = Instantiate(hole, Hit.point, Quaternion.LookRotation(Hit.normal, Vector3.up) * Quaternion.Euler(new Vector3(180, 0, 0)));
            BulletHole.transform.SetParent(Hit.transform);
            Enemy enemy = Hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }
            //create bullethole

            Destroy(BulletHole, 20f);
        }
        Debug.DrawRay(transform.parent.transform.position, -transform.forward*100 + VectorOverride + RandomDot,Color.red,10);
    }

}