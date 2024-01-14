using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // nodes are elements required to open a gate

    // gates associated with the node
    [SerializeField] List<NodeGate> parents;

    // is this node a brazier (fire gate)
    public bool isBrazier;

    // is enabled
    public bool isActive;

    // internal
    private Animator anim;
    
    private void Start()
    {
        // internal variable
        anim = GetComponent<Animator>();
    }

    public void Toggle() // flip state of node, update gates
    {
        isActive = !isActive;
        if (isBrazier)
        {
            anim.SetBool("IsActive", isActive);
        }
        foreach (NodeGate gate in parents)
        {
            gate.CheckSelf();
        }
    }
}
