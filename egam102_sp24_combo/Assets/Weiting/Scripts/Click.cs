using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Click : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        // 查找并引用 GameManager 实例
        gameManager = FindObjectOfType<GameManager>();

        // 为每个数字按钮添加点击监听器
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            // 通过按钮的名称解析出对应的数字
            string buttonName = button.gameObject.name;
            int buttonNumber;
            if (int.TryParse(buttonName.Replace("Button ", ""), out buttonNumber))
            {
                button.onClick.AddListener(delegate { gameManager.CheckNumber(buttonNumber); });
            }
        }
    }
}
