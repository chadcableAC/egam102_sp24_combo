using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // �����ײ�����Ƿ��ǡ�ǽ��
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject); 
        }
    }
}
