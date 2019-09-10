using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation //: MonoBehaviour 
{
    private bool m_isMultiAnimation = false;
    private bool m_isMultiSprite = false;
    private Animator m_AnimatorHero;
    private Animator m_AnimatorBack;
    private Animator m_AnimatorFront;

    private SpriteRenderer m_spriteRendererHeroModel;
    private SpriteRenderer m_spriteRendererBack;
    private SpriteRenderer m_spriteRendererFront;

    private GameObject m_modelView;

    public string CurrentAnimationPlay = "";

    public PlayerAnimation(Animator p_AnimatorHero, GameObject p_modelView)
    {
        Debug.Log(">>>>>>>> IsMultiSprite " + p_modelView.name);
        m_isMultiSprite = true;
        m_AnimatorHero = p_AnimatorHero;
        m_modelView = p_modelView;
        Init();
    }

    public PlayerAnimation(Animator p_AnimatorHero, SpriteRenderer p_spriteRendererHeroModel)
    {
        m_isMultiAnimation = false;
        m_AnimatorHero = p_AnimatorHero;
        m_spriteRendererHeroModel = p_spriteRendererHeroModel;
        Init();
    }
    public PlayerAnimation(Animator p_AnimatorBack, Animator p_AnimatorFront, SpriteRenderer p_spriteRendererBack, SpriteRenderer p_spriteRendererFront)
    {
        m_isMultiAnimation = true;
        m_AnimatorBack = p_AnimatorBack;
        m_AnimatorFront = p_AnimatorFront;
        m_spriteRendererBack = p_spriteRendererBack;
        m_spriteRendererFront = p_spriteRendererFront;
        Init();
    }

    void Init()
    {
        if (!m_isMultiAnimation &&  m_AnimatorHero == null)
            Debug.Log("############ Hero Animator Component is empty");

        if(m_spriteRendererHeroModel == null)
            Debug.Log("############ Hero SpriteRender Component is empty");

        if (m_isMultiAnimation == true && (m_AnimatorBack == null || m_AnimatorFront == null))
            Debug.Log("############ Hero Animators Component is empty");

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void PersonLook(bool isRight)
    {
        if(m_isMultiSprite)
        {
            float x = m_modelView.transform.localScale.x;
            if ((x > 0 && isRight) || (x < 0 && !isRight))
            {
                x *= -1;
                if (m_modelView != null)
                {
                    m_modelView.transform.localScale = new Vector3(x
                        , m_modelView.transform.localScale.y,
                        m_modelView.transform.localScale.z);
                }
            }
        }
        else if (!m_isMultiAnimation)
        {
            m_spriteRendererHeroModel.flipX = isRight;
        }
        else
        {
            m_spriteRendererBack.flipX = isRight;
            m_spriteRendererFront.flipX = isRight;
        }
    }

    private bool isValidAnimation()
    {
        if (!m_isMultiAnimation && m_AnimatorHero == null)
            return false;
        if (m_isMultiAnimation == true && (m_AnimatorBack == null || m_AnimatorFront == null))
            return false;

        return true;
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
        if (!isValidAnimation())
            return;

        if (!m_isMultiAnimation)
        {
            m_AnimatorHero.SetBool("TriggerMove", isMoving);
        }
        else
        {
            m_AnimatorBack.SetBool("TriggerMove", isMoving);
            m_AnimatorFront.SetBool("TriggerMove", isMoving);
        }

        if (isMoving)
            CurrentAnimationPlay = "TriggerMove";
        else
            CurrentAnimationPlay = "";
             

        //m_AnimatorHero.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
    }
    public void PersonIdle()
    {
        if (!isValidAnimation())
            return;

        if (!m_isMultiAnimation)
        {
            m_AnimatorHero.SetBool("TriggerMove", false);
            m_AnimatorHero.SetBool("TriggerIdle", true);
        }
        else
        {
            m_AnimatorBack.SetBool("TriggerMove", false);
            m_AnimatorBack.SetBool("TriggerIdle", true);
            m_AnimatorFront.SetBool("TriggerMove", false);
            m_AnimatorFront.SetBool("TriggerIdle", true);
        }


        //CurrentAnimationPlay = "";
        CurrentAnimationPlay = "TriggerIdle";
    }


}
