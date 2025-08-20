using Photon.Pun;
using UnityEngine;

public class HitboxEvent : MonoBehaviour
{
    private PhotonView myPV;

    void Awake()
    {
        myPV = transform.root.GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.GetComponent<AgentCtrl>().GetHit();
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<FindPlayerCtrl>().GetHit();

            if (myPV.IsMine)
            {
                bool isWinner = FindGameManager.Instance.SetScore();

                if (isWinner)
                {
                    string nickname = myPV.Owner.NickName;
                    myPV.RPC("Winner", RpcTarget.AllBuffered, nickname);
                }
            }
        }
    }
}
