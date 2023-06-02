using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed = 2f;
    public float maxHeight = 5f;
    public float minimalHeight = 0f;

    public GameObject goo;

    private bool isGooOnPlatform = false;

    public bool isItYmovement = false;
    public bool isItZmovement = false;

    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        //go up
        if (isGooOnPlatform && transform.position.y < maxHeight)
        {
            float newAxis = transform.position.y + speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, Mathf.Min(newAxis, maxHeight), transform.position.z);
        }
        
        //go down
        if (!isGooOnPlatform && transform.position.y > minimalHeight)
        {
            float newAxis = transform.position.y - speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, Mathf.Max(newAxis, minimalHeight), transform.position.z);
        }
        
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LiftUpWithDelay());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isGooOnPlatform = false;
        }
    }

    IEnumerator LiftUpWithDelay()
    {
        yield return new WaitForSeconds(2f); 

        isGooOnPlatform = true; 
    }


}
