using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPalette : MonoBehaviour {

    public Color ColorVood;//= Color.clear; //"#379200".ToColor();
    public Color ColorRock;// = Color.clear; //"#77A7C2".ToColor();
    public Color ColorUfo;// = Color.clear; //"#FF527C".ToColor();
    public Color ColorElka;// = Color.clear; //"#025400".ToColor();
    public Color ColorWallRock;// = Color.clear; //"#7F7F7F".ToColor();
    public Color ColorWallWood;// = Color.clear; //"#9F673E".ToColor();

    public static Dictionary<string, Color> PaletteColors = new Dictionary<string, Color>();


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

    // Use this for initialization
    void Start () {
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
