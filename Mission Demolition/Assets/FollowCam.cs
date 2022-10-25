using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static private FollowCam S; //another private singleton
    static public GameObject POI;

    public enum eView { none, slingshot,castle, both};

    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    public GameObject viewBothGO;


    [Header("Dynamic")]
    public float camZ;
    public eView nextView = eView.slingshot;

    private void Awake()
    {
        S = this; //anytime we have a singleton, we have to hook it up
        camZ = this.transform.position.z;
    }
    
    private void FixedUpdate()
    {
        //if (POI == null) return;
        //get position of the POI
        Vector3 destination = Vector3.zero;

        if (POI != null)
        {
            // if it has a rigidbody , is i sleeping/not moving
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if ( (poiRigid != null) && (poiRigid.IsSleeping()))
            {
                POI = null;
            }
        }

        if (POI != null)
        {
            destination = POI.transform.position;
        }

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        transform.position = destination;
        // set te orthographic size of the camera to keep the ground in view of it
        Camera.main.orthographicSize = destination.y + 10;
    }

    public void SwitchView(eView newView)
    {
        if (newView == eView.none)
        {
            newView = nextView;
        }
        switch (newView)
        {
            case eView.slingshot:
                POI = null;
                nextView = eView.castle;
                break;
            case eView.castle:
                POI = MissionDemolition.GET_CASTLE();
                nextView = eView.both;
                break;
            case eView.both:
                POI = viewBothGO;
                nextView = eView.slingshot;
                break;

        }
    }

    public void SwitchView()
    {
        SwitchView(eView.none);
    }

    static public void SWITCH_VIEW(eView newView)
    {
        S.SwitchView(newView);
    }
}
