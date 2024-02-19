using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isMoving = true;

    void Update()
    {
        if (isMoving)
        {
            // ����Ŀ�귽��
            Vector3 targetPosition = new Vector3(0.09f, 0.827f, -5.783f); // Busλ��
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            // �ƶ�С��
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // ����Ƿ񵽴�Busλ�ã��򵥾����⣩
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f) // ������ֵ
            {
                isMoving = false; // ֹͣ�ƶ�
            }
        }
    }
}
