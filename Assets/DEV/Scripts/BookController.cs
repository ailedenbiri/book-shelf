using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BookController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        // Mouse sol týklandýðýnda
        if (Input.GetMouseButtonDown(0))
        {
          
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

           
            if (Physics.Raycast(ray, out hit))
            {
                
                hit.transform.DORotate(new Vector3(-90f, 0f, 0f), 1f);

                hit.transform.DOMoveY(0.44f, 1f);
            }
        }
    }
}
