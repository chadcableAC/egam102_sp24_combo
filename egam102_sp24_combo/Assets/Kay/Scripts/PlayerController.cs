using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public GameObject up;
    public GameObject down;

    // animators for each object

    public Color activeColor;
    public Color inactiveColor;

    public bool leftHasBeenClicked = false;
    public bool rightHasBeenClicked = false;
    public bool upHasBeenClicked = false;
    public bool downHasBeenClicked = false;

    //public IEnumerator hasClickedTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && leftHasBeenClicked == false)
        {
            left.GetComponent<PolygonCollider2D>().enabled = true;
            leftHasBeenClicked = true;
            // change color
            // do animator for movement
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && rightHasBeenClicked == false)
        {
            right.GetComponent<PolygonCollider2D>().enabled = true;
            rightHasBeenClicked = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && upHasBeenClicked == false)
        {
            up.GetComponent<PolygonCollider2D>().enabled = true;
            upHasBeenClicked = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && downHasBeenClicked == false)
        {
            down.GetComponent<PolygonCollider2D>().enabled = true;
            downHasBeenClicked = true;
        }
    }

}
