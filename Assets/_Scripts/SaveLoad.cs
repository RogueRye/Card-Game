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
}

[System.Serializable]
public class ProfileData
{
    public static ProfileData currentProfile;

    public List<string> deckCardIndexes = new List<string>();
    public List<CardData> cardCollection = new List<CardData>();
}

[System.Serializable]
public class CardData
{
    public string cardName;
    public int cardLVL;
    public int cardAmount;

    public CardData(Card card)
    {
        cardName = card.cardName;
        cardLVL = 1;
        cardAmount = 1;
    }
}
