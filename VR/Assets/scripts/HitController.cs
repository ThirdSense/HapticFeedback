using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class HitController : MonoBehaviour {

    public Transform projectile;
    public Camera player;
    public int dof;

    private Vector3 spawn_pos;
    private List<Vector3> motorList;

    private float timeCounter;

    private float spawnFreq = 2.0f; // Time to spawn 1 projectile
    private float contactThresAngle = 7.5f;

    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        motorList = new List<Vector3> {
            new Vector3(2, 3, 4),
            new Vector3(3, 4, 5),
            new Vector3(4, 5, 6),
        };

        timeCounter = 0f;

    }
   
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("projectile"))
        {
            float min_dist = 9999f;
            int motorIndex = -1;

            Vector3 player_pos = player.transform.position;

            foreach (ContactPoint contact in col.contacts)
            {
                float angle = Vector3.Angle(contact.point, player_pos); //  Mathf.Atan2(player_pos.z - contact.point.z, player_pos.x - contact.point.x);
                if (angle > contactThresAngle)
                {
                    Debug.Log("HIT" + angle.ToString() + " " + contact.point + " " + player_pos);
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
                
            }

            // activeMotor(index, min_dist) [Note you can place this in a couple places depending on wanted behaviour]
            Destroy(col.gameObject);
        }
    }

    void SpawnProjectile()
    {
        Vector3 player_pos = player.transform.position;
        float deg = 0.25f * Mathf.PI * Random.Range(-100f, 100f) / 100f;
        float hypo = 5f + Random.Range(0f, 20f);
        float x = player_pos.x + Mathf.Sin(deg) * hypo;
        float y = Random.Range(3f, 4f);
        float z = player_pos.z + Mathf.Cos(deg) * hypo;

        spawn_pos = new Vector3(x, y, z);

        Object.Instantiate(projectile, spawn_pos, Quaternion.identity);
    }

    void Update()
    {
        timeCounter += Time.deltaTime;
        if(timeCounter > spawnFreq)
        {
            SpawnProjectile();
            timeCounter = 0f;
        }
    }
}


