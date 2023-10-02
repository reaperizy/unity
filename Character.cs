using Fusion;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Visual representation of a Player - the Character is instantiated by the map once it's loaded.
/// This class handles camera tracking and player movement and is destroyed when the map is unloaded.
/// (I.e. the player gets a new avatar in each map)
/// </summary>

public class Character : NetworkBehaviour
{
	[SerializeField] private Text _name;
	[SerializeField] private Animator _animator;
	[SerializeField] private MeshRenderer _mesh;
	[SerializeField] private CharacterInteraction _interaction;
    [SerializeField] private PlayerStateManager _playerStateManager;

	[SerializeField] private Transform _anchorCharacter;
	[SerializeField] private GameObject _characterCowo;
    [SerializeField] private GameObject _characterCewe;

	public bool cowok;
	public string Role;

    public float moveVelocity = 5f;

	[UnityHeader("Networked Anim Field")]
	[Networked] public Angle yCamRotation { get; set; }
	[Networked] public int xMovement { get; set; }
	[Networked] public int yMovement { get; set; }

	[Networked] public Player Player { get; set; }

	[Networked]
	private bool _isReadInput { get; set; }

	public Image speakingIndicator;

	[Networked(OnChanged = nameof(UpdateSpeakingIndicator))] public NetworkBool isSpeaking { get; set; }


    public override void Spawned()
	{
		_isReadInput = false;

		cowok = false;
		if (HasInputAuthority)
		{
			Role = "Dosen";
            // Jika boolean "cowok" adalah true
            
        }

		SpawnCharacterModel();
    }

	private void SpawnCharacterModel()
    {
		if (cowok)
		{
			// Membuat karakter laki-laki sebagai anak dari _anchorCharacter
			var model = Instantiate(_characterCowo, _anchorCharacter.position, _anchorCharacter.rotation);
			model.transform.SetParent(_anchorCharacter);

			// Mengambil animator dari karakter laki-laki dan mengatur ke _animator
			_animator = model.GetComponent<Animator>();

		}
		else
		{
			// Jika boolean "cowok" adalah false, maka kita mengasumsikan karakter perempuan
			// Membuat karakter perempuan sebagai anak dari _anchorCharacter
			var model = Instantiate(_characterCewe, _anchorCharacter.position, _anchorCharacter.rotation);
			model.transform.SetParent(_anchorCharacter);

			// Mengambil animator dari karakter perempuan dan mengatur ke _animator
			_animator = model.GetComponent<Animator>();
		}
	}

	public void SetPlayer(Player player)
    {
		Player = player;
		_interaction.Player = player;
    }

	protected static void UpdateSpeakingIndicator(Changed<Character> changed)
	{
		bool _isSpeaking = changed.Behavior.isSpeaking;
		Image _speakingIndicator = changed.Behavior.speakingIndicator;

		if (_isSpeaking)
		{
			_speakingIndicator.enabled = true;
		}
		else
		{
			_speakingIndicator.enabled = false;
		}
	}

    public void LateUpdate()
	{
		// This is a little brute-force, but it gets the job done.
		// Could use an OnChanged listener on the properties instead.
		_name.text = Player.Name.Value;
		_mesh.material.color = Player.Color;
	}

	public override void FixedUpdateNetwork()
	{
		if (Player == null) return;
		if (_playerStateManager.CurrentGameState != GameState.Play) return;

		if (Player.InputEnabled && GetInput(out InputData data))
		{
			_isReadInput = true;

			if (data.GetButton(ButtonFlag.LEFT))
            {
				transform.position -= Runner.DeltaTime * moveVelocity * transform.right;
				xMovement = -1;
			} 
			else if (data.GetButton(ButtonFlag.RIGHT))
            {
				transform.position += Runner.DeltaTime * moveVelocity * transform.right;
				xMovement = 1;
			} 
			else if (data.GetButton(ButtonFlag.FORWARD))
            {
				transform.position += Runner.DeltaTime * moveVelocity * transform.forward;
				yMovement = 1;
			} 
			else if (data.GetButton(ButtonFlag.BACKWARD))
            {
				transform.position -= Runner.DeltaTime * moveVelocity * transform.forward;
				yMovement = -1;
			}
			else // No input
			{
				_isReadInput = false;
				xMovement = 0;
				yMovement = 0;
			}

			yCamRotation += data.YCamRotation;
		}

        transform.rotation = Quaternion.Euler(0, (float)yCamRotation, 0);
	}

    public override void Render()
    {
		if(_isReadInput)
        {
			_animator.SetFloat("xMovement", xMovement);
			_animator.SetFloat("yMovement", yMovement);
        }
        else
		{
			_animator.SetFloat("xMovement", 0);
            _animator.SetFloat("yMovement", 0);
        }
    }
}