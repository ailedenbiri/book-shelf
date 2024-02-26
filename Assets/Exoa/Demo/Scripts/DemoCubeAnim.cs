using UnityEngine;

public class DemoCubeAnim : MonoBehaviour
{

    public bool rotate;
    public Vector3 rotationVector;

    public bool move;
    public Vector3 startPos;
    public Vector3 endPos;
    public float duration = 1;



    void Update()
    {
        float t2 = Mathf.PingPong(Time.time * .4f, 1);
        if (move) transform.position = Vector3.Lerp(startPos, endPos, t2);
        if (rotate) transform.Rotate(rotationVector * Time.deltaTime);
    }
}
