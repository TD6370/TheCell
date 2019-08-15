//using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.U2D;

public class ManagerPalette : MonoBehaviour {

    public Color SceneSkyColor = Color.white;
    public Color SceneEquatorColor = Color.white;
    public Color SceneGroundColor = Color.white;

    public Color ColorVood;//= Color.clear; //"#379200".ToColor();
    public Color ColorRock;// = Color.clear; //"#77A7C2".ToColor();
    public Color ColorUfo;// = Color.clear; //"#FF527C".ToColor();
    public Color ColorElka;// = Color.clear; //"#025400".ToColor();
    public Color ColorWallRock;// = Color.clear; //"#7F7F7F".ToColor();
    public Color ColorWallWood;// = Color.clear; //"#9F673E".ToColor();

    public static Dictionary<string, Color> PaletteColors = new Dictionary<string, Color>();
    public Dictionary<string, Texture2D> TexturesPrefabs = new Dictionary<string, Texture2D>();
    public Dictionary<string, Sprite> SpritesPrefabs = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> SpritesWorldPrefabs = new Dictionary<string, Sprite>();
    public Dictionary<string, Texture2D> TexturesMaps = new Dictionary<string, Texture2D>();
    //private Dictionary<string, Texture2D> m_TexturesMaps;
    private Dictionary<string, Sprite> m_SpritesMaps = null;

    public SpriteAtlas SpriteAtlasPrefab;
    public SpriteAtlas SpriteAtlasPrefabWorld;
    public SpriteAtlas SpriteAtlasMapPrefab;

    private void Awake()
    {
        PaletteColors = new Dictionary<string, Color>
        {
            {"SpriteBossLizard",ColorBossLizard },
            {"SpriteBossRed",ColorBossRed },
            {"SpriteBossBandos",ColorBossBandos },
            {"SpriteBossBooble",ColorBossBooble },
            {"SpriteBossAlien",ColorBossAlien },
            {"SpriteBossDroid",ColorBossDroid },
            {"SpriteBossArm",ColorBossArm },
            {"SpriteBoss",ColorBoss },
            {"PrefabVood","#379200".ToColor() },
            {"PrefabRock","#77A7C2".ToColor() },
            {"PrefabBoss","#F152FF".ToColor() },
            {"PrefabUfo",ColorUfo },
            {"PrefabElka","#025400".ToColor() },
            {"PrefabWallRock","#7F7F7F".ToColor() },
            {"PrefabWallWood","#9F673E".ToColor() },
            //{"PrefabElka",ColorElka },
            //{"PrefabWallRock",ColorWallRock },
            //{"PrefabWallWood",ColorWallWood },

        };

       
        //{ "PrefabVood",ColorVood },
        //{ "PrefabRock",ColorRock },
    }

    public Sprite[] GetSpritesAtlasPrefab()
    {
        Sprite[] spritesAtlas = new Sprite[SpriteAtlasPrefab.spriteCount];
        SpriteAtlasPrefab.GetSprites(spritesAtlas);
        return spritesAtlas;
    }

    public Sprite[] GetSpritesAtlasPrefabWorld()
    {
        Sprite[] spritesAtlas = new Sprite[SpriteAtlasPrefabWorld.spriteCount];
        SpriteAtlasPrefabWorld.GetSprites(spritesAtlas);
        return spritesAtlas;
    }

    

    // Use this for initialization
    void Start () {
       

    }

    #region Load map icons
    public void LoadSpritePrefabs()
    {
        List<Sprite> spritesAtlas = new List<Sprite>();

        Sprite[] spritesPrefabsAtlas = GetSpritesAtlasPrefab();
        Sprite[] spritesPrefabsAtlasWorld = GetSpritesAtlasPrefabWorld();
        spritesAtlas.AddRange(spritesPrefabsAtlas);
        spritesAtlas.AddRange(spritesPrefabsAtlasWorld);

        foreach (var sprt in spritesAtlas)
        {
            sprt.name = sprt.name.ClearClone();

            try
            {
                SpritesWorldPrefabs.Add(sprt.name, sprt);
            }catch(System.Exception x)
            {
                Debug.Log("######### LoadSpritePrefabs " + x);
                Debug.Log("######### LoadSpritePrefabs sprt.name=" + sprt.name);
            }

            if (sprt.name.IndexOf("Wall") != -1)
            {
                sprt.name = "Prefab" + sprt.name;
            }
            SpritesPrefabs.Add(sprt.name, sprt);
            //Debug.Log("ADD spritesPrefabsAtlas: " + nameSprite);
        }

        InitMapIconsTextures();
    }

