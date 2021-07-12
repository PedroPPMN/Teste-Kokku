using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource board;
    AudioSource pieces;

    AudioClip menu;
    AudioClip move;

    void Start()
    {
        board = GameObject.Find("Tetris_Board").GetComponent<AudioSource>();
        pieces = GameObject.Find("Pieces").GetComponent<AudioSource>();

        menu = (AudioClip)Resources.Load("Audio/Wav/Full");
        move = (AudioClip)Resources.Load("Audio/move");

        PlayLoop();
    }

    public void PlayLoop()
    {
        board.clip = menu;
        board.Play();
        board.loop = true;
    }

    public void ChangeMusicVelocity(float velocity)
    {
        board.pitch = velocity;
        pieces.pitch = velocity;
    }

    public void PlayMovePiece()
    {
        pieces.clip = move;
        pieces.Play();
    }
}
