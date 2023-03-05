using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �����, ��������������� ��� ���������� ����������� � ������� ���� ����.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    // ���� ���� ���������� ���.
    static public List<GameSession> gameSessions;
    // -1 - ������ ����� ����. 0-n - ��������� ����������� ����.
    static public int sessionToStart;
    // ������� �������.
    static public GameSession gameSessionToCreate;

    [SerializeField] Button loadSessionButton;

    [SerializeField] Canvas newGameUI;
    [SerializeField] TMP_Text nameForTheNewSession;

    [SerializeField] Canvas loadGameUI;
    [SerializeField] GameObject content;
    List<SavedSessionItem> savedSessionItems;
    [SerializeField] SavedSessionItem item = null;

    [SerializeField] HelpUI helpUI;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            // �������������� ������.
            if (File.Exists(Application.persistentDataPath + "/savedSessions.eofs"))
            {
                string json = File.ReadAllText(Application.persistentDataPath + "/savedSessions.eofs", Encoding.UTF8);
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                gameSessions = JsonConvert.DeserializeObject<List<GameSession>>(json, settings);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"������ �������� ����! {e.Message}");
        }

        // ���� ������ ���, ������ ��� ������ ������ ��� ��������� ������. � ����� ������, ����� ��� � ������ ���.
        if (gameSessions == null || gameSessions.Count == 0)
        {
            gameSessions = new List<GameSession>();
            sessionToStart = -1;
            //loadSessionButton.gameObject.SetActive(false);
            loadSessionButton.interactable = false;
            ShowHelp();
        }
    }

    public void DeleteListItem(int id)
    {
        gameSessions.RemoveAt(id);
        RefreshList();
    }

    public void StartNewSession()
    {
        // TODO: ����� ������� ����� GameRules.

        // �������� ����� ������ � �������� �.
        gameSessionToCreate = new GameSession(0);
        sessionToStart = -1;

        // ��������, ���� �� ��������.
        if (string.IsNullOrEmpty(nameForTheNewSession.text) || nameForTheNewSession.text.StartsWith("\u200B"))
        {
            gameSessionToCreate.SessionName = "Save - " + gameSessionToCreate.LastPlayingTime;
        }
        else
        {
            gameSessionToCreate.SessionName = nameForTheNewSession.text;
        }

        // �������� ����� ����.
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void CloseLoadSessionUI()
    {
        loadGameUI.gameObject.SetActive(false);
    }

    public void OpenLoadingSessionUI()
    {
        // ������� �������� ������, ��� ����� ������� ���� ��� ��������.
        SavedSessionItem.MainMenuManagerVar = this;
        loadGameUI.gameObject.SetActive(true);
        RefreshList();
    }

    void RefreshList()
    {
        // ������� ����.
        SavedSessionItem[] childs = GameObject.Find("Content").GetComponentsInChildren<SavedSessionItem>();
        for (int i = 0; i < childs.Length; i++)
        {
            Destroy(childs[i].gameObject);
        }

        // �������� ����.
        for (int i = 0; i < gameSessions.Count; i++)
        {
            GameObject item = Instantiate(this.item.gameObject);
            SavedSessionItem savedSessionItem = item.GetComponent<SavedSessionItem>();
            //item.transform.parent = content.transform;
            item.transform.SetParent(content.transform);
            savedSessionItem.CreateItem(i);
        }
    }

    public void ShowHelp()
    {
        // ������� �������.
        helpUI.Open();
    }

    public static void SaveSessions()
    {
        // Debug.Log("��������� ������� ������: " + gameSessions.Count);

        try
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string jsonString = JsonConvert.SerializeObject(gameSessions, settings);
            File.WriteAllText(Application.persistentDataPath + "/savedSessions.eofs", jsonString, Encoding.UTF8);

            Debug.Log(jsonString);
            Debug.Log("���������� ���������.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"������ ���������� ����! {e.Message}");
        }
    }

    public void ExitGame()
    {
        // ����������� ������ � ������ �� ����.
        SaveSessions();
        Application.Quit();
    }
}
