using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUISystem : MonoBehaviour {

    public Text playerPrompts;
    public Image promtImage;

    public Image[] healthImages;
    public Sprite[] healthIcons;
    public Sprite emptyHeart;

    public Text livesText;
    public Text gemsAmount;

    public Animator playerInformationAnimator;
    public Animator gameOverScreen;

    public void DisplayLifesText(int lives)
    {
        livesText.text = "x" + lives.ToString();
    }

    public void DisplayMessageToPlayer(string actionMessage)
    {

        playerPrompts.text = actionMessage;
        playerPrompts.enabled = true;

        //playerMessageAnim.SetBool("LeavingBoards", false);
        //playerMessageAnim.SetBool("InFrontOfSign", true);
    }

    public void MoveTextBack()
    {
       // playerMessageAnim.SetBool("InFrontOfSign", false);
        //playerMessageAnim.SetBool("LeavingBoards", true);
    }

    public void DisplayGems(int gems)
    {
        gemsAmount.text = "x" + gems.ToString();
    }

    public void DisplayPlayerHealth(float health, float maxHealth)
    { 
        for(int i = 0; i < maxHealth; i++)
        {
            healthImages[i].sprite = emptyHeart;
            for (float x = i; x < i + 1; x+= 0.25f)
            { 
                if (x > health && x % 1 == 0)
                {
                    healthImages[i].sprite = emptyHeart;
                    break;
                }
                else if (x > health)
                {
                    break;
                }

                if (x < health)
                {
                    int space = (int)((x - i) / 0.25f);
                    healthImages[i].sprite = healthIcons[ space ];
                }
            }  
        }
    }

    IEnumerator MovePlayerDetailsBack()
    {
        yield return new WaitForSeconds(5);
        playerInformationAnimator.SetBool("SeePlayerInfo", false);
    }

    public void ShowPlayerDetails()
    {
        playerInformationAnimator.SetBool("SeePlayerInfo", true);
        StartCoroutine(MovePlayerDetailsBack());
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetBool("GameOver", true);
    }
}
