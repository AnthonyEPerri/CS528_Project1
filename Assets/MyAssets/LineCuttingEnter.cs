using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCuttingEnter : MonoBehaviour
{
    public GameObject LineCutterComment;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        LineCutterComment.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        LineCutterComment.SetActive(false);
    }
}
