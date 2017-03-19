using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardedScript : MonoBehaviour {

   
    public GameObject[] boards;
    public GameObject[] damagedBoards;

    public GameObject currentObject;
    int currentHit = -1;
    public Material mat; // temp

    PlayerUISystem playerUi;

    void ChangeObjectMesh()
    {
        GameObject newMesh = boards[currentHit];
        Vector3 positionToSpawn = currentObject.transform.position;
        Quaternion rotation = currentObject.transform.rotation;
        Debug.Log(positionToSpawn);
        Destroy(currentObject);
        SpawnDebris(positionToSpawn);
      
        GameObject go = Instantiate(newMesh, positionToSpawn, rotation);
        go.transform.parent = transform;
        go.GetComponent<Renderer>().material = mat;
        currentObject = go;
    }

    void SpawnDebris(Vector3 pos)
    {
        int randomChoice = Random.Range(0, damagedBoards.Length);
        foreach (Transform debrisPiece in damagedBoards[randomChoice].transform)
        {
            GameObject currentDebris = Instantiate(debrisPiece.gameObject, pos, Quaternion.identity, transform);
            Rigidbody rb = currentDebris.GetComponent<Rigidbody>();
            rb.AddRelativeForce(Vector3.down * 4f, ForceMode.Impulse);
            StartCoroutine(RemoveCollider(currentDebris.gameObject, rb));
        }
    }

    IEnumerator RemoveCollider(GameObject deb, Rigidbody rb)
    {
        Debug.Log("De-Activated");
        StartCoroutine(CleanUpDebris(deb));
        yield return new WaitForSeconds(3f);
        rb.useGravity = false;
        deb.GetComponent<Collider>().enabled = false;
    }

    IEnumerator CleanUpDebris(GameObject deb)
    {
        Renderer render = deb.GetComponent<Renderer>();
        float timeToFade = 4f;
        Color finalColor = new Color(render.material.color.r, render.material.color.b, render.material.color.g, 0);
        
        yield return new WaitForSeconds(5f);
        while (timeToFade > 0)
        {
            float amountToLerp = timeToFade / Time.deltaTime;
            render.material.color = Color.Lerp(render.material.color, finalColor, amountToLerp);
            timeToFade -= Time.deltaTime;
            yield return null;
        }
        Destroy(deb);
    }

    IEnumerator AddObjectDestructionDelay()
    {
        yield return new WaitForSeconds(8);
        Destroy(gameObject);
    }

    public void AddToDamage()
    {
        currentHit++;
        
        Debug.Log("Running");
        if(currentHit == 4)
        {
            Destroy(currentObject);
            StartCoroutine(AddObjectDestructionDelay());
        }
        else
        {
            ChangeObjectMesh();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerUi = other.GetComponent<PlayerUISystem>();
            playerUi.DisplayMessageToPlayer("Enter X to break boards");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerUi.MoveTextBack();
            playerUi = null;
        }
    }
}
