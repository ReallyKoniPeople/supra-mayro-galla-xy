using Cinemachine;
using UnityEditor;
using UnityEngine;

public class Kubba : MonoBehaviour
{
    public float heightOffset = 1.65f;

    public AudioSource randomAudioSource;
    public AudioSource walkAudioSource;
    public AudioSource deathAudioSource;
    public GameObject shellPrefab;

    public void Update()
    {
        int random = Random.Range(1, 10000);
        if (random == 1)
        {
            if (!randomAudioSource.isPlaying)
            {
                randomAudioSource.PlayOneShot(randomAudioSource.clip, 1f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (!CompareTag("EnemyWeakpoint"))
        {
            player.PlayerDeath();
            return;
        }

        if (!deathAudioSource.isPlaying)
        {
            deathAudioSource.PlayOneShot(deathAudioSource.clip, 1f);
        }

        if (transform.parent.parent.TryGetComponent(out CinemachineDollyCart dollyCart))
        {
            dollyCart.enabled = false;
        }

        player.JumpWithoutSound();


        var shellObject = Instantiate(shellPrefab);
        shellObject.transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y - heightOffset, transform.parent.position.z);
        shellObject.transform.rotation = transform.parent.rotation;
        shellObject.transform.parent = transform.parent.parent;

        //transform.parent.gameObject.SetActive(false);
        foreach (Transform child in transform.parent)
        {
            if (child.name != "KubbaDeathAudio")
                child.gameObject.SetActive(false);
        }
    }
}
