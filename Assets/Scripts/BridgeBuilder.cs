using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BridgeBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject bridgePrefab;
    private GameObject newBridge;
    private GameManager gameManager;
    [SerializeField]
    private AudioClip constructionClip;
    private AudioSource constructSound;

    Vector3 rotationPoint;
    private float stepAngle; //falling step angle by tick
    private float fallingTime;
    private float increasingSpeed;
    Vector3 spawnPosition;
    
    enum BridgeState { NotExist, Instantiated, Falling }
    BridgeState bridgeState;

    private void Awake()
    {
        constructSound = GetComponent<AudioSource>();
        bridgeState = BridgeState.NotExist;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        fallingTime = 2000f; //ms
        increasingSpeed = 5f; //speed of building
    }

    public void InitPosition(float spawnX)
    {
        spawnPosition = new Vector3(spawnX, bridgePrefab.transform.position.y);
    }

    public bool IsOnGround() // if raycast from right corner of bridge touch block
    {
        if (newBridge != null) 
        {
            LayerMask mask = LayerMask.GetMask("Ground");
            Transform bridgeTransform = newBridge.transform;
            Vector3 castPoint = new Vector3(bridgeTransform.localPosition.x + bridgeTransform.localScale.y / 2,
                bridgeTransform.localPosition.y, bridgeTransform.position.z);

            //Debug.DrawRay(castPoint, Vector3.down, Color.green, 10.0f);
            //Debug.DrawRay(castPoint, Vector3.right, Color.green, 10.0f);
            if (Physics2D.Raycast(castPoint, Vector2.down, 1.0f, mask) || Physics2D.Raycast(castPoint, Vector2.right, 0.7f, mask))
            {
                return true;
            }
            else
            {
                newBridge = null;
                return false;
            }
        }
        else
        {
            return true;
        }
    }


    public void Build()
    {
        switch (bridgeState)
        {
            case BridgeState.NotExist:
                if (Input.touchCount >= 1)
                {
                    if (Input.touches[0].phase == TouchPhase.Began)
                    {
                        InstantiateBridge();
                    }
                }
                else if(Input.GetMouseButtonDown(0))
                {
                    InstantiateBridge();
                }
                break;
            case BridgeState.Instantiated:
                if (Input.touchCount >= 1)
                {
                    if (Input.touches[0].phase == TouchPhase.Ended)
                    {
                       StopBuilding();
                    }
                }
                else if (Input.GetMouseButtonUp(0) || newBridge.transform.localScale.y >= 5)
                {
                    StopBuilding();
                }
                else
                {
                    IncreasingBridgeSize();
                }
                break;
            case BridgeState.Falling:
                FallBridge();
                break;
        }

    }

    private void InstantiateBridge()
    {
        newBridge = Instantiate(bridgePrefab);
        newBridge.transform.position = spawnPosition;
        bridgeState = BridgeState.Instantiated;
        constructSound.clip = constructionClip;
        constructSound.Play();
    }
    private void IncreasingBridgeSize()
    {
        //size up
        newBridge.transform.position += Vector3.up * increasingSpeed * Time.deltaTime / 2.0f;
        newBridge.transform.localScale += Vector3.up * increasingSpeed * Time.deltaTime; 
    }
    private void StopBuilding()
    {
        constructSound.Stop();
        //define rotation point
        rotationPoint = newBridge.transform.position;
        rotationPoint.y -= newBridge.transform.localScale.y / 2;
        rotationPoint.x += newBridge.transform.localScale.x / 2;
        stepAngle = -90 / (fallingTime / 1000); //calculate angle per tick
        bridgeState = BridgeState.Falling;
    }

    private void FallBridge()
    {
        //if not at the right position
        if (newBridge.transform.localRotation.eulerAngles.z > 270f || newBridge.transform.localRotation.eulerAngles.z == 0)
        {
            newBridge.transform.RotateAround(rotationPoint, new Vector3(0f, 0f, 1f), stepAngle * Time.deltaTime);
        }
        else
        {
            //make sure that bridge have exactly -90 degree rotation and height of block
            newBridge.transform.rotation = Quaternion.Euler(0, 0, -90f);
            newBridge.transform.position = new Vector3(newBridge.transform.position.x, bridgePrefab.transform.position.y,
                                                        newBridge.transform.position.z);
            bridgeState = BridgeState.NotExist;
            gameManager.ChangeState(GameManager.GameState.Moving);
        }
    }
}
