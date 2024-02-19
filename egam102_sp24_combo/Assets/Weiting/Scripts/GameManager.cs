using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;


    private EgamMicrogameInstance microgameInstance;
    public Button restartButton; // 在Inspector中设置这个按钮


    public GameObject personPrefab; // 小人的Prefab引用
    private List<GameObject> allPersons = new List<GameObject>(); // 存储所有小人的列表
    public Transform busStop; // 巴士停靠点，需要在Unity编辑器的Inspector中设置

    private int remainingPersons; // 场景中剩余的Person数量

    void Start()
    {
        Debug.Log("GameManager Start");
        StartCoroutine(GameFlow());

        // 检查 microgameInstance 是否为 null
        microgameInstance = FindObjectOfType<EgamMicrogameInstance>();
        if (microgameInstance == null) Debug.Log("microgameInstance is null");

        // 检查 resultText 和 restartButton 是否为 null
        if (resultText == null) Debug.Log("resultText is null");
        if (restartButton == null) Debug.Log("restartButton is null");

        resultText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(RestartGame); // 为重启按钮添加事件监听器


    }

    IEnumerator GameFlow()
    {
        // 第一组
        yield return StartCoroutine(GenerateAndMoveGroup(Random.Range(1, 6)));
        // 第二组
        yield return StartCoroutine(GenerateAndMoveGroup(Random.Range(1, 6)));
        // 所有组生成和移动完毕，开始离开巴士的逻辑
        StartCoroutine(PersonsLeaveBusRandomly());
    }

    IEnumerator GenerateAndMoveGroup(int size)
    {
        for (int i = 0; i < size; i++)
        {
            Vector3 spawnPosition = new Vector3(4.416f, 0.834f, -5.842f); // 指定生成小人的位置
            GameObject person = Instantiate(personPrefab, spawnPosition, Quaternion.identity);
            allPersons.Add(person);
            // 移动小人到巴士停靠点的逻辑
            StartCoroutine(MovePersonToBus(person));
            yield return new WaitForSeconds(0.5f); // 生成下一个小人前的等待时间
        }
        // 等待这一组小人全部移动到巴士停靠点
        yield return new WaitForSeconds(5); // 根据实际情况调整等待时间
    }

    IEnumerator MovePersonToBus(GameObject person)
    {
        float step = 5f * Time.deltaTime; // 移动速度
        while (Vector3.Distance(person.transform.position, busStop.position) > 0.1f)
        {
            person.transform.position = Vector3.MoveTowards(person.transform.position, busStop.position, step);
            yield return null;
        }
        person.SetActive(false); // 假设小人“进入”巴士
    }

    IEnumerator PersonsLeaveBusRandomly()
    {
        yield return new WaitForSeconds(2); // 模拟等待时间

        int leaveCount = Random.Range(1, allPersons.Count + 1); // 随机确定离开巴士的小人数量
        for (int i = 0; i < leaveCount && allPersons.Count > 0; i++)
        {
            int personIndex = Random.Range(0, allPersons.Count);
            GameObject personToLeave = allPersons[personIndex];
            allPersons.RemoveAt(personIndex); // 从列表中移除

            personToLeave.SetActive(true);
            personToLeave.transform.position = busStop.position + new Vector3(-1, 0, 0);
            personToLeave.GetComponent<Rigidbody>().isKinematic = false;
            // 确保碰撞销毁脚本是激活的，这里假设你已经有了相应的脚本处理碰撞
            // personToLeave.GetComponent<DestroyOnCollision>().enabled = true;

            Vector3 wallPosition = new Vector3(-6.325f, 1.313f, -7.457f); // 墙的位置
            Vector3 toWallDirection = (wallPosition - personToLeave.transform.position).normalized; // 计算到墙的方向
            personToLeave.GetComponent<Rigidbody>().velocity = toWallDirection * 5f; // 向墙的方向移动

            yield return new WaitForSeconds(0.5f); // 等待一段时间再让下一个小人离开
        }
    }

    public void UpdateRemainingPersonsCount()
    {
        GameObject[] persons = GameObject.FindGameObjectsWithTag("person");
        remainingPersons = persons.Length;
        Debug.Log("Updated Remaining Persons Count: " + remainingPersons);
    }

    public void CheckNumber(int selectedNumber)
    {
        UpdateRemainingPersonsCount(); // 确保在检查之前更新数量
        Debug.Log($"Player selected: {selectedNumber}, Remaining Persons: {remainingPersons}");

        if (selectedNumber == remainingPersons)
        {
            Debug.Log("Win!");
            if (resultText != null)
            {
                resultText.text = "You Win!";
                resultText.gameObject.SetActive(true); // 显示结果文本

                microgameInstance.WinGame();
            }
            EndGame(true); // 调用EndGame方法并传递true表示玩家赢了
        }
        else
        {
            Debug.Log("Lose!");
            if (resultText != null)
            {
                resultText.text = "You Lose!";
                resultText.gameObject.SetActive(true); // 显示结果文本
            }
            EndGame(false); // 调用EndGame方法并传递false表示玩家输了
        }

        restartButton.gameObject.SetActive(true);
    }

    public void EndGame(bool isWin)
    {
        Debug.Log($"EndGame called with isWin: {isWin}");

        resultText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        if (isWin)
        {
            resultText.text = "You Win!";
            
        }
        else
        {
            resultText.text = "You Lose!";
        }
    }

    // 重启游戏的方法
    private void RestartGame()
    {
        // 重新加载当前场景来重启游戏
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
