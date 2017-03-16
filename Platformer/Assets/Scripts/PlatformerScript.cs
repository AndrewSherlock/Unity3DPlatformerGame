using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerScript : MonoBehaviour {

    public bool moveOnPlayerLand;
    public bool falls;

    public float fallDelay;
    WayPointSystem waypoint;
    Animator anim;


    // Use this for initialization
    void Start () {
		if(falls || moveOnPlayerLand)
        {
            waypoint = transform.parent.GetComponent<WayPointSystem>();
            anim = GetComponent<Animator>();
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && falls)
        {
            StartCoroutine(WaitToMakePlatformFall(fallDelay));
        } else if (collision.gameObject.tag == "Player" && moveOnPlayerLand)
        {
            waypoint.active = true;
            GameObject player = collision.gameObject;
            player.transform.parent = transform;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.parent = null;
    }

    IEnumerator WaitToMakePlatformFall(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("playerOnPlatform", true);
        StartCoroutine(RemoveObject(delay));
    }

    IEnumerator RemoveObject(float delay)
    {
        yield return new WaitForSeconds(delay + 3f);
        Destroy(gameObject);
    }
}
