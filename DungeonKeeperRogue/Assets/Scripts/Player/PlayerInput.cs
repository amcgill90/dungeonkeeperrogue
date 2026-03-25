using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	[SerializeField] private float _handShowThreshold = 0.1f;
	[SerializeField] private float _handHideThreshold = 0.5f;

	private Player _player;
	private RaycastHit2D[] _raycastHits = new RaycastHit2D[2];


	public void Init(Player player)
	{
		_player = player;
	}

	private void Update()
	{
		if (_player == null)
		{
			return;
		}

		var mouse = UnityEngine.InputSystem.Mouse.current;

		// handle hand showing
		float normalizedScreenY = mouse.position.value.y / Screen.height;
		if (normalizedScreenY <= _handShowThreshold)
		{
			_player.Hand.Show(true);
		}
		else if (normalizedScreenY > _handHideThreshold)
		{
			_player.Hand.Show(false);
		}

		// Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouse.position.value.x, mouse.position.value.y, -Camera.main.transform.position.z));

		// if (Physics2D.Raycast(worldPos, Vector2.zero, ContactFilter2D.noFilter, _raycastHits) > 0)
		// {
		// 	RaycastHit2D hit = _raycastHits[0];
		// 	Hand hand = hit.collider.GetComponentInParent<Hand>();
		// 	if (hand != null)
		// 	{
		// 		// make sure hand is in "visible" state
		// 		hand.Show(true);
		// 	}
		// }
	}
}
