using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
	public float DelayTime = 1;
	public void Start()
	{
		Invoke("DoDestroy", DelayTime);
	}

	public void DoDestroy()
	{
		Destroy(gameObject);
	}
}
