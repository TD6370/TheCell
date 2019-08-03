using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class SceneLighting : MonoBehaviour {

    public Texture2D DayLUT;
    public Texture2D MorningLUT;
    public Texture2D NightLUT;

    public Light HeroSpotLight;
    public Light HeroPointLight;

    private bool m_IsSceneGradientColorOn = false;
    public bool IsSceneGradientColorOn = false;

    private bool m_IsHeroLight = false;
    public bool IsHeroLight = false;

    private PostProcessingProfile m_PostProcessProfile;
    private PostProcessingBehaviour m_postProcess;
    private UserLutModel m_lutModel;

    private FiltersTimeOfDay m_FilterTimeOfDay = FiltersTimeOfDay.Day;
    public FiltersTimeOfDay FilterTimeOfDay = FiltersTimeOfDay.Day;
    public enum FiltersTimeOfDay
    {
        MorningPre,
        Morning,
        MorningPost,
        DayPre,
        Day,
        DayPost,
        EveningPre,
        Evening,
        EveningPost,
        NightPre,
        Night,
        NightPost
    }

    private void Awake()
    {
        m_postProcess = Storage.Instance.MainCamera.GetComponent<PostProcessingBehaviour>();
        if (m_postProcess == null)
            Debug.Log("######### Error loading DrawGeometry.PostProcessingBehaviour");
        else
        {
            m_PostProcessProfile = m_postProcess.profile;
            if (m_PostProcessProfile == null)
                Debug.Log("######### Error loading DrawGeometry.PostProcessingProfile");
        }
        m_lutModel = new UserLutModel();
    }


    // Use this for initialization
    void Start () {
            }
	
	// Update is called once per frame
	void Update () {
        //TEST
        if (m_IsHeroLight != IsHeroLight)
            SetHeroLight(IsHeroLight);

        if (m_FilterTimeOfDay != FilterTimeOfDay)
            SetFiltersLUT();

        if (m_IsSceneGradientColorOn != IsSceneGradientColorOn)
            UpadteGameGraphSetting(); //TEST
    }

    private void SetHeroLight(bool isLightOn)
    {
        m_IsHeroLight = isLightOn;
        HeroSpotLight.enabled = isLightOn;
        HeroPointLight.enabled = isLightOn;
    }

    private void SetFiltersLUT()
    {
        //if (IsSceneGradientColorOn)
        //    return;

        m_FilterTimeOfDay = FilterTimeOfDay;
        switch (FilterTimeOfDay)
        {
            case FiltersTimeOfDay.Day:
                m_lutModel.settings = new UserLutModel.Settings
                {
                    lut = DayLUT,
                    contribution = 1f
                };
                HeroSpotLight.enabled = false;
                HeroPointLight.enabled = false;
                break;
            case FiltersTimeOfDay.Morning:
                m_lutModel.settings = new UserLutModel.Settings
                {
                    lut = MorningLUT,
                    contribution = 1f
                };
                
                HeroSpotLight.enabled = true;
                HeroPointLight.enabled = false;
                break;
            case FiltersTimeOfDay.Night:
                m_lutModel.settings = new UserLutModel.Settings
                {
                    lut = NightLUT,
                    contribution = 1f
                };

                HeroSpotLight.enabled = true;
                HeroPointLight.enabled = true;
                break;
        }
        //TEST// Light HERO ONN where NOT DAY
        //IsSceneGradientColorOn = FilterTimeOfDay != FiltersTimeOfDay.Day;
        //IsHeroLight = FilterTimeOfDay != FiltersTimeOfDay.Day;
        

        m_PostProcessProfile.userLut.enabled = true;
        m_PostProcessProfile.userLut = m_lutModel;
        m_postProcess.profile = m_PostProcessProfile;
    }

    public void UpadteGameGraphSetting()
    {
        m_IsSceneGradientColorOn = IsSceneGradientColorOn;
        if (IsSceneGradientColorOn)
        {
            //RenderSettings.ambientLight = Storage.Palette.SceneSkyColor;
            RenderSettings.ambientSkyColor = Storage.Palette.SceneSkyColor;
            RenderSettings.ambientEquatorColor = Storage.Palette.SceneEquatorColor;
            RenderSettings.ambientGroundColor = Storage.Palette.SceneGroundColor;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        }
        else
        {
            RenderSettings.ambientSkyColor = Color.white;
            RenderSettings.ambientEquatorColor = Color.white;
            RenderSettings.ambientGroundColor = Color.white;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        }

        //TEST
        IsHeroLight = IsSceneGradientColorOn;

        SetFiltersLUT();
    }
}
