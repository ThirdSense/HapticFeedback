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

        Vector3 direction = (player_pos - transform.position).normalized;
        rb.velocity = direction * speed;
    }




}
