using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation //: MonoBehaviour 
{

    private Animator m_AnimatorHero;
    private SpriteRenderer m_spriteRendererHeroModel;

    public PlayerAnimation()
    {
        Start();
    }


    void Start () {

        m_AnimatorHero = Storage.Instance.HeroModel.GetComponent<Animator>();
        if (m_AnimatorHero == null)
            Debug.Log("############ Hero Animator Component is empty");

        m_spriteRendererHeroModel = Storage.Instance.HeroModel.GetComponent<SpriteRenderer>();
        if (m_AnimatorHero == null)
            Debug.Log("############ Hero SpriteRender Component is empty");


        //AnimationMoveHero
        AnimatorTransitionInfo transInfo = m_AnimatorHero.GetAnimatorTransitionInfo(0);
        AnimatorClipInfo[] clipInfo = m_AnimatorHero.GetCurrentAnimatorClipInfo(0);
        AnimatorStateInfo stateInfo = m_AnimatorHero.GetCurrentAnimatorStateInfo(0);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void HeroLook(bool isRight)
    {
        m_spriteRendererHeroModel.flipX = isRight;
    }

    public void HeroMove(bool isMoving)
    {
        if (m_AnimatorHero == null)
            return;

        m_AnimatorHero.SetBool("TriggerMoveHero", isMoving);
        //m_AnimatorHero.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
    }
}
