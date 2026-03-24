using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
	[SerializeField] private Player _playerPrefab;

	private Player _player;

	public Player Player => _player;


	protected override void OnInitialized()
	{
		base.OnInitialized();

		_player = Instantiate(_playerPrefab, transform);
		_player.Init();
	}
}
