using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileManager : MonoBehaviour
{

    [SerializeField]
    TMP_InputField nameInput;
    [SerializeField]
    Transform loadBtnPanel;
    [SerializeField]
    Button loadBtnPrefab;
    [SerializeField]
    TMP_Text nameDisplay;


    private void Start()
    {       
        SaveLoad.LoadGames();

        if (SaveLoad.savedProfiles.Count > 0)
            SaveLoad.SetCurrentProfile(SaveLoad.savedProfiles[0]);

        nameDisplay.text = ProfileData.currentProfile != null ? ProfileData.currentProfile.profileName : "None";
        
    }

    public void CreateNewProfile()
    {
        ProfileData newData;
        if (nameInput == null || nameInput.text == string.Empty)
        {
            newData = new ProfileData("No name");
        }
        else
        {
            newData = new ProfileData(nameInput.text.ToString());
        }

        AddDefaultCollection(ref newData);
        SaveLoad.savedProfiles.Add(newData);
        SaveLoad.SaveGames();
        SaveLoad.SetCurrentProfile(newData);
        DisplayCurrentProfileName();
    }

    void AddDefaultCollection(ref ProfileData newData)
    {
        var path = Application.dataPath + "/Resources/Cards/";
        DirectoryInfo dir = new DirectoryInfo(path);
        // Debug.Log(dir);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (var f in info)
        {
            var name = f.Name;
            var extension = f.Extension;
            if (extension == ".meta")
                continue;
            var result = name.Replace(extension, "");

            var newCard = Resources.Load("Cards/" + result, typeof(Card)) as Card;
            var newCardData = new CardData(newCard);
            if (!newData.cardCollection.ContainsKey(newCard.cardName))
                newData.cardCollection.Add(newCard.cardName, newCardData);

        }
    }

    public void ShowProfilesToLoad()
    {
        SaveLoad.SaveGames();
        SaveLoad.LoadGames();
        ResetPanel();
        loadBtnPanel.gameObject.SetActive(true);
        foreach (var data in SaveLoad.savedProfiles)
        {
            var newBtn = Instantiate(loadBtnPrefab, loadBtnPanel).GetComponent<Button>();
            newBtn.transform.localPosition = Vector3.zero;
            newBtn.onClick.AddListener(() => SaveLoad.SetCurrentProfile(data));
            newBtn.onClick.AddListener(() => loadBtnPanel.gameObject.SetActive(false));
            newBtn.onClick.AddListener(() => DisplayCurrentProfileName());
            newBtn.GetComponentInChildren<TMP_Text>().text = data.profileName;

            var deleteBtn = newBtn.transform.GetChild(0).GetComponent<Button>();
            deleteBtn.onClick.AddListener(() => SaveLoad.DeleteProfile(data));
            deleteBtn.onClick.AddListener(() => ShowProfilesToLoad());
        }
    }

    public void ResetPanel()
    {
        for (int i = 1; i < loadBtnPanel.childCount; i++)
        {
            Destroy(loadBtnPanel.GetChild(i).gameObject);
        }
    }

    void DisplayCurrentProfileName()
    {
        nameDisplay.text = ProfileData.currentProfile != null ? ProfileData.currentProfile.profileName : "No Profile Set";
    }

}
