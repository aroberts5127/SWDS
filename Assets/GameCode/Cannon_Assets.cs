using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Cannon_Assets : MonoBehaviour {
    public Vector3 PlayerStart;

    public GameObject LaserPrefab;
    public GameObject BulletPrefab;
    public List<GameObject> EnemyPrefabList;
    public List<Sprite> EnemyHealthImageList;

    public List<GameObject> CannonList;

    public GameObject GunObject;
    public GameObject LaserObject;
    public List<GameObject> SingleShotBulletTypes;
    public List<GameObject> SpreadShotBulletTypes;

    public ItemDatabase Database;

    public Transform BulletParent;
    public Transform EnemyParent;
    public Transform PickupParent;
    public Transform WeaponParent;
    public List<Transform > BulletSpawn;

    public GameObject PulseBomb;
    public GameObject ImmuneBarrier;
    public GameObject LeftWall;
    public GameObject RightWall;

   [Header("UI Objects")]
    public Transform HealthDisplayParent;
    public Text HealthOverCountText;
    public Transform PointGainDisplayParent;
    public Transform BombGainDisplayParent;
    public Transform WeaponGainDisplayParent;

    public GameObject PointGainObj;
    public GameObject BombGainObj;
    public GameObject WeaponGainObj;

    [Header("Music & Sounds")]
    public AudioClip GunSound;
    public AudioClip EnemyDeathSound;
    public AudioClip PickupSound;
    public AudioSource BGMSource;
}
