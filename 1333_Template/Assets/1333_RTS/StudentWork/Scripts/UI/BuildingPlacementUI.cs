using System;
using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;

public class BuildingPlacementUI : MonoBehaviour
{
    [SerializeField] private RectTransform LayoutGroupParent;
    [SerializeField] private SelectBuildingButton ButtonPrefab;
    [SerializeField] private BuildingTypesSo BuildingData;

    private void Start()
    {
        foreach (BuildingData t in BuildingData.Buildings)
        {
            SelectBuildingButton button = Instantiate(ButtonPrefab, LayoutGroupParent);
            button.Setup(t);
        }
    }
}
