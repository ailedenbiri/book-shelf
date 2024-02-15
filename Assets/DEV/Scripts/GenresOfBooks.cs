using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Genre { Love, Cowboy, Education, Noir, Health, SelfImprovement, ChildBook, Fantasy, Horror };
public enum ColorOfBook { Red, Purple, Blue, Empty };


[CreateAssetMenu(menuName = "ScriptableObjects/BookSettings")]
public class BookSettingsScriptableObject : ScriptableObject
{
    public Genre Genre;
    public ColorOfBook ColorOfBook;
}