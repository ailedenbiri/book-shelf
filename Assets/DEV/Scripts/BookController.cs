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

                if (Physics.Raycast(ray, out hit))
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

                        selectedBook = bookTransform;
                        GameManager.instance.state = GameManager.GameState.Waiting;
                        PlayBookAnimation(selectedBook.transform);
                    }

                    else if (hit.transform.TryGetComponent<DistanceCalculator>(out DistanceCalculator shelf) && selectedBook != null)
                    {
                        GameManager.instance.state = GameManager.GameState.Waiting;
                        Vector3 bookPoint = shelf.AddPositionCalculate(selectedBook);
                        selectedBook.placed = true;
                        selectedBook.GetComponent<Collider>().enabled = false;
                        selectedBook = null;
                    }
                }
            }
        }

    }


    private void PlayBookAnimation(Transform bookTransform)
    {

        bookTransform.DOKill();
        bookTransform.GetComponent<Book>().UpdatePositions();

        bookTransform.DORotate(new Vector3(0f, -90f, 0f), 0.5f);
        bookTransform.DOMoveX(0.10f, 0.5f);
        bookTransform.DOMoveY(0.5f, 0.5f);
        bookTransform.DOMoveZ(0f, 0.5f).OnComplete(() =>
        {
            GameManager.instance.state = GameManager.GameState.Playing;
        });
    }

    public void PlaceBookOnShelf(Transform bookTransform, Vector3 targetPosition)
    {


        bookTransform.DOKill();
        //bookTransform.DOMove(targetPosition, 1.75f).SetEase(Ease.OutQuart);
        //bookTransform.DORotate(new Vector3(0f, -180f, 0f), 1.75f).SetEase(Ease.InOutBack).OnComplete(() => PlayParticleEffect(bookTransform.position));

        Sequence bookSeq = DOTween.Sequence();

        bookSeq.Append(bookTransform.DOMove(targetPosition - Vector3.forward * 1f, 1.45f));
        bookSeq.Join(bookTransform.DORotate(new Vector3(0f, -180f, 0f), 1.45f));
        bookSeq.Append(bookTransform.DOMove(targetPosition, 0.3f));
        bookSeq.AppendCallback(() => PlayParticleEffect(targetPosition + Vector3.up * 0.5f - Vector3.forward * 0.5f));

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
