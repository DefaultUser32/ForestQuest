using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [SerializeField] Vector2 minBound;
    [SerializeField] Vector2 maxBound;
    [SerializeField] float timeBetween;

    private void Start()
    {
        Debug.Log("?");
        StartCoroutine(RandomHandler());
    }
    private IEnumerator RandomHandler()
    {
        Vector2 nextPos;
        while (true)
        {
            yield return new WaitForSeconds(timeBetween);
            nextPos.x = Random.Range(minBound.x, maxBound.x);
            nextPos.y = Random.Range(minBound.y, maxBound.y);
            transform.localPosition = nextPos;
        }
    }
}
