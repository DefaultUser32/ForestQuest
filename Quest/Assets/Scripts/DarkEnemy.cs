using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEnemy : Enemy
{
    // Matthew Macdonald
    // 14-01-24
    // A variant of the enemy script, used for changes in the names of animations
    public override bool isAttacking // is true when the current animation is the attack animation
    {
        get
        {
            return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HeavyBandit_Attack";
        }
    }
    public override bool isHurt // is true when the current animation is the hurt animation
    {
        get
        {
            return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HeavyBandit_Hurt";
        }
    }
}