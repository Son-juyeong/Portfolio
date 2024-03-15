using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearMain : MonoBehaviour
{

    [SerializeField] GameObject continue_txt;

    //..공통
    [SerializeField] GameObject result;
    [SerializeField] GameObject[] gameResultObjects;
    [SerializeField] GameObject[] gameResultLists;
    [SerializeField] GameObject score;

    //..텍스트
    [SerializeField] TMP_Text enemyKills_txt;
    [SerializeField] TMP_Text blockedBullets_txt;
    [SerializeField] TMP_Text totalDamge_txt;
    [SerializeField] TMP_Text itemEquiped_txt;
    [SerializeField] TMP_Text score_txt;

    //..게임오버
    [SerializeField] GameObject sphere_gameover;
    [SerializeField] TMP_Text clear_txt;
    private int count = 0;
    private bool isContinue;
    [SerializeField] GetUser getUser;
    void Start()
    {
        this.isContinue = false;

        this.result.SetActive(true);
        this.GameOver();

        this.enemyKills_txt.text = InfoManager.Instance.kills.ToString();
        this.blockedBullets_txt.text = InfoManager.Instance.blockedBullet.ToString();
        this.itemEquiped_txt.text = InfoManager.Instance.itemEquiped.ToString();
        this.totalDamge_txt.text = InfoManager.Instance.totalDamage.ToString();
        this.score_txt.text = InfoManager.Instance.score.ToString();
    }

    private void Update()
    {
        foreach (GameObject gameObject in gameResultObjects)
        {
            gameObject.transform.Rotate(new Vector3(0, 0.8f, 0) * 180 * Time.deltaTime);
        }

        if (this.isContinue == true)
        {
            this.GoLobby();
            if (count == 0)
            {
                this.getUser.onRankActive();
                count = 1;
            }
        }
    }

    void GameOver()
    {
        this.StartCoroutine(CoShowGameOver());
    }

    //GameOver Scene 보여주기
    private IEnumerator CoShowGameOver()
    {
        //this.sphere_gameover.SetActive(true);
        this.clear_txt.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        this.score.SetActive(true);

        for (int i = 0; i < InfoManager.Instance.score + 1; i += 10)
        {
            this.score_txt.text = i.ToString();
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < gameResultLists.Length; i++)
        {
            this.gameResultLists[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);
        this.continue_txt.gameObject.SetActive(true);
        this.isContinue = true;
    }

    private void GoLobby()
    {
        if (OVRInput.Get(OVRInput.RawButton.Any))
        {
            InfoManager.Instance.InitGameScore();
            SceneManager.LoadScene("LobbyScene");
        }
    }
}