using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Need to Update Functions:  loadChildObjects, repositionChildObjects, and Update.

Currently, the background is scrolling horizontally.
After update, the background will scroll vertiaclly.
*/


public class Scroll_Background : MonoBehaviour
{
    public GameObject[] levels;
    private Camera mainCamera;
    private Vector2 screenBounds;
    public float choke;
    public float scrollSpeed;

    void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        foreach (GameObject obj in levels)
        {
            loadChildObjects(obj);
        }
    }

    void loadChildObjects(GameObject obj)
    {
        if(obj == levels[0]) {
            choke = 0;
            }else {
            choke = -3;
        }
        float objectHeight = obj.GetComponent<SpriteRenderer>().bounds.size.y - choke;   //
        int childsNeeded = (int)Mathf.Ceil(screenBounds.y * 2 / objectHeight);   //
        GameObject clone = Instantiate(obj) as GameObject;
        for (int i = 0; i <= childsNeeded; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(obj.transform.position.x, objectHeight * i, obj.transform.position.z); //
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    void repositionChildObjects(GameObject obj)
    {
        if (obj == levels[0]) /// this is the hard way.  we create a new choke value according to the desired beachground object
        {
            choke = 0;
        }
        else
        {
            choke = -3;
        }
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectHeight = lastChild.GetComponent<SpriteRenderer>().bounds.extents.y - choke;  //
            if (transform.position.y + screenBounds.y > lastChild.transform.position.y + halfObjectHeight)   //
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x, lastChild.transform.position.y + halfObjectHeight * 2, lastChild.transform.position.z);  //
            }
            else if (transform.position.y - screenBounds.y < firstChild.transform.position.y - halfObjectHeight)  //
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x, firstChild.transform.position.y - halfObjectHeight * 2, firstChild.transform.position.z);  //
            } 
            ///(2) a little cleaner if background objects were child objects of the main background, but need to issolate the repeated objects due to a repositioning issue 
        }
    }
    void Update()
    {

        Vector3 velocity = Vector3.zero;
        Vector3 desiredPosition = transform.position + new Vector3(0, scrollSpeed, 0); //x
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.3f);
        transform.position = smoothPosition;

    }
    void LateUpdate()
    {
        foreach (GameObject obj in levels)
        {
            repositionChildObjects(obj);
        }
    }
}