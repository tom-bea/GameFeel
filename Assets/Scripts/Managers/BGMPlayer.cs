using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioSource BGM;

    void OnEnable()
    {
        GameManager.UpdateArtAndAnimationsEvent += StartBGM;
    }

    void OnDisable()
    {
        GameManager.UpdateArtAndAnimationsEvent -= StartBGM;
    }

    void StartBGM(){
        if (BGM.isPlaying == false)
            BGM.Play();
    }
}
