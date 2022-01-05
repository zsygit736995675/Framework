using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "SY_SubMenue/CreateLanguageSetting")]
public class LanguageSetting : ScriptableObject
{
    public List<TagSprite> sprites;
}

[Serializable]
public class TagSprite
{
    public string tag;
    public Sprite zhSprite;
    public Sprite enSprite;
    public Sprite hiSprite;
}
