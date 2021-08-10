using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;


    //
    public static GameAssets instance {
        get {
            //to get the reference of the script
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();

            return _i;
        }
    }

    public Sprite CodeMonkeyHeadSprite;
}
