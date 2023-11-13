using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLauchStar : MonoBehaviour
{
    public float speed = 0.1f;
    private void OnTriggerEnter(Collider other)
    {
        CinemachineDollyCart dollyCart = other.transform.parent.GetComponent<CinemachineDollyCart>();
        CinemachineSmoothPath dollyTrack = gameObject.transform.parent.GetComponent<CinemachineSmoothPath>();
        dollyCart.m_Path = dollyTrack;
        dollyCart.m_Speed = speed;
    }
}
