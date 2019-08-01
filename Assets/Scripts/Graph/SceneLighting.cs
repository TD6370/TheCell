using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLighting : MonoBehaviour {

    public Light HeroSpotLight;
    public Light HeroPointLight;

    private bool m_IsHeroLight = false;
    public bool IsHeroLight = false;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_IsHeroLight != IsHeroLight)
            SetHeroLight(IsHeroLight);
    }

    private void SetHeroLight(bool isLightOn)
    {
        m_IsHeroLight = isLightOn;
        HeroSpotLight.enabled = isLightOn;
        HeroPointLight.enabled = isLightOn;
    }
}
