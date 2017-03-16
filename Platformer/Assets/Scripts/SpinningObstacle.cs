using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObstacle : MonoBehaviour {

    public float spinSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RotateObject();
        
    }

    void RotateObject()
    {
        transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
    }
}
