using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

public class HitController : MonoBehaviour {

    public Transform projectile;
    public Camera player;
    public int dof;
    public int nNearestTriggered;
    public float spawnFreq; // Time to spawn 1 projectile

    private Vector3 spawn_pos;
    private List<Vector3> motorList;

    private float timeCounter;

    
    private float contactThresAngle = 7.5f;

    public string port;
    private SerialPort stream;

    struct MotorDistance
    {
        public int port;
        public float distance;
    }

    void sendMotor(int motor_num, int power, int time)
    {
        string s = "BUZZ " + motor_num.ToString() + " " + power.ToString() + " " + time.ToString() + "\r\n";

        stream.WriteLine(s);
        stream.BaseStream.Flush();
    }

    void Start()
    {
        if (port.Length == 0)
            port = "COM3";

        stream = new SerialPort(port, 9600);
        stream.ReadTimeout = 50;
        stream.Open();

        motorList = new List<Vector3> {
                        // x,  y,  z
            new Vector3(-0.1f, -0.3f, 0.2f), // 0 Top Left of Back 0
            new Vector3( 0.1f, -0.3f, 0.2f), // 1 Top Right of Back
            new Vector3( 0.5f, 0.1f, 0.0f), // 2 Wrist of left
            new Vector3( -0.4f, 0.1f, 0.2f), // 3 Front of left
            new Vector3( -0.4f, 0.1f, 0.0f), // 4 Wrist of right 
            new Vector3( 0.4f, 0.1f, 0.2f), // 5 Front of right
            new Vector3( 0.3f, 0.2f, 0.2f), // 6 mid right arm
            new Vector3( 0.1f, -0.3f, 0.2f), // 7 shoulder right
            new Vector3( 0.1f, -0.4f, 0.2f), // 8 right rib down
            new Vector3( 0.1f, -0.5f, 0.2f), // 9 right rib
            new Vector3( -0.1f, -0.5f, 0.2f), // 10 left rib
            new Vector3( -0.1f, -0.6f, 0.2f), // 11 left rib down
            new Vector3( -0.1f, -0.3f, 0.2f), // 12 Wrist of left 
            new Vector3( -0.3f, -0.2f, 0.2f), // 13 Front of left
            new Vector3( -0.1f, -0.6f, -0.2f), // 14 left rib down
            new Vector3( 0.1f, -0.4f, -0.2f) // 15 right rib down
        };

        timeCounter = 0f;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("projectile"))
        {
            Vector3 player_pos = player.transform.position;
            List<MotorDistance> distances = new List<MotorDistance>();

            Debug.Log(col.contacts.Length);

            foreach (ContactPoint contact in col.contacts)
            {
                int counter = 0;
                Vector3 contactPoint = contact.point - player_pos;

                bool isFront = (contactPoint.z >= 0.1f);
                foreach (Vector3 vec in motorList)
                {
                    float dist = Vector3.Distance(contactPoint, vec);

                    MotorDistance md = new MotorDistance();
                    md.port = counter;
                    md.distance = dist;

                    distances.Add(md);
                    ++counter;
                }

                distances.OrderBy(d => d.distance);
                
                for (int i = 0; i < nNearestTriggered && i < distances.Count; i++)
                {
                    MotorDistance md = distances[i];
                    sendMotor(md.port, 50, 1000);
                }
                
            }
            Destroy(col.gameObject);
        }
    }

    void SpawnProjectile()
    {
        Vector3 player_pos = player.transform.position;
        float deg = 1f * Mathf.PI * Random.Range(-100f, 100f) / 100f;
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


