using UnityEngine;

public class Diggable : MonoBehaviour
{
	[SerializeField] private GameObject _spawnPrefabOnDig;

	public delegate void DigDelegate(Diggable diggable);
	public static event DigDelegate OnDig;


	public void AssignPowerup()
	{
		// TODO
	}

	public void Dig()
	{
		if (_spawnPrefabOnDig != null)
		{
			Instantiate(_spawnPrefabOnDig, transform.position, Quaternion.identity);
		}

		OnDig?.Invoke(this);

		Destroy(gameObject);
	}
}
