using UnityEngine;

public static class RectTransformExtensions
{
    public static void AnchorToCorners(this RectTransform transform)
    {
        if (transform == null)
            throw new System.ArgumentNullException("transform");

        if (transform.parent == null)
            return;

        var parent = transform.parent.GetComponent<RectTransform>();

        Vector2 newAnchorsMin = new Vector2(transform.anchorMin.x + transform.offsetMin.x / parent.rect.width,
                          transform.anchorMin.y + transform.offsetMin.y / parent.rect.height);

        Vector2 newAnchorsMax = new Vector2(transform.anchorMax.x + transform.offsetMax.x / parent.rect.width,
                          transform.anchorMax.y + transform.offsetMax.y / parent.rect.height);

        transform.anchorMin = newAnchorsMin;
        transform.anchorMax = newAnchorsMax;
        transform.offsetMin = transform.offsetMax = new Vector2(0, 0);
    }

    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
    {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    public static Vector2 GetSize(this RectTransform trans)
    {
        return trans.rect.size;
    }

    public static float GetWidth(this RectTransform trans)
    {
        return trans.rect.width;
    }

    public static float GetHeight(this RectTransform trans)
    {
        return trans.rect.height;
    }



    public static void SetBottomLeftPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetTopLeftPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetBottomRightPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetDefaultScale(this RectTransform trans)
    {
        trans.localScale = new Vector3(1, 1, 1);
    }


    public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
    }

    public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }


    public static void SetSize(this RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }
    public static void SetWidth(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }
    public static void SetHeight(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }



    public static Rect ScreenRectToRectTransform(this RectTransform rt, Rect r, Camera cam = null)
    {
        Vector2 localCenter;
        Vector2 localTopRight;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, new Vector2(r.x, r.y), cam, out localCenter);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, new Vector2(r.x + r.width * .5f, r.y + r.height * .5f), cam, out localTopRight);

        return new Rect(localCenter.x, localCenter.y, Mathf.Abs(localTopRight.x - localCenter.x) * 2, Mathf.Abs(localTopRight.y - localCenter.y) * 2);
    }


    public static Camera GetObjectRenderCamera(this RectTransform obj)
    {
        Canvas objCanvas = obj.GetComponentInParent<Canvas>();
        Camera cam = null;

        switch (objCanvas.renderMode)
        {
            case RenderMode.ScreenSpaceCamera:
            case RenderMode.WorldSpace:
                cam = objCanvas.worldCamera;
                break;
            case RenderMode.ScreenSpaceOverlay:
                cam = null;
                break;
        }
        return cam;
    }

    public static Rect GetRectFromOtherParent(this RectTransform obj, RectTransform other)
    {
        Vector3[] objCorners = new Vector3[4];
        obj.GetWorldCorners(objCorners);
        Vector3 topLeftCorner = objCorners[0];
        Vector3 bottomRightCorner = objCorners[2];

        Camera objToScreenCam = obj.GetObjectRenderCamera();
        Camera screenToOtherObjCam = other.GetObjectRenderCamera();

        Vector2 topLeftScreenPos = RectTransformUtility.WorldToScreenPoint(objToScreenCam, topLeftCorner);
        Vector2 bottomRightScreenPos = RectTransformUtility.WorldToScreenPoint(objToScreenCam, bottomRightCorner);

        Vector2 localPointTopLeft, localPointBottomRight;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(other, topLeftScreenPos, screenToOtherObjCam, out localPointTopLeft);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(other, bottomRightScreenPos, screenToOtherObjCam, out localPointBottomRight);

        float width = Mathf.Abs(localPointBottomRight.x - localPointTopLeft.x);
        float height = Mathf.Abs(localPointTopLeft.y - localPointBottomRight.y);
        Vector2 localCenter = localPointTopLeft + new Vector2(width * .5f, height * .5f);

        Rect r = new Rect(localPointTopLeft.x, localPointTopLeft.y, width, height);
        Rect rCenter = new Rect(r.center.x, r.center.y, width, height);

        return rCenter;
    }

}
