using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject head;
    private Vector3 offset;

    void Start()
    {
        offset = head.transform.position - transform.position;
    }

	void Update()
    {
        transform.position = head.transform.position + offset;
    }
}
