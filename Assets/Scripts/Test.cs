using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Vector2 sdf = GameManager.Instance.mainCam.ScreenToWorldPoint(transform.position)
        Debug.Log(transform.position);
        Debug.Log(Camera.main.ScreenToWorldPoint(transform.position));
        Debug.Log(transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(transform.position);
            Debug.Log(Camera.main.ScreenToWorldPoint(transform.position));
            Debug.Log(transform.localPosition);
        }
    }
}
