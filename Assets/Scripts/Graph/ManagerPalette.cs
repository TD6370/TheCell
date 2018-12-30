//using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.U2D;

public class ManagerPalette : MonoBehaviour {

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

    public SpriteAtlas SpriteAtlasPrefab;
    public SpriteAtlas SpriteAtlasPrefabWorld;

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


    public void LoadSpritePrefabs()
    {

        //PaletteColors = new Dictionary<string, Color>
        //{
        //    {"SpriteBossLizard",ColorBossLizard },
        //    {"SpriteBossRed",ColorBossRed },
        //    {"SpriteBossBandos",ColorBossBandos },
        //    {"SpriteBossBooble",ColorBossBooble },
        //    {"SpriteBossAlien",ColorBossAlien },
        //    {"SpriteBossDroid",ColorBossDroid },
        //    {"SpriteBossArm",ColorBossArm },
        //    {"SpriteBoss",ColorBoss },
        //    {"PrefabVood",ColorVood },
        //    {"PrefabRock",ColorRock },
        //    {"PrefabBoss","#F152FF".ToColor() },
        //    {"PrefabUfo",ColorUfo },
        //    {"PrefabElka",ColorElka },
        //    {"PrefabWallRock",ColorWallRock },
        //    {"PrefabWallWood",ColorWallWood },

        //};

        List<Sprite> spritesAtlas = new List<Sprite>();

        Sprite[] spritesPrefabsAtlas = GetSpritesAtlasPrefab();
        Sprite[] spritesPrefabsAtlasWorld = GetSpritesAtlasPrefabWorld();
        spritesAtlas.AddRange(spritesPrefabsAtlas);
        spritesAtlas.AddRange(spritesPrefabsAtlasWorld);

        foreach (var sprt in spritesAtlas)
        {
            sprt.name = sprt.name.ClearClone();

            SpritesWorldPrefabs.Add(sprt.name, sprt);

            if (sprt.name.IndexOf("Wall") != -1)
            {
                sprt.name = "Prefab" + sprt.name;
            }
            SpritesPrefabs.Add(sprt.name, sprt);
            //Debug.Log("ADD spritesPrefabsAtlas: " + nameSprite);
        }

        TexturesMaps = new Dictionary<string, Texture2D>
        {
            {"PrefabVood", Storage.Map.textureVood },
            {"PrefabElka", Storage.Map.textureElka },
            {"PrefabRock", Storage.Map.textureRock },
            {"PrefabWallRock", Storage.Map.textureWallRock },
            {"PrefabWallWood", Storage.Map.textureWallWood },
            {"PrefabField", Storage.Map.textureField },
            {"PrefabHero", Storage.Map.textureHero },

        };
    }

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
