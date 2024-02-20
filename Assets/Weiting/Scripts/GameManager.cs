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
    public Button restartButton; // ��Inspector�����������ť


    public GameObject personPrefab; // С�˵�Prefab����
    private List<GameObject> allPersons = new List<GameObject>(); // �洢����С�˵��б�
    public Transform busStop; // ��ʿͣ���㣬��Ҫ��Unity�༭����Inspector������

    private int remainingPersons; // ������ʣ���Person����

    void Start()
    {
        Debug.Log("GameManager Start");
        StartCoroutine(GameFlow());

        // ��� microgameInstance �Ƿ�Ϊ null
        microgameInstance = FindObjectOfType<EgamMicrogameInstance>();
        if (microgameInstance == null) Debug.Log("microgameInstance is null");

        // ��� resultText �� restartButton �Ƿ�Ϊ null
        if (resultText == null) Debug.Log("resultText is null");
        if (restartButton == null) Debug.Log("restartButton is null");

        resultText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(RestartGame); // Ϊ������ť����¼�������


    }

    IEnumerator GameFlow()
    {
        // ��һ��
        yield return StartCoroutine(GenerateAndMoveGroup(Random.Range(1, 6)));
        // �ڶ���
        yield return StartCoroutine(GenerateAndMoveGroup(Random.Range(1, 6)));
        // ���������ɺ��ƶ���ϣ���ʼ�뿪��ʿ���߼�
        StartCoroutine(PersonsLeaveBusRandomly());
    }

    IEnumerator GenerateAndMoveGroup(int size)
    {
        for (int i = 0; i < size; i++)
        {
            Vector3 spawnPosition = new Vector3(4.416f, 0.834f, -5.842f); // ָ������С�˵�λ��
            GameObject person = Instantiate(personPrefab, spawnPosition, Quaternion.identity);
            allPersons.Add(person);
            // �ƶ�С�˵���ʿͣ������߼�
            StartCoroutine(MovePersonToBus(person));
            yield return new WaitForSeconds(0.5f); // ������һ��С��ǰ�ĵȴ�ʱ��
        }
        // �ȴ���һ��С��ȫ���ƶ�����ʿͣ����
        yield return new WaitForSeconds(5); // ����ʵ����������ȴ�ʱ��
    }

    IEnumerator MovePersonToBus(GameObject person)
    {
        float step = 5f * Time.deltaTime; // �ƶ��ٶ�
        while (Vector3.Distance(person.transform.position, busStop.position) > 0.1f)
        {
            person.transform.position = Vector3.MoveTowards(person.transform.position, busStop.position, step);
            yield return null;
        }
        person.SetActive(false); // ����С�ˡ����롱��ʿ
    }

    IEnumerator PersonsLeaveBusRandomly()
    {
        yield return new WaitForSeconds(2); // ģ��ȴ�ʱ��

        int leaveCount = Random.Range(1, allPersons.Count + 1); // ���ȷ���뿪��ʿ��С������
        for (int i = 0; i < leaveCount && allPersons.Count > 0; i++)
        {
            int personIndex = Random.Range(0, allPersons.Count);
            GameObject personToLeave = allPersons[personIndex];
            allPersons.RemoveAt(personIndex); // ���б����Ƴ�

            personToLeave.SetActive(true);
            personToLeave.transform.position = busStop.position + new Vector3(-1, 0, 0);
            personToLeave.GetComponent<Rigidbody>().isKinematic = false;
            // ȷ����ײ���ٽű��Ǽ���ģ�����������Ѿ�������Ӧ�Ľű�������ײ
            // personToLeave.GetComponent<DestroyOnCollision>().enabled = true;

            Vector3 wallPosition = new Vector3(-6.325f, 1.313f, -7.457f); // ǽ��λ��
            Vector3 toWallDirection = (wallPosition - personToLeave.transform.position).normalized; // ���㵽ǽ�ķ���
            personToLeave.GetComponent<Rigidbody>().velocity = toWallDirection * 5f; // ��ǽ�ķ����ƶ�

            yield return new WaitForSeconds(0.5f); // �ȴ�һ��ʱ��������һ��С���뿪
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
        UpdateRemainingPersonsCount(); // ȷ���ڼ��֮ǰ��������
        Debug.Log($"Player selected: {selectedNumber}, Remaining Persons: {remainingPersons}");

        if (selectedNumber == remainingPersons)
        {
            Debug.Log("Win!");
            if (resultText != null)
            {
                resultText.text = "You Win!";
                resultText.gameObject.SetActive(true); // ��ʾ����ı�

                microgameInstance.WinGame();
            }
            EndGame(true); // ����EndGame����������true��ʾ���Ӯ��
        }
        else
        {
            Debug.Log("Lose!");
            if (resultText != null)
            {
                resultText.text = "You Lose!";
                resultText.gameObject.SetActive(true); // ��ʾ����ı�
            }
            EndGame(false); // ����EndGame����������false��ʾ�������
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

    // ������Ϸ�ķ���
    private void RestartGame()
    {
        // ���¼��ص�ǰ������������Ϸ
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
