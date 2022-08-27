using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score; 

    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private GameObject player;
    private PlayerController playerController;

    private BlockGenerator blockGenerator;
    private BridgeBuilder bridgeBuilder;

    [SerializeField]
    private AudioClip gameOverClip;
    private AudioSource gameOverSound;

    public bool canMove { get; private set; }
    public enum GameState { Moving, Building, Reviving, GameOver }
    public GameState currentState { get; private set; }

    void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
        playerController = player.GetComponent<PlayerController>();
        blockGenerator = GetComponent<BlockGenerator>();
        bridgeBuilder = GetComponent<BridgeBuilder>();
        gameOverSound = GetComponent<AudioSource>();  
        ChangeState(GameState.Moving); //Start game in moving state
    }


    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
    public void ChangeState(GameState state) 
    {
        switch(state)
        {
            case GameState.Moving:
                canMove = true; //enable moving left all objects
                playerController.Run(bridgeBuilder.IsOnGround()); //play animation and check falling
                currentState = GameState.Moving; 
                break;
            case GameState.Building:
                canMove = false;
                //build bridge next to player
                bridgeBuilder.InitPosition(playerController.GetRightBoundPosition());
                playerController.Stay();
                currentState = GameState.Building;
                break;
            case GameState.Reviving:
                canMove = true;
                playerController.Stay();
                playerController.ResetPosition();
                currentState = GameState.Reviving;
                break;
            case GameState.GameOver:
                gameOverSound.PlayOneShot(gameOverClip);
                canMove = false;
                playerController.Stay();
                gameOverPanel.SetActive(true);
                currentState = GameState.GameOver;
                break;
        }
    }
    void Update()
    {
        switch(currentState)
        {
            case GameState.Moving:
                blockGenerator.Generate();
                break;
            case GameState.Building:
                bridgeBuilder.Build();
                break;
            case GameState.Reviving:
                if(playerController.IsOnGround()) //move objects left 
                {
                    ChangeState(GameState.Moving);
                }
                break;  
            case GameState.GameOver:
                break;
        }
       
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Revive()
    {
        gameOverPanel.SetActive(false);
        ChangeState(GameState.Reviving);
    }
}
