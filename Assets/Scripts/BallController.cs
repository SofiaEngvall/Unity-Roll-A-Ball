using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BallController : MonoBehaviour
{
    private const int MAX_POINTS = 8;
 
    //Main player controlled physical representation
    Rigidbody rbBall;

    //Position, direction, speed
    float moveHorizontal, moveVertical;
    Vector3 direction;
    int speed;
    
    //Score, timer
    int score;
    float secOffset;

    //Texts
    public TMP_Text finalTextHeader, finalText, leftText, timerText, playAgainText;

    //Audio
    public AudioSource music;
    AudioSource pickupSound, yaySound;

    // Start is called before the first frame update
    void Start()
    {
        rbBall = GetComponent<Rigidbody>();
        speed = 10;

        InitGame();
        InitTexts();
        InitAudio();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        //If the timer has not started yet
        if (secOffset == 0)
        {
            //If a movement has been made
            if (moveHorizontal != 0 || moveVertical != 0)
            {
                //Save game start time
                secOffset = Time.timeSinceLevelLoad;
            }
        }
        //If the timer has started = the game is running (but not finished)
        else if(score != MAX_POINTS)
        {
            //Update timer
            SetTimerText();
        }
    }

    private void GetInput()
    {
        //Get movement input
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        //Calculate Vector3
        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //- Get game control input -
        //ESC to quit
        if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        //R to  restart game
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetBall();
            ResetPickups();
            InitGame();
            InitTexts();
            music.Play();
        }
    }

    private void FixedUpdate()
    {
        //Move in saved direction
        rbBall.AddForce(direction * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Collided with pickup
        if(other.CompareTag("PickUp"))
        {
            //Play pickup sound
            pickupSound.Play();

            //Increase score
            score++;
            SetLeftText();

            //Deactivate pickup
            other.gameObject.SetActive(false);

            //If game finifhed
            if (score == MAX_POINTS)
            {
                ManageTextAtGameEnd();
                ManageSoundAtGameEnd();
            }
        }
    }

    //Game methods
    private void InitGame()
    {
        score = 0;
        secOffset = 0;
    }
    private void ResetBall()
    {
        rbBall.position = new Vector3(0, 0.5f, 0);
        rbBall.rotation = Quaternion.identity;
        rbBall.velocity = Vector3.zero;
        rbBall.angularVelocity = Vector3.zero;
    }
    private static void ResetPickups()
    {
        //Find the parent by tag and get it's children and activate them all
        Transform[] pickUps = GameObject.FindGameObjectWithTag("PickUps").GetComponentsInChildren<Transform>(true);
        foreach (Transform pickUp in pickUps) pickUp.gameObject.SetActive(true);
    }

    //Audio methods
    private void InitAudio()
    {
        AudioSource[] allSounds = GetComponents<AudioSource>();
        pickupSound = allSounds[0];
        yaySound = allSounds[1];
    }
    private void ManageSoundAtGameEnd()
    {
        music.Stop();
        yaySound.PlayScheduled(3000);
    }

    //Text methods
    private void InitTexts()
    {
        finalTextHeader.text = "";
        finalText.text = "";
        timerText.text = "Time:";
        playAgainText.text = "";
        SetLeftText();
    }
    private void SetLeftText()
    {
        leftText.text = MAX_POINTS - score + " Left";
    }
    private void SetTimerText()
    {
        timerText.text = "Time: " + (Time.timeSinceLevelLoad - secOffset).ToString("0.0") + "s";
    }
    private void ManageTextAtGameEnd()
    {
        timerText.text = "";
        leftText.text = "";
        finalTextHeader.text = "Good job!";
        finalText.text = "You got 'em all\nin just " + (Time.timeSinceLevelLoad - secOffset).ToString("0.00") + "s!";
        playAgainText.text = "< Press R to play again! >";
    }
}
