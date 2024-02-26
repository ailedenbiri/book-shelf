using UnityEngine;

public static class RendererExtensions
{

	static void get_minMax(Vector2 point, ref Vector2 min, ref Vector2 max)
	{
		min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
		max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);
	}

	public static Rect GetScreenRect(this Renderer renderer, Camera cam = null)
	{
		Vector3 cen = renderer.bounds.center;
		Vector3 ext = renderer.bounds.extents;
		cam = cam == null ? Camera.main : cam;
		float screenheight = Screen.height;

		Vector2 min = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z - ext.z));
		Vector2 max = min;

		Vector2 point = min;
		get_minMax(point, ref min, ref max);

		point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z - ext.z));
		get_minMax(point, ref min, ref max);

		point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z + ext.z));
		get_minMax(point, ref min, ref max);

		point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z + ext.z));
		get_minMax(point, ref min, ref max);

		point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z - ext.z));
		get_minMax(point, ref min, ref max);

		point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z - ext.z));
		get_minMax(point, ref min, ref max);

		point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z + ext.z));
		get_minMax(point, ref min, ref max);

		point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z + ext.z));
		get_minMax(point, ref min, ref max);

		Vector3 centerScreenPos2 = cam.WorldToScreenPoint(renderer.bounds.center);

		return new Rect(centerScreenPos2.x, centerScreenPos2.y, max.x - min.x, max.y - min.y);
	}
}
