using UnityEngine;
using System.Collections;

    public class HitController : MonoBehaviour {

    public Transform projectile;

    private Vector3 spawn_pos;

    void Start()
    {
        SpawnProjectile();
    }

    void OntriggerEnter(Collision other)
    {
        Debug.Log("Hit");
        Destroy(other.gameObject);
        SpawnProjectile();            
    }

    void SpawnProjectile()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        float x = Random.Range(-20f, 20f);
        float y = Random.Range(3f, 10f);
        float z = Random.Range(3f, 30f);
        spawn_pos = new Vector3(x, y, z);

        Object.Instantiate(projectile, spawn_pos, Quaternion.identity);
    }
}

