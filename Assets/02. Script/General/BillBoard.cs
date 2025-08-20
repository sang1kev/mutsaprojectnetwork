using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform mainCam;

    void Start()
    {
        mainCam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
