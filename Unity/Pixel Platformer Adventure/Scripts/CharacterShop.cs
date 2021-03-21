using UnityEngine;
using UnityEngine.UI;

public class CharacterShop : MonoBehaviour
{
    [Header("Karekterler ve Fiyatı")]
    public Sprite[] CharacterImage;
    public int[] CharacterMinLevel;

    [Space]
    public GameObject RightOk, LeftOk;
    public Image mainCharacter;
    public Text sellCoinText;


    private int index = 0;

    string select, selected;
    
    private void OnEnable()
    {

        index = 0;
        mainCharacter.sprite = CharacterImage[0];
        RightOk.SetActive(true);
        GameData data = SaveSystem.LoadGameData();
        if (data.language == "TR")
        {
            select = "Seç";
            selected = "Seçildi";
        }
        else
        {
            select = "Select";
            selected = "Selected";
        }
        if (data.CharacterIndex == index)
            sellCoinText.text = selected;
        else
            sellCoinText.text = select;
    }


    void Update()
    {
        //İleri Geri butonların gözükme olayı.
        if (index == 0)
            LeftOk.SetActive(false);
        else if (index > 0 && index < CharacterImage.Length - 1)
        {
            LeftOk.SetActive(true);
            RightOk.SetActive(true);
        }
        else
            RightOk.SetActive(false);
    }

    /// <summary>
    /// Right -> 1,
    /// left -> -1
    /// </summary>
    public void OkButtonEvent(int id)
    {
        if (index == 0 && id == -1) return;
        if (index == CharacterImage.Length - 1 && id == 1) return;
        index += id;
        mainCharacter.sprite = CharacterImage[index];
        
        GameData data = SaveSystem.LoadGameData();
        sellCoinText.text = data.level < CharacterMinLevel[index] ? "Level: " + CharacterMinLevel[index].ToString() : select;
        if (data.CharacterIndex == index) sellCoinText.text = selected;
    }

    public void SellCharacter()
    {
        GameData data = SaveSystem.LoadGameData();
        if (data != null && data.level >= CharacterMinLevel[index])
        {
            var gm = FindObjectOfType<GameManager>();
            gm.ChangeCharacter(index);
            gm.setCharacterIndex(index);
            SaveSystem.SaveGameData(gm);
            sellCoinText.text = selected;
        }

    }

        
}
