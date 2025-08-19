using System.Collections;
using Photon.Pun;
using UnityEngine;

public class FighterGameManager : Singleton<FighterGameManager>
{
    [SerializeField] private GameObject dieUI;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        var randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        PhotonNetwork.Instantiate("FighterPlayer", randomPos, Quaternion.identity);
    }

    public void EndGame()
    {
        Fade.onFadeAction(3f, Color.black, true, () => dieUI.SetActive(true));
    }
}
