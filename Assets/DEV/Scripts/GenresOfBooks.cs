using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum Genre {Love, Harror, Cowboy,Education};
    public enum ColorOfBook { Red, Green, Blue };


[CreateAssetMenu(menuName = "ScriptableObjects/BookSettings")]
public class BookSettingsScriptableObject : ScriptableObject
{
    public Genre Genre;
    public ColorOfBook ColorOfBook;
    public GameObject PrefabOfBook;
}