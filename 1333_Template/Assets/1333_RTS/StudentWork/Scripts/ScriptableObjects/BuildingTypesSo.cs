using System.Collections.Generic;
using RTS_1333;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingTypesSo", menuName = "ScriptableObjects/BuildingTypes")]
public class BuildingTypesSo : ScriptableObject
{
    public List<BuildingData> Buildings = new ();
}
