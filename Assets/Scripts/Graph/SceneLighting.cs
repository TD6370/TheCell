using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class SceneLighting : MonoBehaviour {

    public Texture2D DayLUT;
    public Texture2D MorningLUT;
    public Texture2D EveningLUT;
    public Texture2D Night_1_LUT;
    public Texture2D Night_2_LUT;
    public Texture2D Night_3_LUT;
    public Texture2D Night_4_LUT;

    public Light HeroSpotLight;
    public Light HeroPointLight;

    private bool m_IsSceneGradientColorOn = false;
    public bool IsSceneGradientColorOn = true;

    private bool m_IsHeroLight = false;
    public bool IsHeroLight = false;

    private PostProcessingProfile m_PostProcessProfile;
    private PostProcessingBehaviour m_postProcess;
    private UserLutModel m_lutModel;

    private Color ColorSky_Night = new Color(0.028f, 0.069f, 0.462f);
    private Color ColorEquator_Night = new Color(0.044f, 0.089f, 0.255f);
    private Color ColorGround_Night = new Color(0.073f, 0.063f, 0.142f);

    private Color ColorSky_Morning = new Color(0.439f, 0.698f, 0.690f);
    private Color ColorEquator_Morning = new Color(0.671f, 0.514f, 0.098f);
    private Color ColorGround_Morning = new Color(1.000f, 0.000f, 0.949f);


    private Color ColorSky_Day = new Color(1f, 1f, 1f);
    private Color ColorEquator_Day = new Color(1f, 1f, 1f);
    private Color ColorGround_Day = new Color(1f, 1f, 1f);

    private Color ColorSky_Evening = new Color(0.724f, 0.478f, 1.000f);
    private Color ColorEquator_Evening = new Color(0.271f, 0.297f, 1.000f);
    private Color ColorGround_Evening = new Color(0.478f, 0.486f, 0.631f);

    private Color SelectedColorSky = new Color(0, 0, 0);
    private Color SelectedColorEquator = new Color(0, 0, 0);
    private Color SelectedColorGround = new Color(0, 0, 0);

    private Dictionary<FiltersTimeOfDay, Color> CollectionColorEquator;
    private Dictionary<FiltersTimeOfDay, Color> CollectionColorGround;
    private Dictionary<FiltersTimeOfDay, Color> CollectionColorSky;

    private int m_gradientStepFilter = 0;
    private int m_PeriodTimeOfDay = 7; //30;
    private float m_delayBaseTOD = 2f;

    [Header("Delay Time of day")]
    [SerializeField]
    private float m_delayTOD = 1f;
    [Header("Percent period lost")]
    [SerializeField]
    private float m_percentTimeListTest;

    enum GradientFilterType
    {
        Sky,
        Equator,
        Ground
    }


    private FiltersTimeOfDay m_FilterTimeOfDay = FiltersTimeOfDay.Day;
    public FiltersTimeOfDay FilterTimeOfDay = FiltersTimeOfDay.Day;

    private int m_CalendarIndex = 1;
    private FiltersTimeOfDay[] m_CalendarTimeOfDay = new FiltersTimeOfDay[]
    {
        FiltersTimeOfDay.Morning,
        FiltersTimeOfDay.Day,
        FiltersTimeOfDay.Day,
        FiltersTimeOfDay.Evening,
        FiltersTimeOfDay.Night,
        FiltersTimeOfDay.Night,
    };

    public enum FiltersTimeOfDay
    {
        //MorningPre,
        Morning,
        //MorningPost,
        //DayPre,
        Day,
        //DayPost,
        //EveningPre,
        Evening,
        //EveningPost,
        //NightPre,
        Night,
        //NightPost
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

        LoadCollectioneGradientColors();
    }


    // Use this for initialization
    void Start () {
        //LoadCollectioneGradientColors();
        StartCoroutine(CyrcleTimeOfDayeCoroutine());
    }
	
	// Update is called once per frame
	void Update () {
        //TEST
        if (m_IsHeroLight != IsHeroLight)
            SetHeroLight(IsHeroLight);

        if (m_FilterTimeOfDay != FilterTimeOfDay)
            SetFiltersLighting();

        if (m_IsSceneGradientColorOn != IsSceneGradientColorOn)
            UpadteGameGraphSetting(); //TEST

        //TEST
        //SetFiltersLighting();
    }
       

    IEnumerator CyrcleTimeOfDayeCoroutine()
    {

        while (true)
        {
            yield return new WaitForSeconds(m_delayTOD);
            if(m_gradientStepFilter == m_PeriodTimeOfDay)
            {
                m_gradientStepFilter = 1;
                NextTimeOfDay();
            }else
            {
                m_gradientStepFilter++;
            }
            SetFiltersLighting();


            //if (FilterTimeOfDay == FiltersTimeOfDay.Day || FilterTimeOfDay == FiltersTimeOfDay.Night)
            //    m_delayTOD = m_delayBaseTOD;

            //Delay
            var previousTime = PreviousTimeOfDay();
            if (previousTime != FiltersTimeOfDay.Day && previousTime != FiltersTimeOfDay.Night)
                m_delayTOD = m_delayBaseTOD; //fast

            else
                m_delayTOD = (m_PeriodTimeOfDay - m_gradientStepFilter);  //slow to fast
            if (m_delayTOD < m_delayBaseTOD)
                m_delayTOD = m_delayBaseTOD;


            //Fight Hero On
            float percentTimeLost = ((float)m_gradientStepFilter / (float)m_PeriodTimeOfDay) * 100f;
            m_percentTimeListTest = percentTimeLost;
            if (FilterTimeOfDay == FiltersTimeOfDay.Morning)
            {
                if (percentTimeLost > 20)
                {
                    //end
                    HeroPointLight.enabled = true;
                    HeroSpotLight.enabled = false;
                }
            }
            if (FilterTimeOfDay == FiltersTimeOfDay.Evening)
            {
                if (percentTimeLost > 60)
                {
                    //end
                    HeroPointLight.enabled = true;
                    HeroSpotLight.enabled = false;
                }
            }
            if (FilterTimeOfDay == FiltersTimeOfDay.Day)
            {
                if (percentTimeLost > 40)
                {
                    //end
                    HeroPointLight.enabled = false;
                    HeroSpotLight.enabled = false;
                }
            }
            if (FilterTimeOfDay == FiltersTimeOfDay.Night)
            {
                if (percentTimeLost > 50)
                {
                    //end
                    HeroPointLight.enabled = true;
                    HeroSpotLight.enabled = true;
                }
            }

        }
    }

    private void SetHeroLight(bool isLightOn)
    {
        m_IsHeroLight = isLightOn;
        HeroSpotLight.enabled = isLightOn;
        HeroPointLight.enabled = isLightOn;
    }

    

    public void NextTimeOfDay()
    {
        //bool isLastTOD = (int)FilterTimeOfDay == Enum.GetNames(typeof(FiltersTimeOfDay)).Length - 1;
        //int n = isLastTOD ? 0 : (int)FilterTimeOfDay + 1;
        //string nameTOD = Enum.GetNames(typeof(FiltersTimeOfDay))[n];
        //FilterTimeOfDay = (FiltersTimeOfDay)Enum.Parse(typeof(FiltersTimeOfDay), nameTOD);

        bool isLastTOD = m_CalendarIndex == m_CalendarTimeOfDay.Length - 1;
        m_CalendarIndex = isLastTOD ? 0 : m_CalendarIndex + 1;
        FilterTimeOfDay = m_CalendarTimeOfDay[m_CalendarIndex];
    }

    public FiltersTimeOfDay GetNextTimeOfDay()
    {
        //bool isLastTOD = (int)FilterTimeOfDay == Enum.GetNames(typeof(FiltersTimeOfDay)).Length - 1;
        //int n = isLastTOD ? 0 : (int)FilterTimeOfDay + 1;
        //string nameTOD = Enum.GetNames(typeof(FiltersTimeOfDay))[n];
        //FilterTimeOfDay = (FiltersTimeOfDay)Enum.Parse(typeof(FiltersTimeOfDay), nameTOD);

        bool isLastTOD = m_CalendarIndex == m_CalendarTimeOfDay.Length - 1;
        int calendarIndex = isLastTOD ? 0 : m_CalendarIndex + 1;
        return m_CalendarTimeOfDay[calendarIndex];
    }

    public FiltersTimeOfDay PreviousTimeOfDay()
    {
        //bool isFirstTOD = (int)FilterTimeOfDay == 0;
        //int lastPosit = Enum.GetNames(typeof(FiltersTimeOfDay)).Length - 1;
        //int n = isFirstTOD ? lastPosit : (int)FilterTimeOfDay - 1;
        //string nameTOD = Enum.GetNames(typeof(FiltersTimeOfDay))[n];
        //return (FiltersTimeOfDay)Enum.Parse(typeof(FiltersTimeOfDay), nameTOD);

        bool isFirstTOD = (int)m_CalendarIndex == 0;
        int lastPosit = m_CalendarTimeOfDay.Length - 1;
        int prevIndex = isFirstTOD ? lastPosit : m_CalendarIndex - 1;
        return m_CalendarTimeOfDay[prevIndex];
    }

    private void LoadCollectioneGradientColors()
    {
        CollectionColorEquator = new Dictionary<FiltersTimeOfDay, Color>()
        {
            {FiltersTimeOfDay.Day, ColorEquator_Day },
            {FiltersTimeOfDay.Evening, ColorEquator_Evening },
            {FiltersTimeOfDay.Night, ColorEquator_Night },
            {FiltersTimeOfDay.Morning, ColorEquator_Morning },
        };
        CollectionColorGround = new Dictionary<FiltersTimeOfDay, Color>()
        {
            {FiltersTimeOfDay.Day, ColorGround_Day },
            {FiltersTimeOfDay.Evening, ColorGround_Evening },
            {FiltersTimeOfDay.Night, ColorGround_Night },
            {FiltersTimeOfDay.Morning, ColorGround_Morning },
        };
        CollectionColorSky = new Dictionary<FiltersTimeOfDay, Color>()
        {
            {FiltersTimeOfDay.Day, ColorSky_Day },
            {FiltersTimeOfDay.Evening, ColorSky_Evening },
            {FiltersTimeOfDay.Night, ColorSky_Night },
            {FiltersTimeOfDay.Morning, ColorSky_Morning },
        };
    }
       
    public void UpadteGameGraphSetting()
    {
        m_IsSceneGradientColorOn = IsSceneGradientColorOn;

        if (IsSceneGradientColorOn)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            //TEST
            //IsHeroLight = true;
        }
        else
        {
            RenderSettings.ambientSkyColor = Color.white;
            RenderSettings.ambientEquatorColor = Color.white;
            RenderSettings.ambientGroundColor = Color.white;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        }

        SetFiltersLighting();
    }

    private void SetFiltersLighting()
    {
        //GRADIENT BASE
        SetSceneGradientColor(FilterTimeOfDay);

        m_FilterTimeOfDay = FilterTimeOfDay;
        switch (FilterTimeOfDay)
        {

            case FiltersTimeOfDay.Day:
                SetFilterLightLUT(DayLUT); //LUT

                //start
                if (PreviousTimeOfDay() != FilterTimeOfDay)
                {
                    HeroPointLight.enabled = true;
                    HeroSpotLight.enabled = false;
                }
                break;
           
            case FiltersTimeOfDay.Evening:
                SetFilterLightLUT(EveningLUT); //LUT

                //start
                if (PreviousTimeOfDay() != FilterTimeOfDay)
                {
                    HeroSpotLight.enabled = false;
                    HeroPointLight.enabled = false;
                }
                break;
            case FiltersTimeOfDay.Night:
                SetFilterLightLUT(Night_2_LUT); //LUT

                //start 
                if (PreviousTimeOfDay() != FilterTimeOfDay)
                {
                    HeroPointLight.enabled = true;
                    HeroSpotLight.enabled = false;
                }
                break;
            case FiltersTimeOfDay.Morning:
                SetFilterLightLUT(MorningLUT); //LUT

                //start
                if (PreviousTimeOfDay() != FilterTimeOfDay)
                {
                    HeroSpotLight.enabled = true;
                    HeroPointLight.enabled = true;
                }
                break;
        }
        if (!IsSceneGradientColorOn) //GRADIENT BASE
        {
            return;//TEST
            m_PostProcessProfile.userLut.enabled = true;
            m_PostProcessProfile.userLut = m_lutModel;
            m_postProcess.profile = m_PostProcessProfile;
        }
    }

    private void SetSceneGradientColor(FiltersTimeOfDay tod)
    {
        //TEST COLOR 
        bool isTest = false;//  true;

        if (!IsSceneGradientColorOn)
            return;

        if (isTest)
        {
            RenderSettings.ambientSkyColor = Storage.Palette.SceneSkyColor;
            RenderSettings.ambientEquatorColor = Storage.Palette.SceneEquatorColor;
            RenderSettings.ambientGroundColor = Storage.Palette.SceneGroundColor;
        }
        else
        {
            RenderSettings.ambientSkyColor = SelectedColorSky = GenericGradientColorTimeOfDate(GradientFilterType.Sky);
            RenderSettings.ambientEquatorColor = SelectedColorEquator = GenericGradientColorTimeOfDate(GradientFilterType.Equator);
            RenderSettings.ambientGroundColor = SelectedColorGround = GenericGradientColorTimeOfDate(GradientFilterType.Ground);
        }
    }
        
    private Color GenericGradientColorTimeOfDate(GradientFilterType typeGradient)
    {
        Color prevousFilterColor = new Color();
        Color currentFilterColor = new Color();
        FiltersTimeOfDay prevousTOD = PreviousTimeOfDay();

        switch (typeGradient)
        {
            case GradientFilterType.Sky:
                prevousFilterColor = CollectionColorSky[prevousTOD];
                currentFilterColor = CollectionColorSky[FilterTimeOfDay];
                break;
            case GradientFilterType.Equator:
                prevousFilterColor = CollectionColorEquator[prevousTOD];
                currentFilterColor = CollectionColorEquator[FilterTimeOfDay];
                break;
            case GradientFilterType.Ground:
                prevousFilterColor = CollectionColorGround[prevousTOD];
                currentFilterColor = CollectionColorGround[FilterTimeOfDay];
                break;
        }
        
        float redNew = CalculateNextColor(prevousFilterColor.r, currentFilterColor.r);
        float greenNew = CalculateNextColor(prevousFilterColor.g, currentFilterColor.g);
        float blueNew = CalculateNextColor(prevousFilterColor.b, currentFilterColor.b);
        
        return new Color(redNew, greenNew, blueNew);
    }

    private float CalculateNextColor(float preColor, float curColor)
    {
        float resultColor = 0f;
        if (preColor < curColor)
        {
            float step = (curColor - preColor) / m_PeriodTimeOfDay;
            resultColor = preColor + (step * m_gradientStepFilter);
        }
        else if(preColor >= curColor)
        {
            float step = (preColor - curColor) / m_PeriodTimeOfDay;
            resultColor = preColor - (step * m_gradientStepFilter);
        }
        return resultColor;
    }
       

    private void SetFilterLightLUT(Texture2D lut)
    {
        if (IsSceneGradientColorOn)
            return;

        m_lutModel.settings = new UserLutModel.Settings
        {
            lut = lut,
            contribution = 1f
        };
    }
          

    //private void UserLutModel.Settings CreateUserLutModel(Texture2D setTUT)
    //{
    //    UserLutModel.Settings model = new UserLutModel.Settings
    //    {
    //        lut = setTUT,
    //        contribution = 1f
    //    };
    //    return model;
    //}

    
}
