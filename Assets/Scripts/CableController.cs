using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableController : MonoBehaviour {

    public Transform tailContainer;
    public GameObject tailGO;
    public float speed=50f;

    private Rigidbody2D rigidbody2D;
    private Collider2D collider2D;
    private List<Transform> tail;
    private bool enteredBoard = false;
    private Vector3 lastTailSpawn;
    private float startTime = 0f;
    private float inverseSpeed;
    private bool pendingRotation = false;

	void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        tail = new List<Transform>();
        inverseSpeed = 1 / speed;
    }

	void Update () {

        if (Input.GetMouseButtonDown(0)) {
            pendingRotation = true;
            //transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z - 90f, Vector3.forward);
        }
        if(enteredBoard) {
            if(Time.time-startTime>=speed/100/*(transform.position-lastTailSpawn).magnitude>=collider2D.bounds.extents.y*/) {
                if (pendingRotation) {
                    transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z - 90f, Vector3.forward); ;
                    pendingRotation = false;
                }
                GameObject mTail = Instantiate(tailGO, transform.position,transform.rotation, tailContainer);
                tail.Add(mTail.transform);
                lastTailSpawn = transform.position;
                startTime = Time.time;

               
            }
        }

        transform.position += transform.up*inverseSpeed;
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (enteredBoard && collision.gameObject.CompareTag(Tags.border)) {
            GameObject mTail = Instantiate(tailGO, transform.position, transform.rotation, tailContainer);
            tail.Add(mTail.transform);
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(Tags.border)) {
            enteredBoard = true;
            startTime = Time.time;
            GameObject mTail = Instantiate(tailGO, transform.position, transform.rotation, tailContainer);
            tail.Add(mTail.transform);
            lastTailSpawn = transform.position;
        }
    }
}
