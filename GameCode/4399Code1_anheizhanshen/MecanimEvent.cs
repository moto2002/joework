using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MecanimEvent
{
    private Animator m_animator;
    private AnimationClip m_currentMark;
    private bool m_isNotFadeing = true;
    private float passTime = 0f;

    public MecanimEvent(Animator animator)
    {
        this.m_animator = animator;
    }

    public IEnumerator CheckAnimationChange(Action<int, bool> StateChanged)
    {
        while (true)
        {
            AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(0);
            this.passTime += Time.get_deltaTime();
            if (this.passTime >= currentAnimatorStateInfo.get_length())
            {
                int introduced3 = currentAnimatorStateInfo.get_nameHash();
                StateChanged(introduced3, currentAnimatorStateInfo.get_loop());
                this.passTime = 0f;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator CheckAnimationChange(Action<string, bool> StateChanged)
    {
        while (true)
        {
            AnimatorClipInfo[] currentAnimatorClipInfo = this.m_animator.GetCurrentAnimatorClipInfo(0);
            if (currentAnimatorClipInfo.Length != 0)
            {
                if (currentAnimatorClipInfo[0].get_weight() != 1f)
                {
                    if (this.m_isNotFadeing)
                    {
                        AnimatorClipInfo[] nextAnimatorClipInfo = this.m_animator.GetNextAnimatorClipInfo(0);
                        if ((StateChanged != null) && (nextAnimatorClipInfo.Length != 0))
                        {
                            StateChanged(nextAnimatorClipInfo[0].get_clip().get_name(), true);
                        }
                        this.m_currentMark = currentAnimatorClipInfo[0].get_clip();
                        this.m_isNotFadeing = false;
                        yield return new WaitForFixedUpdate();
                    }
                }
                else if (this.m_currentMark != currentAnimatorClipInfo[0].get_clip())
                {
                    if ((this.m_currentMark != null) && (StateChanged != null))
                    {
                        StateChanged(this.m_currentMark.get_name(), false);
                    }
                    this.m_currentMark = currentAnimatorClipInfo[0].get_clip();
                    this.m_isNotFadeing = true;
                    yield return new WaitForFixedUpdate();
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }


}

