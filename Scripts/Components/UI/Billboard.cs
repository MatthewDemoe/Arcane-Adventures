using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerCam.transform);
    }
}
