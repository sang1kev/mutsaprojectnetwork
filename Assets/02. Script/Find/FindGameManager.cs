using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FindGameManager : Singleton<FindGameManager>
{
    [SerializeField] private GameObject observerCam;

    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI winnerTxt;

    private int score;
    [SerializeField] private int winnerScore = 2;
    
    IEnumerator Start()
    {
        float randomTime = Random.Range(0f, 1f);
        yield return new WaitForSeconds(randomTime);

        int ranIndex = Random.Range(0, 5);

        var randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        PhotonNetwork.Instantiate("Player_" + ranIndex, randomPos, Quaternion.identity);
    }

    public void SetObserver()
    {
        observerCam.SetActive(true);
    }

    public bool SetScore()
    {
        score++;

        scoreTxt.text = $"Current Score : {score}"; 

        if (score >= winnerScore)
        {
            return true;
        }

        return false;
    }

    public void EndGame(string nickname)
    {
        Fade.onFadeAction(3f, Color.white, true, () =>
        {
            winnerTxt.text = $"{nickname} is a Winner!";
            winnerTxt.gameObject.SetActive(true);
        });
    }
}
