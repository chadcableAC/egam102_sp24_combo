using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kay
{ 
    public class Restart : MonoBehaviour
    {
        //restart scene when active
        public void restartGame()
        {
            //reset scene
            SceneManager.LoadScene("kay_jam3");
        }
    }
}