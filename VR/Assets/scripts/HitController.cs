using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class HitController : MonoBehaviour {

    public Transform projectile;

    private Vector3 spawn_pos;

    private List<Vector3> motorList;


    void Start()
    {
        motorList = new List<Vector3> {
            new Vector3(2, 3, 4),
            new Vector3(3, 4, 5),
            new Vector3(4, 5, 6),
        };
        SpawnProjectile();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("trigger");
        Debug.Log(col.gameObject);
    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject);
        if(col.gameObject.CompareTag("projectile"))
        {
            Debug.Log("Hit");

            float min_dist = 9999f;
            int motorIndex = -1;

            foreach (ContactPoint contact in col.contacts)
            {
                int counter = 0;
                foreach (Vector3 vec in motorList)
                {
                    float dist = Vector3.Distance(contact.point, vec);
                    if (dist < min_dist)
                    {
                        min_dist = dist;
                        motorIndex = counter;
                    }
                    ++counter;
                }
            }

            Debug.Log("Activate Motor " + motorIndex.ToString() + " " + min_dist.ToString());
            // activeMotor(index, min_dist)
            Destroy(col.gameObject);
        }
    }

    void SpawnProjectile()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        float x = Random.Range(-10f, 10f);
        float y = Random.Range(3f, 4f);
        float z = Random.Range(3f, 30f);
        spawn_pos = new Vector3(x, y, z);

        Object.Instantiate(projectile, spawn_pos, Quaternion.identity);
    }
    void Update()
    {
    }


}


