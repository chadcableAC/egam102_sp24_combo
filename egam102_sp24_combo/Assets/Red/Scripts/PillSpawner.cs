/*using System.Collections.Generic;
using UnityEngine;

public class PillSpawner : MonoBehaviour
{
    // 假设Pill类定义在MedicationManager中或在外部，但与MedicationManager关联
    public List<MedicationManager.Pill> pills; // 存储所有Pill信息的列表
    public Transform targetTransform; // 需要监视旋转的GameObject的Transform
    public Transform spawnPoint; // Pills生成的中心点位置
    //private float lastAngle = 0f; // 用于记录上一次检查的角度

    private float spawnRate = 0.5f; // 每0.5秒生成一个Prefab
    private float lastSpawnTime = 0.0f; // 记录上次生成Prefab的时间

    void Start()
    {
        
    }

    void Update()
    {
        CheckRotationAndSpawnPill();
        //CheckAndSpawnPills();
    }
    void CheckRotationAndSpawnPill()
    {
        // 获取当前Z轴旋转角度的绝对值，并确保它在0-360度之间
        float currentAngle = Mathf.Abs(targetTransform.eulerAngles.z % 360);
        // 检查旋转角度是否满足条件
        bool shouldSpawn = currentAngle > 90 && currentAngle < 270;

        if (shouldSpawn && Time.time - lastSpawnTime > spawnRate)
        {
            foreach (var pill in pills)
            {
                if (pill.currentInstances < pill.maxInstances)
                {
                    Instantiate(pill.prefab, spawnPoint.position, Quaternion.identity); // 生成Pill
                    pill.currentInstances++; // 更新当前已生成的实例数
                    lastSpawnTime = Time.time; // 更新上次生成Prefab的时间
                    break; // 成功生成一个Pill后退出循环
                }
            }
            
        }
    }
   *//* void CheckAndSpawnPills()
    {
        // 计算从上次到现在的角度变化量
        float currentAngle = targetTransform.eulerAngles.y;
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(lastAngle, currentAngle));

        if (angleDifference >= 90)
        {
            foreach (var pill in pills)
            {
                if (pill.currentInstances < pill.maxInstances)
                {
                    Instantiate(pill.prefab, spawnPoint.position, Quaternion.identity); // 生成Pill
                    pill.currentInstances++; // 更新当前已生成的实例数
                    lastAngle = currentAngle; // 更新角度以便下一次比较
                    break; // 成功生成一个Pill后退出循环
                }
            }
        }
    }*//*
}*/
