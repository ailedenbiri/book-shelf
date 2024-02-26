using UnityEngine;

namespace Exoa.Utils
{
    public class GameObjectUtils
    {

        /*
      x     - value             (input/output)
      v     - velocity          (input/output)
      xt    - target value      (input)
      zeta  - damping ratio     (input)
      omega - angular frequency (input)
      h     - time step         (input)
    */
        public float Spring
        (
          ref float x, ref float v, float xt,
          float zeta, float omega, float h
        )
        {
            float f = 1.0f + 2.0f * h * zeta * omega;
            float oo = omega * omega;
            float hoo = h * oo;
            float hhoo = h * hoo;
            float detInv = 1.0f / (f + hhoo);
            float detX = f * x + h * v + hhoo * xt;
            float detV = v + hoo * (xt - x);
            x = detX * detInv;
            v = detV * detInv;
            return x;
        }

        /**
        * Get Object's bounds in the 3D space
        **/
        public static Bounds GetObject3DRect(GameObject obj)
        {
            RectTransform objRect = obj.GetComponent<RectTransform>();
            Renderer objRenderer = obj.GetComponent<Renderer>();
            Collider objCollider = obj.GetComponentInChildren<Collider>();

            Bounds newRect = new Bounds();
            if (objRenderer != null)
            {
                newRect = objRenderer.bounds;
            }
            else if (objCollider != null)
            {
                newRect = objCollider.bounds;
            }
            return newRect;
        }
    }
}
