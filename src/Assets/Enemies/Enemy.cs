using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float dyingAnimationDuration = 1;
    public float dyingAnimationScale = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CompareTag("EnemyWeakpoint"))
            {
                foreach (Transform child in transform.parent)
                {
                    if (child.TryGetComponent(out Collider collider))
                        collider.enabled = false;
                }

                if (transform.parent.parent.TryGetComponent(out CinemachineDollyCart dollyCart))
                {
                    dollyCart.enabled = false;
                }

                var currentScale = transform.parent.parent.parent.localScale;
                transform.parent.parent.parent.localScale = new(currentScale.x, currentScale.y * dyingAnimationScale, currentScale.z);
                Destroy(transform.parent.parent.parent.gameObject, dyingAnimationDuration);
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
