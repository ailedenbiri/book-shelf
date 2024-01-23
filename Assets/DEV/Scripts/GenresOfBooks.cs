using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum Genre {Love, Harror, Cowboy,Education};
    public enum ColorOfBooks { Red, Green, Blue };


[CreateAssetMenu(menuName = "ScriptableObjects/BookSettings")]
public class BookSettingsScriptableObject : ScriptableObject
{
    public Genre Genre;
    public ColorOfBooks ColorOfBooks;
}