using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class WayPointSystem : MonoBehaviour {

    public Vector3[] positionsToMoveTo;
    public bool isReversible;
    public float delay;
    public float moveSpeed;

    bool movingToNextPoint;
    bool movingBackwards;
    int nextPos = 1;

    public bool active;

	// Use this for initialization
	void Start () {
        movingToNextPoint = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (movingToNextPoint)
            {

                if (transform.position == positionsToMoveTo[nextPos]) // TEST MUST CHANGE
                {
                    movingToNextPoint = false;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, positionsToMoveTo[nextPos], 2f * Time.deltaTime);
                }
            }

            if (!movingToNextPoint)
            {
                if (nextPos == positionsToMoveTo.Length - 1)
                {
                    if (isReversible && transform.position == positionsToMoveTo[nextPos])
                    {
                        movingBackwards = true;
                        moveBackwards();
                    }
                    else if (transform.position == positionsToMoveTo[nextPos])
                    {
                        nextPos = 0;
                        StartCoroutine(MoveToPointDelay(delay));
                    }
                }

                if (movingBackwards && nextPos == 0)
                {
                    if (transform.position == positionsToMoveTo[nextPos])
                    {
                        movingBackwards = false;
                        moveForward();
                    }
                }

                if (!movingBackwards && transform.position == positionsToMoveTo[nextPos] && nextPos != positionsToMoveTo.Length - 1)
                {
                    moveForward();
                }

                if (movingBackwards && transform.position == positionsToMoveTo[nextPos] && nextPos != 0)
                {
                    moveBackwards();
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
        nextPos--;
        StartCoroutine(MoveToPointDelay(delay));
    }

    IEnumerator MoveToPointDelay(float delay)
    {
        movingToNextPoint = false;
        yield return new WaitForSeconds(delay);
        movingToNextPoint = true;
    }
}
