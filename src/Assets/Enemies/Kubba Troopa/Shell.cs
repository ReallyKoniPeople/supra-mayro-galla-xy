using Cinemachine;
using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CrawlOutOfShell());
    }

    private IEnumerator CrawlOutOfShell()
    {
        yield return new WaitForSeconds(10);
        var kubba = transform.parent.parent.Find("Kubba");
        if (kubba != null)
        {
            foreach (Transform child in kubba)
            {
                child.gameObject.SetActive(true);
            }
        }

        if (transform.parent.parent.TryGetComponent(out CinemachineDollyCart dollyCart))
        {
            dollyCart.enabled = true;
        }

        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Destroy(transform.parent.parent.parent.gameObject);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().JumpWithoutSound();
    }
}
