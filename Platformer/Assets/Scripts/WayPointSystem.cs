using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointSystem : MonoBehaviour {

    public Vector3[] positionsToMoveTo;
    public bool isReversible;
    public float delay;
    public float moveSpeed;

    bool movingToNextPoint;
    bool movingBackwards;
    int nextPos = 1;

	// Use this for initialization
	void Start () {
        movingToNextPoint = true;

    }
	
	// Update is called once per frame
	void Update () {
        if (movingToNextPoint)
        {
            if (!movingBackwards)
            {
                if(transform.position == positionsToMoveTo[nextPos])
                {
                    movingToNextPoint = false;
                }

                transform.position = Vector3.MoveTowards(transform.position,positionsToMoveTo[nextPos], moveSpeed * Time.deltaTime);

                if(nextPos == positionsToMoveTo.Length)
                {
                    if (isReversible)
                    {
                        moveBackwards();
                    } else
                    {
                        nextPos = 0;
                        moveForward();
                    }
                }
                else
                {
                    moveForward();
                }
            }
            else
            {
                if (transform.position == positionsToMoveTo[nextPos])
                {
                    movingToNextPoint = false;
                }
            }
        }
	}

    void moveForward() {
        nextPos++;
        StartCoroutine(MoveToPointDelay(delay));
    }

    void moveBackwards()
    {

    }

    IEnumerator MoveToPointDelay(float delay)
    {
        movingToNextPoint = false;
        yield return new WaitForSeconds(delay);
        movingToNextPoint = true;
    }
}
