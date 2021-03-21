using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class GameData
{
    public int level;
    [Min(0)]
    public int CharacterIndex;
    public string language;
    /*
     *Musk dude -> 0,
     *Ninja Frog -> 1,
     *Pink Man -> 2,
     *Virtual Guy -> 3
     */

    public GameData(GameManager gm)
    {
        level = gm.GetLevel();
        CharacterIndex = gm.GetActiveCharacterIndex();
        language = gm.GetLanguage();
    }

}


public class GameManager : MonoBehaviour
{
    private int level = 1;
    public string language = "EN";
    private ReklamManager rm;

    private int character_index = 0;
    public float GameFinishTime;
    private float timer = 100f;
    public Text ui_TimerText;
    private GameObject player;

    [Header("Sahneler Arası Geçiş")]
    [Space]
    public Animator transition;
    public float transitionTime = 1f;

    [Header("UI For Home Scene")]
    [Space]
    public GameObject homeCanvas;
    public GameObject levelCanvas;
    public GameObject shopWindow;
    public Slider slider;

    [Space]
    public Transform Characters;
    public GameObject pausePanel;

    GameObject[] fruit;
    public static int fruitCounter = 0;

    AudioManager audio;

    

    void Awake()
    {
        audio = FindObjectOfType<AudioManager>();
        rm = FindObjectOfType<ReklamManager>();
        level = 1;
        GameData data = SaveSystem.LoadGameData();
        if (data != null)
        {
            level = data.level;
            character_index = data.CharacterIndex;
            language = data.language;
        }
        else
        {
            language = "EN";
            character_index = 0;
            SaveSystem.SaveGameData(this);
        }
        player = Characters.GetChild(character_index).gameObject;
        player.SetActive(true);



        if (levelCanvas != null) // ana menu için.
        {
            Debug.Log(level);
            Button[] btn = levelCanvas.GetComponentsInChildren<Button>();
            for (int i = level; i < btn.Length; i++)
            {
                btn[i].GetComponent<Image>().color = Color.grey;
                btn[i].enabled = false;
            }
        }
        if (slider != null)
            slider.value = audio.value;
        
        if (ui_TimerText == null) return;
        timer = GameFinishTime;
        ui_TimerText.text = GameFinishTime.ToString();

        fruit = GameObject.FindGameObjectsWithTag("Fruit");
        fruitCounter = fruit.Length;

    }

    internal void setCharacterIndex(int index)
    {
        character_index = index;
    }
    bool loadad = false;
    void Update()
    {
        if (ui_TimerText == null || fruit == null) return; // bu kod sadece ana menude çalışacak.
        timer -= Time.deltaTime;

        if(timer >= 0)
        {
            if(fruitCounter <= 0)
            {

                level++;
                GameData data = SaveSystem.LoadGameData();
                if((data != null && data.level < level) || data == null)
                    SaveSystem.SaveGameData(this);
                LoadNextLevel();
                ui_TimerText = null;
                return;
            }
            int t = (int)timer;
            ui_TimerText.text = t.ToString();
        } 
        else
        {
            RestartScene();
        }

        if(player.transform.position.y <= -10)
        {
            RestartScene();
            
            
        }
    }

   /* private void OnApplicationQuit()
    {

        if(SaveSystem.LoadGameData().level < level)
            SaveSystem.SaveGameData(this);
    }*/

    public void Destroy2AnimationFinish(GameObject obj)
    {
        Destroy(obj);
    }

    public void RestartScene()
    {
        Time.timeScale = 1; // pause panel için.
        //Debug.Log(SceneManager.GetSceneAt(0).name);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public static void FruitCounter(int fruit = -1)
    {
        if (fruit < 0)
            fruitCounter--;
        else
            fruitCounter = fruit;
    }

    private void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private int startLevelIndex = 1;
    public void StartGame()
    {
        GameData data = SaveSystem.LoadGameData();
        if (data != null)
            level = data.level;
        StartCoroutine(LoadLevel(level));
    }

   IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("start");

        yield return new WaitForSeconds(transitionTime);
        if (rm) rm.reklamSayac--;
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadLevelButton(int level)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadLevel(level));
    }


    public void ShowLevelCanvas()
    {
        homeCanvas.SetActive(false);
        levelCanvas.SetActive(true);
    }



    public int GetLevel()
    {
        return level;
    }

    public void ResetGame()
    {
        level = 1;
        character_index = 0;
        GameData data = SaveSystem.LoadGameData();
        if (data != null)
            SaveSystem.SaveGameData(this);
        else
            Debug.LogError("Hata! Oyun sıfırlanamadı.");
    }

    public void ShopButtonEvent()
    {
        GameData data = SaveSystem.LoadGameData();
        if (data.CharacterIndex == 0)
            shopWindow.transform.GetComponentInChildren<Text>().text = "Seçildi";

        shopWindow.SetActive(true);
        homeCanvas.SetActive(false);
    }

    public void BackHomeWindow()
    {
        homeCanvas.SetActive(true);
        //shopWindow.SetActive(false);
        //shopWindow.GetComponent<Animator>().Play("closeShopWindow");
    }

    /*public void ShopWindowAnimator()
    {
        shopWindow.SetActive(false);
    }*/
    

    public void LevelCanvasToBackHome()
    {
        Button[] btn = levelCanvas.GetComponentsInChildren<Button>();
        
        foreach (var b in btn)
        {
            if (b.name == "back button") continue;
            b.GetComponent<Animator>().Play("button_close");
        }



    }

    internal int GetActiveCharacterIndex()
    {
        return character_index;
    }

    public void ChangeCharacter(int index)
    {

        //Characters.GetChild(character_index).gameObject.SetActive(true);
        //for kullanılmazsa ilk karekter seçildiginde önceki karekter false olmuyor.
        for (int i = 0; i<Characters.childCount; i++)
        {
            Characters.GetChild(i).gameObject.SetActive(false);
        }
        Characters.GetChild(index).gameObject.SetActive(true);
    }

    public void PauseGame()
    {

        audio.Pause("SoundTrack");
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        audio.Play("SoundTrack");
        pausePanel.SetActive(false);
        Time.timeScale = 1;

    }

    public string GetLanguage()
    {
        /*if(language != null || language != "")
            return language;
        return "EN";*/
        return language;
    }

    public void ChangeLanguage(string lang)
    {
        //if (lang.Equals(language)) return;

        language = lang;
        SaveSystem.SaveGameData(this);

        FindObjectOfType<Language>().SetLanguage(lang);
    }

    public void VolumeSetting(Slider s)
    {
        Debug.Log("adasd");
        audio.Volume(s);
    }

}
