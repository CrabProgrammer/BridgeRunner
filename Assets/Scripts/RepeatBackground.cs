using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPosition;
    private float repeatWidth;

    private void Start()
    {
        startPosition = transform.position;
        //background image was repeated twice by wrap mode
        repeatWidth = GetComponent<BoxCollider2D>().size.x / 2; 
    }

    private void Update()
    {
        if(transform.position.x < startPosition.x - repeatWidth)
        {
            transform.position = startPosition;
        }
    }
}
