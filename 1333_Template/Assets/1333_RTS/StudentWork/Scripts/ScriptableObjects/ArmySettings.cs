using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     The ArmySettings class serves as a ScriptableObject for managing configurations
///     related to armies in a game, such as their skins and identifiers.
/// </summary>
[CreateAssetMenu(fileName = "ArmySettings", menuName = "Game/ArmySettings")]
public class ArmySettings : ScriptableObject
{
	/// <summary>
	///     Stores configurations for army settings.
	/// </summary>
	/// <remarks>
	///     Used to define settings for armies, such as skins and IDs,
	///     and is populated with a list of ArmySetting objects.
	/// </remarks>
	public List<ArmySetting> Settings = new();

	/// <summary>
	///     Dictionary used as a lookup table for quick access to ArmySetting objects by their ArmyId.
	/// </summary>
	private Dictionary<int, ArmySetting> _armySettingsLookup = new();

	/// <summary>
	///     Provides a read-only lookup table for ArmySetting objects, optimized for quick access by ArmyId.
	/// </summary>
	public IReadOnlyDictionary<int, ArmySetting> ArmySettingsLookup => _armySettingsLookup;

	/// <summary>
	///     Called when the ScriptableObject is enabled or loaded in memory.
	///     Initializes the lookup dictionary to optimize access to army settings.
	/// </summary>
	private void OnEnable()
	{
		if (_armySettingsLookup != null && _armySettingsLookup.Count > 0)
			return;

		_armySettingsLookup = new Dictionary<int, ArmySetting>(Settings.Count);
		foreach (var skill in Settings)
			if (!_armySettingsLookup.TryAdd(skill.ArmyId, skill))
				Debug.LogWarning($"Duplicate army ID {skill.ArmyId} found in {name}.");
	}
}

/// <summary>
///     Represents a setting for an army, containing properties for its ID and associated skins.
/// </summary>
[Serializable]
public class ArmySetting
{
	/// <summary>
	///     Unique identifier for an army.
	/// </summary>
	public int ArmyId;

	/// <summary>
	///     Represents the visual appearance of an army unit using a specified material.
	/// </summary>
	public Material UnitSkin;

	/// <summary>
	///     Material applied to represent the appearance of a building in the game environment.
	/// </summary>
	public Material BuildingSkin;
}
