using Cinemachine;
using UnityEngine;

public class Tromp : MonoBehaviour
{
    public AudioSource impactAudioSource;
    bool directionUp = true;


    void Start()
    {
        CinemachineDollyCart dollyCart = transform.parent.GetComponent<CinemachineDollyCart>();
        dollyCart.m_Position = Random.Range(0.0f, 7.5f); // every tromp gets a random starting position
    }

    void Update()
    {
        CinemachineDollyCart dollyCart = transform.parent.GetComponent<CinemachineDollyCart>();
        if (directionUp && dollyCart.m_Position == 7.5)
        {
            directionUp = false;
        }
        else if (!directionUp && dollyCart.m_Position == 0)
        {
            if (!impactAudioSource.isPlaying)
            {
                impactAudioSource.PlayOneShot(impactAudioSource.clip, 1f);
            }
            directionUp = true;
        }

        if (directionUp)
        {
            dollyCart.m_Speed = 5.0f;
        }
        else
        {
            dollyCart.m_Speed = -25.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayerDeath();
        }
    }
}
