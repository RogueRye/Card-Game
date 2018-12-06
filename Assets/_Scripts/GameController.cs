using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Player playerOne;
    public Player playerTwo;
    public GameObject winnerDisplayGO;
    public TMP_Text winnerDisplay;
    bool gameOver = false;
    private void Start()
    {
        winnerDisplayGO.SetActive(false);
    }

    private void Update()
    {
        if (!gameOver)
        {
            if (playerOne.GetLifePoints() <= 0)
            {
                StartCoroutine(EndGame(playerTwo));
            }
            else if (playerTwo.GetLifePoints() <= 0)
            {
                StartCoroutine(EndGame(playerOne));
            }
        }
    }


    IEnumerator EndGame(Player winner)
    {
        gameOver = true;
        playerOne.enabled = false;
        playerTwo.enabled = false;
        winnerDisplay.text = "";
        winnerDisplayGO.SetActive(true);
        string target = string.Format("{0} wins!", winner.playerName);
        for (int i = 0; i < target.Length; i++)
        {
            winnerDisplay.text += target[i];
            yield return new WaitForSeconds(.2f);
        }
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene(0);
    }

}

public enum CardTypes
{
    Creature,
    Spell

}
