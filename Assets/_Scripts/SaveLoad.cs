using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public static class SaveLoad {

    public static List<ProfileData> savedProfiles = new List<ProfileData>();

    public static void SaveGames()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/deckProfiles.gd");
        bf.Serialize(file, savedProfiles);
        file.Close();
    }

    public static void LoadGames()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/deckProfiles.gd", FileMode.Open);
            savedProfiles = (List<ProfileData>)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void SetCurrentProfile(ProfileData data)
    {
        ProfileData.currentProfile = data;
    }

    public static void DeleteProfile(ProfileData data)
    {
        if (savedProfiles.Contains(data))
        {
            savedProfiles.Remove(data);
        }
    }
}

[System.Serializable]
public class ProfileData
{
    public static ProfileData currentProfile;
    public string profileName = "Ryan";

    public ProfileData(string _name)
    {
        profileName = _name;
    }

    public List<string> deckCardNames = new List<string>();
    //public List<CardData> cardCollection = new List<CardData>();
    public Dictionary<string, Card> cardCollection = new Dictionary<string, Card>();
}

[System.Serializable]
public class CardData
{
    public string cardName;


    public CardData(Card card)
    {
        cardName = card.cardName;
    }
}
