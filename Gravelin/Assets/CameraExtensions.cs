using UnityEngine;

namespace Assets
{
	public static class CameraExtensions
	{
		public static Line RayCastReticleTarget(this Camera camera, Vector3 origin)
		{
			var middleScreenPoint = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight)*0.5f);
			RaycastHit hit;
			if(Physics.Raycast(middleScreenPoint, camera.transform.forward, out hit))
			{
				return new Line(middleScreenPoint, hit.point);
			}
			return null;
		}
	}

	public class Line
	{
		public Line(Vector3 origin, Vector3 targetPoint)
		{
			Origin = origin;
			TargetPoint = targetPoint;
			ToTarget = TargetPoint - Origin;
		}

		public Vector3 Origin { get; private set; }
		public Vector3 TargetPoint { get; private set; }

		public Vector3 ToTarget { get; private set; }
	}
}
