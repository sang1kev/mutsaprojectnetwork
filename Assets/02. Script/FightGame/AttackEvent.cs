using Photon.Pun;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    [SerializeField] private PhotonView myPV;
    [SerializeField] private float damage;

    void Awake()
    {
        myPV = transform.root.GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView otherPV = other.GetComponent<PhotonView>();

            if (myPV.IsMine)
            {
                myPV.RPC("TriggerEvent", RpcTarget.All, otherPV.ViewID, damage);
            }
        }
    }
}
