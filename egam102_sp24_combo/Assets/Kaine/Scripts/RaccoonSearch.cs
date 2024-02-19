using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonSearch : MonoBehaviour
{
    public GameObject coonPlayer, dumpster, trashCan;
    public Animator coonAnim;
    public List<DumpItem> dumpList ;
    public DumpItem dumpScript;
    public bool canSearch, hasSearched;


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(EgamInput.GetKey(EgamInput.Key.Action) && canSearch == true && hasSearched == false)
        {
            coonAnim.SetBool("isSearching", true);
            for (int i = 0; i < dumpList.Count; i++)
            {
                dumpList[i].TrashLauncher();
                //Debug.Log("shoot trash");
                hasSearched = true;
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == coonPlayer)
        {
            canSearch = true;
            //coonAnim.SetBool("isSearching", true);
            //Debug.Log("true");
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == coonPlayer)
        {
            canSearch = false;
            coonAnim.SetBool("isSearching", false);
            //Debug.Log("false");
        }
    }
}
