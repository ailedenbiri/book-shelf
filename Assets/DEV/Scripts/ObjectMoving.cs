using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoving : MonoBehaviour
{
    private bool objeSecili = false;
    private Vector3 fareSonKonum;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    objeSecili = true;
                    fareSonKonum = hit.point - transform.position;
                }
            }
        }

        if (Input.GetMouseButton(0) && objeSecili)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 fareKonum = ray.GetPoint(10) - fareSonKonum;

            // Z eksenindeki deðeri sabit býrakarak sadece x ve y eksenlerinde hareket etmesini saðla
            fareKonum.z = transform.position.z;

            transform.position = fareKonum;
        }

        if (Input.GetMouseButtonUp(0))
        {
            objeSecili = false;
        }
    }
}
