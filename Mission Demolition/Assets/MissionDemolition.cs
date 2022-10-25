using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // private Singleton

    [Header("Inscribed")]
    public Text uitlevel;
    public Text uitshots;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // FollowCam Mode

    // Start is called before the first frame update
    void Start()
    {
        S = this;
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        if (castle != null )
        {
            Destroy(castle);
        }
        // destroy old projectiles
        Projectile.DESTROY_PROJECTILES();
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
        //zoom out to show both
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI()
    {
        uitlevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitshots.text = "Shots Taken: " + shotsTaken;

    }
    // Update is called once per frame
    void Update()
    {
        UpdateGUI();
        // check for level end
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            //zoom out to show both
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
            shotsTaken = 0;
        }
        StartLevel();
    }

    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }

    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }
}
