using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10;
    public float range = 50f;
    public int maxAmmo = 30;
    public int currentMaxAmmo;
    public int currentAmmo;
    public float fireRate = 0.1f;
    public float smashRate = 1f;
    public float reloadTime = 1.5f;
    public float nextTimeToFire;
    public float nextTimeToSmash;
    public float normalZoom;
    public float aimZoom;
    public bool isReloading;
    public bool isAiming;
}