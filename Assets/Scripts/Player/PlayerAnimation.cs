using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation //: MonoBehaviour 
{

    private Animator m_AnimatorHero;
    private SpriteRenderer m_spriteRendererHeroModel;

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

        //m_spriteRendererHeroModel = Storage.Instance.HeroModel.GetComponent<SpriteRenderer>();
        if (m_AnimatorHero == null)
            Debug.Log("############ Hero SpriteRender Component is empty");
        //AnimationMoveHero
        //AnimatorTransitionInfo transInfo = m_AnimatorHero.GetAnimatorTransitionInfo(0);
        //AnimatorClipInfo[] clipInfo = m_AnimatorHero.GetCurrentAnimatorClipInfo(0);
        //AnimatorStateInfo stateInfo = m_AnimatorHero.GetCurrentAnimatorStateInfo(0);
    }


    //private void SetAnimator(Animator _animator = null)
    //{
    //    if(_animator != null)
    //        m_AnimatorHero = _animator;
    //    if (m_AnimatorHero == null)
    //        m_AnimatorHero = Storage.Instance.HeroModel.GetComponent<Animator>();
    //    if (m_AnimatorHero == null)
    //        Debug.Log("############ Hero Animator Component is empty");
    //}

    // Update is called once per frame
    void Update () {
		
	}

    public void PersonLook(bool isRight)
    {
        m_spriteRendererHeroModel.flipX = isRight;
    }

    public void PersonMove(bool isMoving)
    {
        if (m_AnimatorHero == null)
            return;

        m_AnimatorHero.SetBool("TriggerMove", isMoving);
        //m_AnimatorHero.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
    }
}
