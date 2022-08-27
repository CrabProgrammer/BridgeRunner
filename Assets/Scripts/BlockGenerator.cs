using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    private Vector3 spawnPosition;
    private float newBlockWidth;
    private float rangeToNextBlock;

    [SerializeField]
    private GameObject spawnPositionObject;
    [SerializeField]
    private GameObject lastBlock;
    [SerializeField]
    private GameObject blockPrefab;

    public void Start()
    {
        spawnPosition = spawnPositionObject.transform.position;
        InitNewBlock();
    }
    public void Generate()
    {
        //check range from right edge of last block to left edge of new block 
        if ((spawnPosition.x - newBlockWidth / 2) - (lastBlock.transform.position.x + lastBlock.transform.localScale.x/2) > rangeToNextBlock)
        {
            GameObject newBlock = Instantiate(blockPrefab);
            newBlock.transform.localScale = new Vector3(newBlockWidth, blockPrefab.transform.localScale.y, 0);
            newBlock.transform.position = spawnPosition;
            lastBlock = newBlock;
            InitNewBlock();   
        }
    }

    private void InitNewBlock()
    {
        newBlockWidth = Random.Range(0.5f, 2f);
        rangeToNextBlock = Random.Range(1.5f, 3.5f);
    }
}
