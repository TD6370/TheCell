using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation //: MonoBehaviour 
{

    private Animator m_AnimatorHero;
    private SpriteRenderer m_spriteRendererHeroModel;
    public string CurrentAnimationPlay = "";

    public PlayerAnimation(Animator p_AnimatorHero, SpriteRenderer p_spriteRendererHeroModel)
    {
        m_AnimatorHero = p_AnimatorHero;
        m_spriteRendererHeroModel = p_spriteRendererHeroModel;
        Init();
    }

    void Init()
    {
        if (m_AnimatorHero == null)
            Debug.Log("############ Hero Animator Component is empty");

        if (m_AnimatorHero == null)
            Debug.Log("############ Hero SpriteRender Component is empty");
       
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void PersonLook(bool isRight)
    {
        m_spriteRendererHeroModel.flipX = isRight;

        if (m_AnimatorHero == null)
            return;
    }

    private void Test()
    {
        //AnimationMoveHero
        //AnimatorTransitionInfo transInfo = m_AnimatorHero.GetAnimatorTransitionInfo(0);
        //AnimatorClipInfo[] clipInfo = m_AnimatorHero.GetCurrentAnimatorClipInfo(0);
        //AnimatorStateInfo stateInfo = m_AnimatorHero.GetCurrentAnimatorStateInfo(0);
        //var anim = m_AnimatorHero.GetAnimatorTransitionInfo(0);

        //FIXANIM
        //int intMove = isRight ? -1 : 0;
        //m_AnimatorHero.SetFloat("root", -1);
        //m_AnimatorHero.rootPosition.Set(m_AnimatorHero.rootPosition.x * intMove, m_AnimatorHero.rootPosition.y * intMove, m_AnimatorHero.rootPosition.z);

        //TEST-----------
        //AnimatorStateInfo stateInfo = m_AnimatorHero.GetCurrentAnimatorStateInfo(0);
        //var m_AnimatorClipInfo = m_AnimatorHero.GetCurrentAnimatorClipInfo(0);
        //Debug.Log("Starting clip : " + m_AnimatorClipInfo[0].clip);
        //--------------
    }

    public void PersonMove(bool isMoving)
    {
        if (m_AnimatorHero == null)
            return;

        m_AnimatorHero.SetBool("TriggerMove", isMoving);
        if (isMoving)
            CurrentAnimationPlay = "TriggerMove";
        else
            CurrentAnimationPlay = "";
             

        //m_AnimatorHero.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
    }
    public void PersonIdle()
    {
        if (m_AnimatorHero == null)
            return;

        m_AnimatorHero.SetBool("TriggerMove", false);
        m_AnimatorHero.SetBool("TriggerIdle", true);
        //CurrentAnimationPlay = "";
        CurrentAnimationPlay = "TriggerIdle";
    }


}
