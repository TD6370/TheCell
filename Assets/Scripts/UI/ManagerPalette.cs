using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPalette : MonoBehaviour {

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
            {"","".ToColor() },

        };
    }

    // Use this for initialization
    void Start () {
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
