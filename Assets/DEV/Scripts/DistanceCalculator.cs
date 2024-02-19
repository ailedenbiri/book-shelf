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
            ShelfGrid g = null;
            if (!pivotOnMiddle)
            {
                g = Instantiate(GameAssets.i.pfShelfGrid, this.transform.position + Vector3.right * 0.2f + (i * sizeCoefficient * Vector3.right), GameAssets.i.pfShelfGrid.transform.rotation);
            }
            else
            {
                g = Instantiate(GameAssets.i.pfShelfGrid, this.transform.GetComponent<Renderer>().bounds.center + (i * sizeCoefficient * Vector3.right), GameAssets.i.pfShelfGrid.transform.rotation);
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
        bookPos.y = this.transform.position.y;

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
                GameManager.instance.CountBooks();
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
        bookPos.y = this.transform.position.y;

        return bookPos;
    }



}
