using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BookController : MonoBehaviour
{
    public static BookController instance;

    private Transform book;
    [SerializeField] private ParticleSystem particleEffect;


    public Book selectedBook;
    private bool moved = false;

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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.TryGetComponent(out Book bookTransform))
                {
                    if (selectedBook != null)
                    {
                        if (selectedBook == bookTransform)
                        {
                            ReturnToOriginalPosition(selectedBook.transform, true);
                        }
                        else
                        {
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

                    selectedBook = bookTransform;
                    PlayBookAnimation(selectedBook.transform);
                }

                else if (hit.collider.CompareTag("Respawn") && selectedBook != null)
                {
                    Vector3 bookPoint = hit.collider.GetComponentInParent<DistanceCalculator>().AddPositionCalculate(selectedBook);
                    PlaceBookOnShelf(selectedBook.transform, bookPoint);
                    selectedBook.placed = true;
                    GameManager.instance.UnlockBooks();
                    selectedBook.GetComponent<Collider>().enabled = false;
                    selectedBook = null;
                    moved = false;
                }
            }
        }
    }


    private void PlayBookAnimation(Transform bookTransform)
    {
        bookTransform.DOKill();
        bookTransform.GetComponent<Book>().UpdatePositions();

        bookTransform.DORotate(new Vector3(0f, -90f, 0f), 1f);
        bookTransform.DOMoveX(0.10f, 0.5f);
        bookTransform.DOMoveY(0.5f, 0.5f);
        bookTransform.DOMoveZ(0f, 0.5f);
    }

    private void PlaceBookOnShelf(Transform bookTransform, Vector3 targetPosition)
    {


        bookTransform.DOKill();
        Transform temp = bookTransform;
        bookTransform.DOMove(targetPosition, 1f).SetEase(Ease.OutQuad);
        bookTransform.DORotate(new Vector3(0f, -180f, 0f), 1f).SetEase(Ease.OutQuad).OnComplete(() => PlayParticleEffect(temp.position));


    }

    public void ReturnToOriginalPosition(Transform bookTransform, bool unlockBooks = false)
    {
        bookTransform.DOKill();

        bookTransform.DOMove(bookTransform.GetComponent<Book>().startPos, 1f).SetEase(Ease.OutQuad);
        bookTransform.DORotateQuaternion(bookTransform.GetComponent<Book>().startRot, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            if (unlockBooks)
            {
                GameManager.instance.UnlockBooks();
            }
        });


    }

    private void PlayParticleEffect(Vector3 position)
    {

        if (particleEffect != null)
        {
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
