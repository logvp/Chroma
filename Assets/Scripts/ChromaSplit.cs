using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaSplit : MonoBehaviour, Interactible
{
    public bool splitable = true;

    public Material rMat, gMat, bMat;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact(GameObject _src)
    {
        Debug.Log("I got clicked on!");
        GameObject a = Instantiate(gameObject, transform.position + transform.right, transform.rotation);
        GameObject b = Instantiate(gameObject, transform.position - transform.right, transform.rotation);
    }
}
