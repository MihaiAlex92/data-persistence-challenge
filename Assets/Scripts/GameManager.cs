using System;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public string PlayerName;

    public string BestPlayerName;
    public int BestPlayerScore;

    private string _saveFilePath;
    private bool _loadedData = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        _saveFilePath = Application.persistentDataPath + "/challenge_savefile.json";
        Debug.Log($"Save file folder: {_saveFilePath}");
        LoadGameData();

        if(_loadedData)
        {
            GameObject.Find("Info").GetComponent<TMP_Text>().text = $"Best Score: {BestPlayerName} - {BestPlayerScore}";
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SaveGameData()
    {
        var saveData = new SaveGame() { PlayerName = BestPlayerName, Score = BestPlayerScore };
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(_saveFilePath, json);
        Debug.Log("Game Saved!");
    }

    public void LoadGameData()
    {
        if (!File.Exists(_saveFilePath))
        {
            return;
        }

        string json = File.ReadAllText(_saveFilePath);
        var savedData = JsonUtility.FromJson<SaveGame>(json);
        BestPlayerName = savedData.PlayerName;
        BestPlayerScore = savedData.Score;
        _loadedData = true;
        Debug.Log("Game Data Loaded!");

    }

    public bool IsSavedDataLoaded()
    {
        return _loadedData;
    }



    public void OnStartButtonClick()
    {
        PlayerName = GameObject.Find("PlayerNameInput").GetComponent<TMP_InputField>().text;
        SceneManager.LoadScene(1);
    }


    public void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
                        Application.Quit();

#endif
    }

    [Serializable]
    public class SaveGame
    {
        public string PlayerName;
        public int Score;
    }

}
