using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlight;
    public AudioSource turnOnSound;
    public AudioSource turnOffSound;

    private bool isOn = false;

    void Start()
    {
        flashlight.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("G"))
        {
            if (isOn)
            {
                flashlight.SetActive(false);
                if (!turnOffSound.isPlaying) // 只有在关闭音效没有播放时才播放
                {
                    turnOffSound.Play();
                }
            }
            else
            {
                flashlight.SetActive(true);
                if (!turnOnSound.isPlaying) // 只有在打开音效没有播放时才播放
                {
                    turnOnSound.Play();
                }
            }

            isOn = !isOn; // 切换手电筒状态
        }
    }
}
