using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MoveLevelParent : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // handles moving all level selectors when slider is moved

    // scrolling information
    [SerializeField] Scrollbar bar; // reference to scrollbar
    [SerializeField] float startPos; // starting position
    [SerializeField] float endPos; // ending position

    private Vector3 originalPos; // original position

    private void Start()
    {
        // set original position
        originalPos.y = transform.localPosition.y;
        originalPos.z = transform.localPosition.z;
    }
    public void UpdateSelf()
    {
        // set position to be inbetween start/end positions depending on the value of the bar
        transform.localPosition = new Vector3(math.lerp(startPos, endPos, bar.value), originalPos.y, originalPos.z);
    }
}
