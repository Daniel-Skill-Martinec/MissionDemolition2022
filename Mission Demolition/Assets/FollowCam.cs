using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Dynamic")]
    public float camZ;

    private void Awake()
    {
        camZ = this.transform.position.z;
    }
    
    private void FixedUpdate()
    {
        if (POI == null) return;
        //get position of the POI
        Vector3 destination = POI.transform.position;
        destination.z = camZ;
        transform.position = destination;
    }
}