    public void InitMapIconsTextures()
    {
        
        TexturesMaps = new Dictionary<string, Texture2D>
        {
            {"PrefabVood", Storage.Map.textureVood },
            {"PrefabElka", Storage.Map.textureElka },
            {"PrefabRock", Storage.Map.textureRock },
            {"PrefabWallRock", Storage.Map.textureWallRock },
            {"PrefabWallWood", Storage.Map.textureWallWood },
            {"PrefabField", Storage.Map.textureField },
            {"PrefabHero", Storage.Map.textureHero },
            {"Parket", Storage.Map.textureParket },
        };
        var init = SpritesMaps;
    }
    
    public Dictionary<string, Sprite> SpritesMaps
    {
        get
        {
            if (m_SpritesMaps == null)
            {
                m_SpritesMaps = new Dictionary<string, Sprite>();
                Sprite[] _sprites = GetSpritesAtlasMapPrefab();
                string nameSprite;
                int indS = 0;
                foreach (Sprite sprt in _sprites)
                {
                    var textureItem =  sprt.texture;
                    //Texture2D textureItem = _sprites[indS].texture;
                    //indS++;

                    nameSprite = sprt.name.Replace("(Clone)", "");
                    nameSprite = nameSprite.Replace("Map", "");

                    Texture2D textureRes = Resources.Load<Texture2D>("Textures/Map/GridIcons/" + nameSprite + "Map");
                    if (textureRes != null)
                        textureItem = textureRes;

                    m_SpritesMaps.Add(nameSprite, sprt);
                    if (TexturesMaps.ContainsKey(nameSprite))
                        Debug.Log("######## EXIST TEXTURE MAP: " + nameSprite);
                    else
                        TexturesMaps.Add(nameSprite, textureItem);
                }
            }
            return m_SpritesMaps;
        }

    }

    public Sprite[] GetSpritesAtlasMapPrefab()
    {
        Sprite[] spritesAtlas = new Sprite[SpriteAtlasMapPrefab.spriteCount];
        SpriteAtlasMapPrefab.GetSprites(spritesAtlas);
        return spritesAtlas;
    }

    //public Sprite GetSpriteFromAtlasPrefab(string prefabType)
    //{
    //    string nameTexture = NamesTexturesMaps[prefabType];
    //    //Sprite[] spritesAtlas = new Sprite[SpriteAtlasMapPrefab.spriteCount];
    //    //SpriteAtlasMapPrefab.GetSprites(spritesAtlas);
    //    //SpriteAtlasMapPrefab
    //    return SpriteAtlasMapPrefab.GetSprite(nameTexture);// .Find(p=>p.name=="");
    //}
#endregion


    public static Color GetColor(string nameColor)
    {
        return PaletteColors[nameColor];
    }

    // Update is called once per frame
    void Update () {
		
	}

    public static Color ColorBossLizard
    {
        get
        {
            return "#85FF72".ToColor();
        }
    }

    public static Color ColorBossRed
    {
        get
        {
            return "#D44E4E".ToColor();
        }
    }

    public static Color ColorBossBandos
    {
        get
        {
            return "#FBE98D".ToColor();
        }
    }

    public static Color ColorBossBooble
    {
        get
        {
            return "#8DFBF4".ToColor();
        }
    }

    public static Color ColorBossAlien
    {
        get
        {
            return "#B6BDFF".ToColor();
        }
    }
    public static Color ColorBossDroid
    {
        get
        {
            return "#CDB6FF".ToColor();
        }
    }
    public static Color ColorBossArm
    {
        get
        {
            return "#B6FFD3".ToColor();
        }
    }
    public static Color ColorBoss
    {
        get
        {
            return "#EBEBEB".ToColor();
        }
    }
    //public static Color Color
    //{
    //    get
    //    {
    //        return "".ToColor();
    //    }
    //}

    
    


}
