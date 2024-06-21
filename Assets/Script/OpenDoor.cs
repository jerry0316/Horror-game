using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenDoor : MonoBehaviour
{
    private Animation anim;

    [SerializeField] private TextMeshProUGUI CodeText;
    // Start is called before the first frame update
    void Start()
    {
        
        anim = GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
