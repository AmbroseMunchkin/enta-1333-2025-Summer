using UnityEngine;

namespace RTS_1333
{
	/// <summary>
	///     Represents a specific unit instance in the game, derived from UnitBase.
	///     Handles unit initialization, movement, attacks, and damage.
	/// </summary>
	public class UnitInstance : UnitBase
	{
		/// <summary>
		///     Represents the Animator parameter hash for the "Alive" state.
		/// </summary>
		/// <remarks>
		///     Used to check if the game unit is in an alive state.
		/// </remarks>
		/// <value>
		///     The precomputed hash of "Alive", used for Animator interactions.
		/// </value>
		/// <see cref="Animator.StringToHash(string)" />
		private static readonly int Alive = Animator.StringToHash("Alive");

		/// <summary>
		///     Represents a hash identifier for the "Speed" parameter in the animator.
		/// </summary>
		private static readonly int Speed = Animator.StringToHash("Speed");

		/// <summary>
		///     Represents the hash value for the "Attack" animation trigger in the Animator.
		/// </summary>
		/// <remarks>
		///     This static field is used to optimize the performance when referencing animation triggers in Unity.
		///     Animator.StringToHash generates an integer hash for the "Attack" parameter of the Animator,
		///     reducing the overhead of string comparisons during runtime.
		/// </remarks>
		private static readonly int Attack = Animator.StringToHash("Attack");

		/// <summary>
		///     Static integer hash used for referencing the "Hurt" animation trigger in the Animator.
		/// </summary>
		private static readonly int Hurt = Animator.StringToHash("Hurt");

		private static readonly Color[] ArmyColors =
		{
			Color.cyan, Color.red, Color.yellow, Color.green, Color.magenta, Color.blue, Color.white, Color.black
		};

		/// <summary>
		///     ArmyData is a serialized private field, likely representing the applicable
		///     ArmySettings for a particular UnitInstance in the game.
		/// </summary>
		/// <remarks>
		///     ArmySettings is a ScriptableObject that provides configuration settings
		///     for the army, such as unit appearances or other properties.
		/// </remarks>
		[Header("Game Data")]
		[SerializeField] private ArmySettings ArmyData;

		/// <summary>
		///     Private field responsible for managing character animation states.
		/// </summary>
		/// <remarks>
		///     This field holds a reference to the Animator component, which controls animations for the unit.
		///     It is used to trigger and update animations based on unit behavior (e.g., movement, attack).
		/// </remarks>
		[Header("Prefab Stuff")]
		[SerializeField] private Animator _characterAnimator;

		/// <summary>
		///     Stores the GameObject containing all skinned mesh renderers for the unit.
		/// </summary>
		/// <remarks>
		///     This variable serves as a container for holding one or more skinned mesh renderers
		///     that represent the visible appearance of the unit, including its material and animations.
		/// </remarks>
		[SerializeField] private GameObject _unitSkins;

		/// <summary>
		///     A serialized private field representing the particle system
		///     triggered when the unit takes damage.
		/// </summary>
		/// <remarks>
		///     - This field links to a `ParticleSystem` component in the Unity editor.
		///     - It is used in the `TakeDamage` method to visually represent damage
		///     taken by a unit instance.
		///     - The particle system is optional. If it is not assigned, the behavior
		///     will safely ignore triggering particles.
		/// </remarks>
		[SerializeField] private ParticleSystem _hurtParticles;

		private UnitData _unitData;

		private void Update()
		{
			Tick();
		}

		private void OnDrawGizmos()
		{
			if (CurrentPath == null || CurrentPath.Count < 2)
				return;

			// Set gizmo color for this army.
			Gizmos.color = _unitData.ArmyId < ArmyColors.Length ? ArmyColors[_unitData.ArmyId] : Color.black;
			// Draw lines between each node in the path.
			for (var i = 0; i < CurrentPath.Count - 1; i++)
				Gizmos.DrawLine(CurrentPath[i].WorldPosition, CurrentPath[i + 1].WorldPosition);
		}

