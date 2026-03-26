using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	[SerializeField] private float _handShowThreshold = 0.1f;
	[SerializeField] private float _handHideThreshold = 0.5f;
	[SerializeField] private LayerMask _inputLayerMask;

	private Player _player;
	private Card _lastHighlightedCard;
	private MapNode _lastHighlightedNode;
	private RaycastHit2D[] _raycastHits = new RaycastHit2D[2];
	private ContactFilter2D _uiContactFilter;
	private Card _cardBeingPlayed = null;


	public void Init(Player player)
	{
		_player = player;

		_uiContactFilter = new ContactFilter2D()
		{
			layerMask = _inputLayerMask,
			useTriggers = true
		};
	}

	public MapNode DoMapNodeSelection(NodeSelectionFilterOptions filter)
	{
		var mouse = UnityEngine.InputSystem.Mouse.current;
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouse.position.value.x, mouse.position.value.y, -Camera.main.transform.position.z));
		MapNode nodeUnderMouse = null;

		if (Physics2D.Raycast(worldPos, Vector2.zero, _uiContactFilter, _raycastHits) > 0)
		{
			RaycastHit2D hit = _raycastHits[0];
			nodeUnderMouse = hit.collider.GetComponentInParent<MapNode>();

			if (nodeUnderMouse != null && (filter == null || filter.IsValidOption(nodeUnderMouse) == false))
			{
				nodeUnderMouse = null;
			}
		}

		_lastHighlightedNode = nodeUnderMouse;

		// set which nodes are options given current filter
		Map.Instance.ShowNodeHighlights(filter, nodeUnderMouse);

		return mouse.leftButton.wasPressedThisFrame ? nodeUnderMouse : null;
	}

	public IEnumerator ProcessInput()
	{
		if (_player == null)
		{
			yield return null;
		}

		var mouse = UnityEngine.InputSystem.Mouse.current;
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouse.position.value.x, mouse.position.value.y, -Camera.main.transform.position.z));
		
		MapNode nodeUnderMouse = null;

		if (Physics2D.Raycast(worldPos, Vector2.zero, _uiContactFilter, _raycastHits) > 0)
		{
			RaycastHit2D hit = _raycastHits[0];
			nodeUnderMouse = hit.collider.GetComponentInParent<MapNode>();

			if (nodeUnderMouse&& nodeUnderMouse.Card)
			{
				var card = nodeUnderMouse.Card;
				if (card)
				{
					HUDTileInfo.Instance.ShowTileInfo(card.Name, card.Description, card.Icon);
				}
			}
			else
			{
				HUDTileInfo.Instance.Hide();
			}
		}

		_lastHighlightedNode = nodeUnderMouse;

		if (_cardBeingPlayed == null)
		{
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

			// find card under mouse and highlight
			Card cardUnderMouse = null;

			if (Physics2D.Raycast(worldPos, Vector2.zero, _uiContactFilter, _raycastHits) > 0)
			{
				RaycastHit2D hit = _raycastHits[0];
				cardUnderMouse = hit.collider.GetComponentInParent<Card>();
				if (cardUnderMouse != null)
				{
					cardUnderMouse.SetHighlighted(true);
				}
			}

			if (_lastHighlightedCard != null && _lastHighlightedCard != cardUnderMouse)
			{
				_lastHighlightedCard.SetHighlighted(false);
			}

			_lastHighlightedCard = cardUnderMouse;

			// handle click of card
			if (cardUnderMouse != null && mouse.leftButton.wasPressedThisFrame && cardUnderMouse.GetCanPlay())
			{
				yield return PlayCard(cardUnderMouse);

				_player.AddCoins(-cardUnderMouse.Cost);
				_player.Hand.DiscardCard(cardUnderMouse);
			}
		}
		else
		{
			// card is being played - hide hand
			_player.Hand.Show(false);
		}
	}

	private IEnumerator PlayCard(Card card)
	{
		_cardBeingPlayed = card;

		_player.Hand.Show(false);

		yield return card.PlayCard();

		_cardBeingPlayed = null;
	}
}
