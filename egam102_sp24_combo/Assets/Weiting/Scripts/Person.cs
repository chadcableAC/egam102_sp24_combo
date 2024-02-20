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
            // 计算目标方向
            Vector3 targetPosition = new Vector3(0.09f, 0.827f, -5.783f); // Bus位置
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            // 移动小人
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // 检测是否到达Bus位置（简单距离检测）
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f) // 到达阈值
            {
                isMoving = false; // 停止移动
            }
        }
    }
}
