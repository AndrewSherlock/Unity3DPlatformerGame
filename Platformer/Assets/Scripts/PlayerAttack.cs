using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public Collider axeSwingArea;
 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        SwingAxe();
	}

    void SwingAxe()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Collider[] hitObjects = Physics.OverlapBox(axeSwingArea.bounds.center, axeSwingArea.bounds.extents);

            foreach(Collider obj in hitObjects)
            {
                if(obj.tag == "HitObject")
                {
                    FindObjectThatWasHit(obj);
                }
            }
        }
    }

    void FindObjectThatWasHit(Collider obj)
    {
        switch (obj.name)
        {
            case "BoardsContainer": HandleBoardObj(obj.gameObject);
                break;
            default: Debug.Log(obj.name);
                break;
        }
    }

    void HandleBoardObj(GameObject boards)
    {
        BoardedScript boardScript = boards.GetComponent<BoardedScript>();
        boardScript.AddToDamage();
    }
}
