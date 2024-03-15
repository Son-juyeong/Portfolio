using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    public static BulletGenerator Instance;
    [SerializeField]
    private GameObject bulletPrefab;
    private void Awake()
    {
        Instance = this;
    }

    public GameObject GenerateBullet(Transform parent)
    {
        GameObject bulletGo = Instantiate(bulletPrefab, parent);
        bulletGo.transform.SetParent(null);
        return bulletGo;
    }
}
