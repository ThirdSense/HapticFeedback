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
        player_pos.x += Random.Range(-0.2f, 0.2f);
        player_pos.y -= Random.Range(0.3f, 1.1f);
        player_pos.z += Random.Range(-0.2f, 0.2f);

        Vector3 direction = (player_pos - transform.position).normalized;
        rb.velocity = direction * speed;
    }




}
