using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
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
    public Dictionary<string, Texture2D> TexturesMaps = new Dictionary<string, Texture2D>();

    public SpriteAtlas SpriteAtlasPrefab;

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

        Sprite[] spritesPrefabsAtlas = GetSpritesAtlasPrefab();
        foreach (var sprt in spritesPrefabsAtlas)
        {
            //string namePrefab = sprt.name.GetNamePrefabByTextureMap();

            //Texture2D _texture = sprt.texture;
            //Texture2D _texture = SpriteUtility.GetSpriteTexture(sprt, false /* getAtlasData */);
            //_texture.Apply();
            string nameSprite= sprt.name.Replace("(Clone)", "");
            SpritesPrefabs.Add(nameSprite, sprt);
            Debug.Log("ADD spritesPrefabsAtlas: " + nameSprite);
        }

        TexturesPrefabs = new Dictionary<string, Texture2D>
        {
            {"PrefabVood", Storage.GridData.PrefabVood.GetComponent<SpriteRenderer>().sprite.texture },
            {"PrefabElka", Storage.GridData.PrefabElka .GetComponent<SpriteRenderer>().sprite.texture },
            {"PrefabRock", Storage.GridData.PrefabRock.GetComponent<SpriteRenderer>().sprite.texture },
            {"PrefabWallRock", Storage.GridData.PrefabWallRock.GetComponent<SpriteRenderer>().sprite.texture },
            {"PrefabWallWood", Storage.GridData.PrefabWallWood.GetComponent<SpriteRenderer>().sprite.texture },
            //{"PrefaField", Storage.GridData .GetComponent<SpriteRenderer>().sprite.texture },
        };

        //---------------- Atlas
        //Sprite[] spritesPrefabs = Storage.Map.GetSpritesAtlasPrefab();
        //TexturesMaps = new Dictionary<string, Texture2D>();
        //foreach (var sprt in spritesPrefabs)
        //{
        //    string namePrefab = sprt.name.GetNamePrefabByTextureMap();

        //    //Texture2D _texture = sprt.texture;
        //    Texture2D _texture = SpriteUtility.GetSpriteTexture(sprt, false /* getAtlasData */);
        //    //_texture.Apply();
        //    TexturesMaps.Add(namePrefab, _texture);
        //}
        //---------------- Atlas

        //Resources.Load ()
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

        //public Texture2D textureField;
        //public Texture2D textureRock;
        //public Texture2D textureVood;
        //public Texture2D textureElka;
        //public Texture2D textureWallRock;
        //public Texture2D textureWallWood;
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
