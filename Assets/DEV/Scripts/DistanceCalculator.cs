using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DistanceCalculator : MonoBehaviour
{
    [SerializeField] bool pivotOnMiddle = false;

    public int shelfLength = 8;
    public int gridCount = 8;
    List<ShelfGrid> grids = new List<ShelfGrid>();
    [SerializeField] Genre[] gridGenres;
    [SerializeField] int[] gridColors;

    public float sizeCoefficient;

    private void Start()
    {
        CreateGrids();
    }

    public void CreateGrids()
    {
        float loopStartPoint = 0;
        float loopEndPoint = shelfLength;
        if (pivotOnMiddle)
        {
            loopStartPoint = (float)-(shelfLength - 1) / 2;
            loopEndPoint = (float)shelfLength / 2;

        }
        int counter = 0;
        float i = loopStartPoint;
        while (i < loopEndPoint)
        {
            Debug.Log(i);
            ShelfGrid g;
            if (!pivotOnMiddle)
            {
                g = Instantiate(GameAssets.i.pfShelfGrid, this.transform.position + Vector3.right * 0.2f + (i * sizeCoefficient * Vector3.right), GameAssets.i.pfShelfGrid.transform.rotation);
            }
            else
            {
                g = Instantiate(GameAssets.i.pfShelfGrid, this.transform.GetComponent<Renderer>().bounds.center+ (i * sizeCoefficient * Vector3.right), GameAssets.i.pfShelfGrid.transform.rotation);
            }
            g.genre = gridGenres[counter];
            ColorOfBook color = ColorOfBook.Empty;
            switch (gridColors[counter])
            {
                case 1:
                    color = ColorOfBook.Red;
                    break;
                case 2:
                    color = ColorOfBook.Purple;
                    break;
                case 3:
                    color = ColorOfBook.Blue;
                    break;
                case 4:
                    color = ColorOfBook.Yellow;
                    break;
                case 5:
                    color = ColorOfBook.Green;
                    break;
                default:
                    break;
            }

            g.color = color;
            g.shelf = this;
            grids.Add(g);
            g.UpdateColor();

            counter++;
            i += 1;
        }
        SetupLabels();
    }
    public bool AddBook(Book book, ShelfGrid grid)
    {
        List<ShelfGrid> bookReplacingGrids = new List<ShelfGrid>();
        int gridStart = grids.IndexOf(grid);
        for (int i = gridStart; i < gridStart + book.thickness; i++)
        {
            if (i == (grids.Count) || !grids[i].isEmpty)
            {
                GameManager.instance.state = GameManager.GameState.Playing;
                return false;
            }
            bookReplacingGrids.Add(grids[i]);
        }

        bool replaced = bookReplacingGrids.All(x => (x.color == book.ColorOfBook && x.genre == book.Genre));

        Vector3 bookPos = Vector3.zero;
        foreach (var item in bookReplacingGrids)
        {
            bookPos += item.transform.position;
        }
        bookPos /= bookReplacingGrids.Count;
        bookPos.y = this.transform.position.y + 0.05f;

        //BOOK REPLACING
        Transform bookTransform = book.transform;
        bookTransform.DOKill();
        //bookTransform.DOMove(targetPosition, 1.75f).SetEase(Ease.OutQuart);
        //bookTransform.DORotate(new Vector3(0f, -180f, 0f), 1.75f).SetEase(Ease.InOutBack).OnComplete(() => PlayParticleEffect(bookTransform.position));

        Sequence bookSeq = DOTween.Sequence();

        bookSeq.Append(bookTransform.DOMove(bookPos - Vector3.forward * 1f, 1.45f));
        bookSeq.Join(bookTransform.DORotate(new Vector3(0f, -180f, 0f), 1.45f));
        bookSeq.Append(bookTransform.DOMove(bookPos, 0.3f));
        BookController.instance.selectedBook = null;
        if (replaced)
        {
            bookSeq.AppendCallback(() =>
            {
                BookController.instance.PlayParticleEffect(bookPos + Vector3.up * 0.5f - Vector3.forward * 0.5f, bookTransform.GetComponent<Book>());
                GameManager.instance.state = GameManager.GameState.Playing;
                book.GetComponent<Collider>().enabled = false;
                book.placed = true;
                foreach (var item in bookReplacingGrids)
                {
                    item.isEmpty = false;
                }
                Taptic.Medium();
                GameManager.instance.CountBooks();
                GameManager.instance.UnlockBooks();
            });
        }
        else
        {
            bookSeq.AppendCallback(() =>
            {
                BookController.instance.PlayMissedParticle(bookPos + Vector3.up * 0.5f - Vector3.forward * 0.7f);
                DOVirtual.DelayedCall(0.6f, () => BookController.instance.ReturnToOriginalPosition(bookTransform, true));
                GameManager.instance.WrongShelf();
            });
        }

        return true;
    }

    public Vector3 GetPos(int t, ShelfGrid grid)
    {
        List<ShelfGrid> bookReplacingGrids = new List<ShelfGrid>();
        int gridStart = grids.IndexOf(grid);
        for (int i = gridStart; i < gridStart + t; i++)
        {
            if (i == (grids.Count) || !grids[i].isEmpty)
            {
                GameManager.instance.state = GameManager.GameState.Playing;
                return Vector3.zero;
            }
            bookReplacingGrids.Add(grids[i]);
        }

        Vector3 bookPos = Vector3.zero;
        foreach (var item in bookReplacingGrids)
        {
            bookPos += item.transform.position;
        }
        bookPos /= bookReplacingGrids.Count;
        bookPos.y = this.transform.position.y + 0.05f;

        return bookPos;
    }

    public void SetupLabels()
    {
        List<Genre> genreList = new List<Genre>(gridGenres);
        List<Genre> genreListDifferent = genreList.Distinct().ToList();
        genreListDifferent.RemoveAll(x => x == Genre.Blank);
        List<int> genreCounts = new List<int>();
        for (int i = 0; i < genreListDifferent.Count; i++)
        {
            genreCounts.Add(genreList.Count(x => x == genreListDifferent[i]));
        }

        GameObject g = Instantiate(GameAssets.i.pfLabels[genreListDifferent.Count - 1], FindLabelCoords(GetComponent<MeshFilter>(), genreListDifferent.Count),
            GameAssets.i.pfLabels[genreListDifferent.Count - 1].transform.rotation);
        if (genreListDifferent.Count > 0)
        {
            g.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = GameAssets.i.genreSprites[(int)genreListDifferent[0]];
            g.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = "x" + genreCounts[0];
        }
        if (genreListDifferent.Count > 1)
        {
            g.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = GameAssets.i.genreSprites[(int)genreListDifferent[1]];
            g.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshPro>().text = "x" + genreCounts[1];
        }
        if (genreListDifferent.Count > 2)
        {
            g.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sprite = GameAssets.i.genreSprites[(int)genreListDifferent[2]];
            g.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshPro>().text = "x" + genreCounts[2];
        }
    }

    public Vector3 FindLabelCoords(MeshFilter renderer, int offsetCount)
    {
        Matrix4x4 localToWorld = transform.localToWorldMatrix;
        float x = float.MaxValue, y = float.MaxValue, z = float.MaxValue;
        for (int i = 0; i < renderer.mesh.vertices.Length; i++)
        {
            Vector3 pos = localToWorld.MultiplyPoint3x4(renderer.mesh.vertices[i]);
            if (pos.x < x) x = pos.x;
            if (pos.y < y) y = pos.y;
            if (pos.z < z) z = pos.z;
        }
        z -= 0.05f;
        y += 0.25f;
        float[] offsets = { -0.217288f, -0.325304f, -0.462556f };
        x += offsets[offsetCount - 1];
        return new Vector3(x, y, z);
    }



}
