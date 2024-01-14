using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // Responsible for moving the camera

    [SerializeField] Transform player; // player position
    [SerializeField] float moveRange; // min range before the camera starts scrolling
    [SerializeField] float moveSpeed; // speed at which the camera moves
    [SerializeField] float zPos; // z position of camera
    [SerializeField] float minX; // minimum x position of the camera
    [SerializeField] float maxX; // maximum x position of the camera

    // internal variables
    private Vector3 relativeVec; // vector between player and camera
    private Vector3 newPos; // new camera position after changes

    private void Awake() // ensure camera is properly aligned at start of game
    {
        UpdatePos();
    }
    private void Update() // update camera position each frame
    {
        UpdatePos();
    }
    private void UpdatePos() // update camera position each frame
    {
        // update relative position
        relativeVec = player.position - transform.position;

        // if the relative x position is larger than the min move range, update camera position
        if (Mathf.Abs(relativeVec.x) > moveRange)
        {
            newPos = new Vector3(transform.position.x + relativeVec.x * (Time.deltaTime * moveSpeed), 0, zPos);
        }

        // clamp new position between min and max position
        newPos.x = math.clamp(newPos.x, minX, maxX);

        transform.position = newPos;
    }
}
