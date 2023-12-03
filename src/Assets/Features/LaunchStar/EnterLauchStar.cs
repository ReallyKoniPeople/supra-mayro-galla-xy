using Cinemachine;
using UnityEngine;

public class EnterLauchStar : MonoBehaviour
{
    public AudioSource audioSource;
    public float speed = 0.1f;
    private void OnTriggerEnter(Collider other)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioSource.clip, 1f);
        }

        CinemachineDollyCart dollyCart = other.transform.parent.GetComponent<CinemachineDollyCart>();
        CinemachineSmoothPath dollyTrack = gameObject.transform.parent.GetComponent<CinemachineSmoothPath>();
        dollyCart.m_Path = dollyTrack;
        dollyCart.m_Speed = speed;
    }
}
