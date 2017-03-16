using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObstacle : MonoBehaviour {

    public float spinSpeed;

    public bool xSpin;
    public bool ySpin;
    public bool zSpin;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
        RotateObject();
    }

    void RotateObject()
    {
        if (zSpin)
        {
            transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
        }
        if (ySpin)
        {
            transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
        }
        if (xSpin)
        {
            transform.Rotate(Vector3.left * spinSpeed * Time.deltaTime);
        }
    }
}
