using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnGem : MonoBehaviour
{
    private GameObject gem;
    public AudioSource src;
    public AudioClip reload;

    public void Start()
    {
        src.clip = reload;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            StartCoroutine(GemMovement());
            src.Play();
        }
    }

    private IEnumerator GemMovement()
    {
        Vector3 oldPosition;
        oldPosition = transform.position;
        transform.position = new Vector3(0, -100, 0);
        yield return new WaitForSeconds(2);
        transform.position = oldPosition;
    }
}
