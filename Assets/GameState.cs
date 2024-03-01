using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // inspector
    public GameObject inspector_player;

    private class GameStateImpl
    {
        public GameObject player;

        internal GameStateImpl(GameObject player)
        {
            this.player = player;
        }
    }

    private static GameStateImpl instance;

    public static GameObject Player => instance.player;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(inspector_player != null);
        instance = new GameStateImpl(inspector_player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
