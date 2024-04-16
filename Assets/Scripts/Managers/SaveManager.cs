using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public SaveManager Instance { get; private set; }
    [SerializeField] private string saveFolderPath;

    private void Awake() {
        //We check if there is already a Singleton of SaveManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public static void SaveData<T>(T dataToSave, string filename) {
        string data = JsonUtility.ToJson(dataToSave);
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/{filename}.json", data);
        Debug.Log(">>> File saved!");
    }

    public static bool TryReadSavedData<T>(string filename, out T savedData) {
        if(System.IO.File.Exists(Application.persistentDataPath + $"/{filename}.json")) {
            // File exists
            string savedSettings = System.IO.File.ReadAllText(Application.persistentDataPath + $"/{filename}.json");
            if (savedSettings != "" && savedSettings != null) {
                // We have collected saved settings
                savedData = JsonUtility.FromJson<T>(savedSettings);
                return true;
            }
        }
        // We have no saved settings or file does not exist
        savedData = default;
        return false;
    }

}

[System.Serializable]
public class SettingsData {

    // Post-processing
    public bool vignittePP;

    // Key-Bindings
    public string moveUpKey;
    public string moveLeftKey;
    public string moveDownKey;
    public string moveRightKey;
    public string interactKey;

    // Audio settings
    public float masterVolume;

    public SettingsData(bool vignittePP, string moveUpKey, string moveLeftKey, string moveDownKey, string moveRightKey, string interactKey, float masterVolume) {
        this.vignittePP = vignittePP;
        this.moveUpKey = moveUpKey;
        this.moveLeftKey = moveLeftKey;
        this.moveDownKey = moveDownKey;
        this.moveRightKey = moveRightKey;
        this.interactKey = interactKey;
        this.masterVolume = masterVolume;
    }

}

[System.Serializable]
public class GameSaveData {
    public int difficulty;
    public int level;
    public int score;

    public GameSaveData(int difficulty, int level, int score) {
        this.difficulty = difficulty;
        this.level = level;
        this.score = score;
    }
}
