using UnityEngine;
using System.Collections;

public class projectileControl : MonoBehaviour {

    public float speed;

    private Camera player;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = Camera.main;

        Vector3 player_pos = player.transform.position;
        player_pos.x += Random.Range(-0.4f, 0.4f);
        player_pos.y -= Random.Range(0.3f, 1.1f);
        player_pos.z += Random.Range(-0.2f, 0.2f);

        Vector3 direction = (player_pos - transform.position).normalized;

        transform.Rotate(direction * 360);
        rb.velocity = direction * speed;
    }
}
