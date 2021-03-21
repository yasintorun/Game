using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class ILanguage
{
    public string name;
    public bool isTextMesh;
    public Text Text_UI;
    public TextMeshProUGUI text_mesh;
    //int locale; //0: TR, 1:En
    
}

public class Language : MonoBehaviour
{
    public ILanguage[] AllText;
    Dictionary<string, string> TR = new Dictionary<string, string>();
    Dictionary<string, string> EN = new Dictionary<string, string>();
    public void TR_initialize()
    {
        TR.Add("Language_settings", "Dil:");
        TR.Add("Volume_settings", "Ses:");
        TR.Add("game_reset", "Oyunu Sıfırla");
        TR.Add("game_reset_info", "Oyun Sıfırlanacaktır.\nOnaylıyor musun?");
        TR.Add("yes", "Evet");
        TR.Add("no", "Hayır");
        TR.Add("Level1Info", "Süre Bitmeden Tüm Meyveleri Topla");
        TR.Add("Level2Info", "Yön Tuşlarına ve Zıplama Tuşuna basarak\n Duvarlara Tırmanabilirsin");
    }

    public void EN_initialize()
    {

        EN.Add("Language_settings", "Language:");
        EN.Add("Volume_settings", "Volume:");
        EN.Add("game_reset", "Game Reset");
        EN.Add("game_reset_info", "The game will be reset.\n Do you approve?");
        EN.Add("yes", "Yes");
        EN.Add("no", "No");
        EN.Add("Level1Info", "Gather All Fruits Before Time Ends");
        EN.Add("Level2Info", "You Can Climb Walls\n by Pressing Arrow and Jump Key");

    }
    
    private void Start()
    {
        TR_initialize();
        EN_initialize();
        string lang = FindObjectOfType<GameManager>().GetLanguage();
        if(lang != null)
            SetLanguage(lang);
    }

    public void SetLanguage(string lang)
    {
        foreach (var t in AllText)
        {
            
            if (t.isTextMesh)
                t.text_mesh.text = lang == "TR" ? TR[t.name] : EN[t.name];
            else
                t.Text_UI.text = lang == "TR" ? TR[t.name] : EN[t.name];
        }
    }

    
}

