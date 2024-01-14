using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodeGate : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // handles opening/closing door if nodes are all active

    public List<Node> nodes; // assosicated nodes
    public bool invert; // if inverted, flip the starting/ending positions
    public float endOfset; // amount moved when opened
    private Vector3 startPos; // original position
    private Vector3 endPos; // ending position

    private Coroutine move; // is null if not moving
    private void Start()
    {
        // update starting/ending position
        startPos = transform.position;
        endPos = transform.position;
        endPos.y += endOfset;
        CheckSelf();
    }

    public void CheckSelf() // update gate state
    {
        // check each node, if all are active, open the door
        foreach (Node node in nodes)
        {
            if (!node.isActive)
            {

                StartMove(true);
                return;
            }
        }
        StartMove(false);
    }
    private void StartMove(bool state) // handle the managment of moving
    {
        // exclusive or (^) operator causes state to flip if invert is true
        Vector3 goal = (invert ^ state) ? startPos : endPos;

        // if the gate is almost at its goal, return
        if ((transform.position - goal).magnitude < 0.1f)
        {
            return;
        }

        // if already moving, stop moving
        if (move != null)
        {
            StopCoroutine(move);
            move = null;
        }

        // start moving
        move = StartCoroutine(MoveTo(goal));
    }
    private IEnumerator MoveTo(Vector3 endPos)
    {
        // set step to be a vector representing 1 30th of the distance between the gate and the end position
        Vector3 Step = (endPos - transform.position) / 30;
        transform.position += Step;

        // move gate
        for (int i = 0; i < 30; i++)
        {
            transform.position += Step;
            yield return new WaitForSeconds(0.01f);
        }

        // nullify coroutine, return
        move = null;
        yield return null;
    }
}