		/// <summary>
		///     Initializes the unit with necessary data such as pathfinder, initial health, and army-specific settings.
		/// </summary>
		/// <param name="pathfinder">The pathfinder instance used for navigation.</param>
		/// <param name="armyId">The ID representing the army this unit belongs to.</param>
		public void Initialize(Pathfinder pathfinder, int armyId)
		{
			Pathfinder = pathfinder;
			CurrentHp = _unitType.MaxHp;

			// Skin logic.
			if (ArmyData.ArmySettingsLookup.TryGetValue(armyId, out var setting))
			{
				var skin = setting.UnitSkin;
				foreach (var r in _unitSkins.GetComponentsInChildren<SkinnedMeshRenderer>())
					r.material = skin;
			}

			UpdateAnimatorParameters();
		}

		public void Initialize(Pathfinder pathfinder, UnitData data)
		{
			Pathfinder = pathfinder;
			_unitData = data;
			CurrentHp = data.Health > 0 ? data.Health : data.UnitType.MaxHp;
			transform.position = data.Position;

			// Skin logic.
			if (ArmyData.ArmySettingsLookup.TryGetValue(data.ArmyId, out var setting))
			{
				var skin = setting.UnitSkin;
				foreach (var r in _unitSkins.GetComponentsInChildren<SkinnedMeshRenderer>())
					r.material = skin;
			}

			UpdateAnimatorParameters();
		}

		/// <summary>
		///     Sets a new movement target for the unit using a specific grid node.
		/// </summary>
		/// <param name="node">The grid node representing the target position.</param>
		public void SetTarget(GridNode node)
		{
			SetTarget(node.WorldPosition);
		}

		/// <summary>
		///     Moves the unit to the specified grid node by setting it as the movement target.
		/// </summary>
		/// <param name="targetNode">The grid node to which the unit will move.</param>
		public override void MoveTo(GridNode targetNode)
		{
			SetTarget(targetNode);
		}

		/// <summary>
		///     Triggers the attack animation for the unit.
		/// </summary>
		public void TriggerAttack()
		{
			_characterAnimator.SetTrigger(Attack);
		}

		/// <summary>
		///     Reduces the unit's current health based on the damage taken and handles related effects.
		/// </summary>
		/// <param name="damage">The amount of health to decrease from the current health pool.</param>
		/// Decreases CurrentHp by the specified damage amount.
		/// If CurrentHp becomes 0 or below, sets CurrentHp to 0 and updates animation to 'dead' state.
		/// Triggers the Hurt animation in the character's Animator.
		/// Plays the hurt particle effect if it is assigned.
		public void TakeDamage(int damage)
		{
			CurrentHp -= damage;
			if (CurrentHp <= 0)
			{
				CurrentHp = 0;
				_characterAnimator.SetBool(Alive, false);
			}
			else
			{
				_characterAnimator.SetTrigger(Hurt);
			}

			_hurtParticles?.Play();
		}

		/// <summary>
		///     Updates the animation parameters for the unit's animator.
		///     Synchronizes animation states based on current speed and health status.
		/// </summary>
		private void UpdateAnimatorParameters()
		{
			_characterAnimator.SetFloat(Speed, IsMoving ? _unitType.MoveSpeed : 0f);
			_characterAnimator.SetBool(Alive, CurrentHp > 0);
		}

		/// <summary>
		///     Executes movement logic for the unit in the current tick.
		/// </summary>
		/// <remarks>
		///     Updates the unit's position and state during its movement phase.
		///     Calls the base class movement logic and updates animation parameters.
		/// </remarks>
		public override void DoMove()
		{
			// Call base movement logic
			base.DoMove();

			// Update animation parameters based on movement state
			UpdateAnimatorParameters();
		}
	}
}
