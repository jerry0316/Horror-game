using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWithoutKey: MonoBehaviour
{
    public Animator door;
    public GameObject openText;
    public AudioSource doorSound;

    private bool inPlayer;

    void Start()
    {
        inPlayer = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inPlayer = true;
            openText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inPlayer = false;
            openText.SetActive(false);
        }
    }

    void Update()
    {
        if (inPlayer && Input.GetButtonDown("Interact"))
        {
            if (door.GetBool("open"))
            {
                DoorCloses();
            }
            else
            {
                DoorOpens();
            }
        }
    }

    void DoorOpens()
    {
        Debug.Log("It Opens");
        door.SetBool("open", true);
        door.SetBool("closed", false);
        doorSound.Play();
    }

    void DoorCloses()
    {
        Debug.Log("It Closes");
        door.SetBool("open", false);
        door.SetBool("closed", true);
        doorSound.Play(); 
    }
}
