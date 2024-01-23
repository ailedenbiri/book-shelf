using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BookController : MonoBehaviour
{
    [SerializeField] private List<Transform> books;
    [SerializeField] private ParticleSystem particleEffect;


    private Transform selectedBook;
    private bool moved = false;

    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();

    private void Start()
    {
        foreach (Transform book in books)
        {
            originalPositions[book] = book.position;
        }

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
                if (books.Contains(hit.transform))
                {
                    selectedBook = hit.transform;

                    if (!moved)
                    {
                        PlayBookAnimation(selectedBook);
                        moved = true;
                    }
                    else
                    {
                        ReturnToOriginalPosition(selectedBook);
                        moved = false;

                       
                    }
                }
                else if (hit.collider.CompareTag("Respawn") && selectedBook != null)
                {
                    PlaceBookOnShelf(selectedBook, hit.point);
                    selectedBook = null;
                    moved = false;
                }
            }
        }
    }

       
    private void PlayBookAnimation(Transform bookTransform)
    {
        bookTransform.DOKill();

        bookTransform.DORotate(new Vector3(-90f, 0f, 0f), 1f);
        bookTransform.DOMoveX(0.10f, 0.5f);
        bookTransform.DOMoveY(0.5f,0.5f);
        bookTransform.DOMoveZ(0f, 0.5f);
    }

    private void PlaceBookOnShelf(Transform bookTransform, Vector3 targetPosition)
    {

        bookTransform.DOKill();
        Transform temp = bookTransform;

        bookTransform.DOMove(targetPosition + new Vector3(0f, 0.44f, 0f), 1f).SetEase(Ease.OutQuad);
        bookTransform.DORotate(new Vector3(-90f, 0f, -90f), 1f).SetEase(Ease.OutQuad).OnComplete(() => PlayParticleEffect(temp.position));


    }

    private void ReturnToOriginalPosition(Transform bookTransform)
    {
        bookTransform.DOKill();

        bookTransform.DOMove(originalPositions[bookTransform], 1f).SetEase(Ease.OutQuad);
        bookTransform.DORotate(Vector3.zero, 1f).SetEase(Ease.OutQuad);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("shelfTrigger"))
        {
            Debug.Log("temasvar1");
        }
        
    }

   
}
