using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // inspector
    public GameObject iPlayer;
    public GameObject iPlayerHead;
    public Material iRedMat, iGreenMat, iBlueMat;
    public Transform[] iCheckpoints;

    private class GameStateImpl
    {
        internal GameObject player, playerHead;
        internal Material redMat, greenMat, blueMat;
        internal Transform[] checkpoints;

        internal GameStateImpl(GameObject player, GameObject playerHead, Material redMat, Material greenMat, Material blueMat, Transform[] checkpoints)
        {
            this.player = player;
            this.playerHead = playerHead;
            this.redMat = redMat;
            this.greenMat = greenMat;
            this.blueMat = blueMat;
            this.checkpoints = checkpoints;
        }
    }

    private static GameStateImpl instance;

    public static bool isPaused;
    public static GameObject Player => instance.player;
    public static GameObject PlayerHead => instance.playerHead;
    public static Material RedMat => instance.redMat;
    public static Material GreenMat => instance.greenMat;
    public static Material BlueMat => instance.blueMat;
    public static Transform[] Checkpoints => instance.checkpoints;

    void Awake()
    {
        Debug.Assert(iPlayer != null);
        instance = new GameStateImpl(iPlayer, iPlayerHead, iRedMat, iGreenMat, iBlueMat, iCheckpoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
