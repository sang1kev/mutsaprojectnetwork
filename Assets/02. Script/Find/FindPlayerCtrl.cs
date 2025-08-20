using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using StarterAssets;
using TMPro;

public class FindPlayerCtrl : MonoBehaviourPun
{
    private Animator anim;
    private Camera mainCam;

    [SerializeField] private Transform playerRoot;
    [SerializeField] private TextMeshPro nickName;

    [SerializeField] private GameObject punchBox;
    [SerializeField] private GameObject kickBox;

    private bool isAttack = false;
    private bool isDead = false;

    void Awake()
    {
        mainCam = Camera.main;
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            nickName.text = PhotonNetwork.NickName;
            nickName.color = Color.green;
            var followCam = FindFirstObjectByType<CinemachineCamera>();
            followCam.Target.TrackingTarget = playerRoot;
        }
        else
        {
            GetComponent<PlayerInput>().enabled = false;
            nickName.text = photonView.Owner.NickName;
            nickName.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
    }

    void OnPunch()
    {
        if (!isAttack && !isDead)
            photonView.RPC(nameof(RPC_Punch), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Punch()
    {
        StartCoroutine(PunchRoutine());
    }
    IEnumerator PunchRoutine()

    {
        isAttack = true;
        anim.SetTrigger("punch");
        yield return new WaitForSeconds(0.5f);
        punchBox.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        punchBox.SetActive(false);
        isAttack = false;
    }

    void OnKick()
    {
        if (!isAttack && !isDead)
            photonView.RPC(nameof(RPC_Kick), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Kick()
    {
        StartCoroutine(KickRoutine());
    }

    IEnumerator KickRoutine()
    {
        isAttack = true;
        anim.SetTrigger("kick");
        yield return new WaitForSeconds(0.6f);
        kickBox.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        kickBox.SetActive(false);
        isAttack = false;
    }

    public void GetHit()
    {
        photonView.RPC(nameof(Dead), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Dead()
    {
        isDead = true;
        anim.SetTrigger("death");
        GetComponent<CharacterController>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<ThirdPersonController>().enabled = false;

        if (photonView.IsMine)
        {
            mainCam.cullingMask |= (1 << 9);
            FindGameManager.Instance.SetObserver();
        }
    }

    [PunRPC]
    public void Winner(string nickname)
    {
        FindGameManager.Instance.EndGame(nickname);
    }
}
