using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class BookController : MonoBehaviour
{
    public static BookController instance;

    [SerializeField] ParticleSystem particleEffect;
    [SerializeField] ParticleSystem missedParticle;

    public Book selectedBook;
    [HideInInspector] public GameObject selectedBookTransparent = null;
    [HideInInspector] public ShelfGrid currentSelectedGrid = null;

    private LayerMask gridLayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    private void Start()
    {

        if (particleEffect != null)
        {
            particleEffect.Stop();
        }
    }



    private void Update()
    {
        if (GameManager.instance.state == GameManager.GameState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Book")))
                {
                    if (hit.transform.TryGetComponent(out Book bookTransform))
                    {
                        if (selectedBook != null)
                        {
                            if (selectedBook == bookTransform)
                            {
                                GameManager.instance.state = GameManager.GameState.Waiting;
                                ReturnToOriginalPosition(selectedBook.transform, true);
                                selectedBook = null;
                                return;
                            }
                            else
                            {
                                GameManager.instance.state = GameManager.GameState.Waiting;
                                ReturnToOriginalPosition(selectedBook.transform);
                            }
                        }
                        else
                        {
                            GameManager.instance.LockBooks();
                        }
                        if (bookTransform.placed)
                        {
                            return;
                        }

                        if (selectedBookTransparent != null)
                        {
                            Destroy(selectedBookTransparent.gameObject);
                            selectedBookTransparent = null;
                        }

                        selectedBook = bookTransform;

                        //Create transparent preview of selected book
                        selectedBookTransparent = Instantiate(selectedBook.gameObject, Vector3.zero, Quaternion.Euler(new Vector3(0f, -180f, 0f)));
                        selectedBookTransparent.transform.localScale = Vector3.zero;
                        selectedBookTransparent.GetComponent<Renderer>().material = GameAssets.i.bookMaterialsTransparent[(int)selectedBook.ColorOfBook];
                        selectedBookTransparent.GetComponent<Collider>().enabled = false;

                        GameManager.instance.state = GameManager.GameState.Waiting;
                        PlayBookAnimation(selectedBook.transform);
                    }
                }
                return;
            }
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (selectedBook != null)
                {
                    if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Bookshelf")))
                    {

                        if (hit.transform.TryGetComponent<ShelfGrid>(out ShelfGrid g))
                        {
                            currentSelectedGrid = g;
                            selectedBookTransparent.transform.localScale = selectedBook.transform.localScale;
                            if (g.shelf.GetPos(selectedBook.thickness, g) != Vector3.zero)
                            {
                                selectedBookTransparent.transform.position = g.shelf.GetPos(selectedBook.thickness, g);
                            }
                            else
                            {
                                currentSelectedGrid = null;
                                selectedBookTransparent.transform.localScale = Vector3.zero;
                            }

                        }
                        else
                        {
                            currentSelectedGrid = null;
                            selectedBookTransparent.transform.localScale = Vector3.zero;
                        }
                    }
                    else
                    {
                        currentSelectedGrid = null;
                        selectedBookTransparent.transform.localScale = Vector3.zero;
                    }

                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                if (currentSelectedGrid != null && selectedBook != null)
                {
                    GameManager.instance.state = GameManager.GameState.Waiting;
                    DistanceCalculator d = currentSelectedGrid.shelf;
                    bool bookPlaced = d.AddBook(selectedBook, currentSelectedGrid);
                    Destroy(selectedBookTransparent.gameObject);
                    selectedBookTransparent = null;
                    currentSelectedGrid = null;
                }
            }
        }

    }

    public void DropBookIfSelected()
    {
        if (selectedBook != null)
        {
            GameManager.instance.state = GameManager.GameState.Waiting;
            ReturnToOriginalPosition(selectedBook.transform, true);
            selectedBook = null;
            return;
        }
    }


    private void PlayBookAnimation(Transform bookTransform)
    {

        bookTransform.DOKill();
        bookTransform.GetComponent<Book>().UpdatePositions();

        bookTransform.DORotate(new Vector3(0f, -90f, 0f), 0.5f);
        Debug.Log(Camera.main.transform.position + Vector3.forward * 4.5f - Vector3.up * 1.5f);
        bookTransform.DOMove(Camera.main.transform.position + Vector3.forward * 4.5f - Vector3.up * 2f, 0.5f).OnComplete(() =>
        {
            GameManager.instance.state = GameManager.GameState.Playing;
        });
    }

    public void ReturnToOriginalPosition(Transform bookTransform, bool unlockBooks = false)
    {
        bookTransform.DOKill();

        bookTransform.DOMove(bookTransform.GetComponent<Book>().startPos, 0.5f).SetEase(Ease.OutQuad);
        bookTransform.DORotateQuaternion(bookTransform.GetComponent<Book>().startRot, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            if (unlockBooks)
            {
                GameManager.instance.UnlockBooks();
            }
            GameManager.instance.state = GameManager.GameState.Playing;
        });


    }

    public void PlayMissedParticle(Vector3 position)
    {
        missedParticle.transform.position = position;
        missedParticle.Play();
    }

    public void PlayParticleEffect(Vector3 position, Book book)
    {


        if (particleEffect != null)
        {
            Transform firstChild = particleEffect.transform.GetChild(0);
            Transform thirdChild = particleEffect.transform.GetChild(1);
            Transform fourthChild = particleEffect.transform.GetChild(3);
            ParticleSystem childParticleSystem = thirdChild.GetComponent<ParticleSystem>();
            ParticleSystem childParticleSystem2 = firstChild.GetComponent<ParticleSystem>();
            ParticleSystem childParticleSystem3 = fourthChild.GetComponent<ParticleSystem>();
            var color = childParticleSystem.colorOverLifetime;
            var color2 = childParticleSystem3.colorOverLifetime;
            Gradient grad = new Gradient();
            Gradient grad2 = new Gradient();
            switch (book.ColorOfBook)
            {
                case ColorOfBook.Blue:
                    Debug.Log("Blue");

                    grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.blue, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
                    grad2.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.blue, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
                    particleEffect.startColor = Color.blue;
                    childParticleSystem2.startColor = Color.blue;

                    color.color = grad;
                    color2.color = grad2;

                    break;
                case ColorOfBook.Purple:
                    Debug.Log("purple");
                    grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.magenta, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
                    grad2.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.magenta, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
                    particleEffect.startColor = Color.magenta;
                    childParticleSystem2.startColor = Color.magenta;
                    color.color = grad;
                    color2.color = grad2;
                    break;
                case ColorOfBook.Red:
                    Debug.Log("Red");
                    grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.red, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
                    grad2.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
                    particleEffect.startColor = Color.red;
                    childParticleSystem2.startColor = Color.red;
                    color.color = grad;
                    color2.color = grad2;
                    break;

                default:

                    break;
            }


            particleEffect.transform.position = position;
            particleEffect.Play();


            StartCoroutine(StopParticleEffectAfterDelay(particleEffect.main.duration));
        }
    }

    private IEnumerator StopParticleEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(1f);


        if (particleEffect != null)
        {
            particleEffect.Stop();
        }
    }


}
