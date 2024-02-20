using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞对象是否是“墙”
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject); 
        }
    }
}
