using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Click : MonoBehaviour
{
    private Weiting.GameManager gameManager;

    void Start()
    {
        // ���Ҳ����� GameManager ʵ��
        gameManager = FindObjectOfType<Weiting.GameManager>();

        // Ϊÿ�����ְ�ť���ӵ��������
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            // ͨ����ť�����ƽ�������Ӧ������
            string buttonName = button.gameObject.name;
            int buttonNumber;
            if (int.TryParse(buttonName.Replace("Button ", ""), out buttonNumber))
            {
                button.onClick.AddListener(delegate { gameManager.CheckNumber(buttonNumber); });
            }
        }
    }
}
