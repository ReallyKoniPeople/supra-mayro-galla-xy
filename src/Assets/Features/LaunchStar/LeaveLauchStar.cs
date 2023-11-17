using Cinemachine;
using UnityEngine;

public class LeaveLauchStar : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CinemachineDollyCart dollyCart = other.transform.parent.GetComponent<CinemachineDollyCart>();
        dollyCart.m_Path = null;
        dollyCart.m_Position = 0;
        other.transform.parent.rotation = Quaternion.identity;
    }
}
