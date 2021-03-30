using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using System;

public class ReklamManager : MonoBehaviour
{
    private InterstitialAd interAD;


    private string interID = "Reklamlarrr";
    public int reklamSayac = 3;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(reklamSayac <= 0)
        {
            reklamSayac = 3;
            ReklamGoster();
        }

    }
    static GameObject go;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        if (go == null)
            go = gameObject;
        else
            Destroy(gameObject);

        MobileAds.Initialize(reklamver => { });
        GecisliReklam();
    }

    public void GecisliReklam()
    {
        interAD = new InterstitialAd(interID);

        interAD.OnAdClosed += ReklamKapat;

        AdRequest request = new AdRequest.Builder().Build();
        interAD.LoadAd(request);


    }

    private void ReklamKapat(object sender, EventArgs e)
    {
        FindObjectOfType<AudioManager>().Play("SoundTrack");
        this.GecisliReklam();
    }

    public void ReklamGoster()
    {
        if (interAD.IsLoaded())
        {
            try
            {
                FindObjectOfType<AudioManager>().Stop("SoundTrack");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            interAD.Show();
        }
        else
        {
            Debug.Log("Reklam yüklenmedi");
        }
    }
    

}
