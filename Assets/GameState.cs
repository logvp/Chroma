using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // inspector
    public GameObject iPlayer;
    public Material iRedMat, iGreenMat, iBlueMat;

    private class GameStateImpl
    {
        internal GameObject player;
        internal Material redMat, greenMat, blueMat;

        internal GameStateImpl(GameObject player, Material redMat, Material greenMat, Material blueMat)
        {
            this.player = player;
            this.redMat = redMat;
            this.greenMat = greenMat;
            this.blueMat = blueMat;
        }
    }

    private static GameStateImpl instance;

    public static GameObject Player => instance.player;
    public static Material RedMat => instance.redMat;
    public static Material GreenMat => instance.greenMat;
    public static Material BlueMat => instance.blueMat;

    void Awake()
    {
        Debug.Assert(iPlayer != null);
        instance = new GameStateImpl(iPlayer, iRedMat, iGreenMat, iBlueMat);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
