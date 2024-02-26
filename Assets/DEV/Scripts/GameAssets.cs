using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }

    public ShelfGrid pfShelfGrid;

    public Color[] bookColors;

    public Material[] bookMaterialsTransparent;

    public GameObject[] pfLabels;

    public Sprite[] genreSprites;

    public SpriteRenderer pfBookSprite;
    private Vector3[] bookSpriteScales = { new Vector3(0.0025f, 0.005f, 0.01f), new Vector3(0.005f, 0.0075f, 0.01f), new Vector3(0.0066f, 0.01f, 0.01f) };

    public void AddSpriteToBook(Book b)
    {
        SpriteRenderer g = Instantiate(pfBookSprite, b.transform);
        g.transform.localPosition = new Vector3(0, 0.225f, 0.19f);
        g.transform.localScale = bookSpriteScales[b.thickness - 1];
        g.sprite = genreSprites[(int)b.Genre];
    }
}
