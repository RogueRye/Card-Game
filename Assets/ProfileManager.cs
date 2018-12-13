using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileManager : MonoBehaviour {

    [SerializeField]
    InputField nameInput;
    [SerializeField]
    Transform loadBtnPanel;
    [SerializeField]
    Button loadBtnPrefab;

    private void Awake()
    {

    }

    public void CreateNewProfile()
    {
        ProfileData newData;
        if (nameInput == null || nameInput.text != string.Empty)
        {
            newData = new ProfileData("No name");
        }
        else {
            newData = new ProfileData(nameInput.text.ToString());
        }

        AddDefaultCollection(ref newData);
        SaveLoad.savedProfiles.Add(newData);
        SaveLoad.SaveGames();

        SaveLoad.SetCurrentProfile(newData);
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
            if (!newData.cardCollection.ContainsKey(newCard.cardName))
                newData.cardCollection.Add(newCard.cardName, newCard);


        }
    }

    public void ShowProfilesToLoad()
    {
        ResetPanel();
        loadBtnPanel.gameObject.SetActive(true);
        foreach(var data in SaveLoad.savedProfiles)
        {
            var newBtn = Instantiate(loadBtnPrefab, loadBtnPanel).GetComponent<Button>();
            newBtn.transform.localPosition = Vector3.zero;
            newBtn.onClick.AddListener(() => SaveLoad.SetCurrentProfile(data));
            newBtn.onClick.AddListener(() => loadBtnPanel.gameObject.SetActive(false));
            newBtn.GetComponentInChildren<TMP_Text>().text = data.profileName;
        }
    }

    void ResetPanel()
    {
        for (int i = 0; i < loadBtnPanel.childCount; i++)
        {
            Destroy(loadBtnPanel.GetChild(i).gameObject);
        }
    }

}
