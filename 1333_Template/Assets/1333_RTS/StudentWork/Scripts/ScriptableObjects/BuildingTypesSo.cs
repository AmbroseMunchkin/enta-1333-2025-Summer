using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingTypesSo", menuName = "ScriptableObjects/BuildingTypes")]
public class BuildingTypesSo : ScriptableObject
{
    public List<BuildingData> Buildings = new ();
}

[System.Serializable]
public class BuildingData
{
    public string BuildingName;
    public Sprite BuildingIcon;
}
