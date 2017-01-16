using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    public float ballInitialVelocity = 600f;
    private bool ballInPlay = false;
    private Rigidbody rb;

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((Input.GetButtonDown("Fire1") || KinectManager.instance.isFire) && ballInPlay == false)
        {
            transform.parent = null;
            ballInPlay = true;
            rb.isKinematic = false;
            rb.AddForce(new Vector3(ballInitialVelocity, ballInitialVelocity, 0));
            KinectManager.instance.isFire = false;
        }
    }
}
