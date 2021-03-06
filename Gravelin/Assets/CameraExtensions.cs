﻿using UnityEngine;

namespace Assets
{
	public static class CameraExtensions
	{
		public static Line SphereCastReticleTarget(this Camera camera, Vector3 origin, float sphereRadius, LayerMask mask)
		{
			var middleScreenPoint = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight)*0.5f);
			RaycastHit hit;
			if (Physics.SphereCast(middleScreenPoint, sphereRadius, camera.transform.forward, out hit, 999, ~mask))
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
