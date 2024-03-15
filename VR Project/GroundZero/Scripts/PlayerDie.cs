using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerDie : MonoBehaviour
{
    [SerializeField] GameObject teleportGo;
    [SerializeField] Canvas teleportCanvas;
    [SerializeField] Canvas dieCanvas;
    [SerializeField] GameObject[] arrParticlesGo;
    private bool isDie;

    public void PlayerDeath()
    {
        Debug.Log("Player Die!");
        this.isDie = true;
        this.arrParticlesGo[0].SetActive(true);
        StartCoroutine(this.CoChangeColor());
    }

    public void PlayerWin()
    {
        Debug.Log("Player win!");
        this.isDie = false;
        this.arrParticlesGo[1].SetActive(true);
        this.dieCanvas.GetComponentInChildren<Image>().color = new Color32(26, 26, 77, 255);
        StartCoroutine(this.CoChangeColor());
    }

    private IEnumerator CoChangeColor()
    {
        this.teleportGo.SetActive(true);
        var images = this.teleportCanvas.GetComponentsInChildren<Image>();
    
        int countTime = 0;
        while (countTime < 12)
        {
            if (countTime % 2 == 0)
            {
                for (int i = 0; i < images.Length; i++)
                {
                    if (this.isDie)
                        images[i].color = new Color32(77, 26, 26, 255);//to red
                    else
                        images[i].color = new Color32(26, 26, 77, 255);
                }
                this.dieCanvas.gameObject.SetActive(true);
            }
            else
            {
                for (int i = 0; i < images.Length; i++)
                {
                    images[i].color = new Color32(67, 67, 67, 255);//to black
                    this.dieCanvas.gameObject.SetActive(false);
                }
            }
            yield return new WaitForSeconds(0.25f);
            countTime++;

        }
        yield return new WaitForSeconds(0.1f);
        Debug.Log("fade Out");
        this.dieCanvas.gameObject.SetActive(true);
        var image = this.dieCanvas.GetComponentInChildren<Image>();
        image.color = new Color32(0, 0, 0, 0);
        var color = image.color;
        while (true)
        {//fade out
            Debug.Log(color.a);
            color.a += 0.01f;
            image.color = color;
            if (color.a >= 1)
            {
                if (isDie)
                    SceneManager.LoadScene("GameOverScene");
                else
                    SceneManager.LoadScene("GameClearScene");
                break;

            }
            yield return null;
        }

    }
}

