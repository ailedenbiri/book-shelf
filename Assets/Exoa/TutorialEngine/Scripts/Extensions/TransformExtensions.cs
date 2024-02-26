using UnityEngine;

public static class TransformExtensions
{

	public static void DestroyChildrenByName(this Transform current, string name, bool immediate)
	{
		while (current.Find("remove") != null)
		{
			if (immediate)
				GameObject.DestroyImmediate(current.Find("remove").gameObject);
			else
				GameObject.Destroy(current.Find("remove").gameObject);
		}

	}
	public static GameObject FindClosest(this Transform current, GameObject[] gos, string containsName = null)
	{
		float closestDistance = float.MaxValue;
		float distance;
		GameObject closestGO = null;
		foreach (GameObject g in gos)
		{
			distance = Vector3.Distance(current.position, g.transform.position);
			if (distance < closestDistance && (containsName == null || g.name.Contains(containsName)))
			{
				closestDistance = distance;
				closestGO = g;
			}
		}
		return closestGO;
	}
	public static Transform ClearChildren(this Transform current)
	{
		for (int i = current.childCount - 1; i >= 0; i--)
			GameObject.Destroy(current.GetChild(i).gameObject);
		return current;
	}
	public static Transform ClearChildrenImmediate(this Transform current)
	{
		for (int i = current.childCount - 1; i >= 0; i--)
			GameObject.DestroyImmediate(current.GetChild(i).gameObject);
		return current;
	}
	public static T FindChildRecursiveComp<T>(this Transform current, string name)
	{
		Transform child = current.FindChildRecursive(name);
		return child.GetComponent<T>();
	}
	public static GameObject FindChildInactive(this GameObject parent, string name)
	{
		Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
		foreach (Transform t in trs)
		{
			if (t.name == name)
			{
				return t.gameObject;
			}
		}
		return null;
	}
	public static Transform FindChildRecursive(this Transform current, string name)
	{
		// check if the current bone is the bone we're looking for, if so return it
		if (current.name == name)
			return current;
		// search through child bones for the bone we're looking for
		Transform[] trs = current.GetComponentsInChildren<Transform>(true);
		foreach (Transform t in trs)
		{

			if (t == current) continue;
			// the recursive step; repeat the search one step deeper in the hierarchy
			Transform found = t.FindChildRecursive(name);
			// a transform was returned by the search above and is not null,
			// it must be the one we're looking for
			if (found != null)
				return found;
		}

		// bone with name was not found
		return null;
	}
}
