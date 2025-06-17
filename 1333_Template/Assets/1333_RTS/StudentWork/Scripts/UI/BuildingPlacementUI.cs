using UnityEngine;

namespace RTS_1333
{
	public class BuildingPlacementUI : MonoBehaviour
	{
		[SerializeField] private BuildingPlacementManager BuildingPlacementManager;
		[SerializeField] private SelectBuildingButton SelectBuildingButton;
		[SerializeField] private Transform ScrollRectContent;

		private void Start()
		{
			foreach (var building in BuildingPlacementManager.AllBuildings.Buildings)
			{
				var button = Instantiate(
					SelectBuildingButton, ScrollRectContent);
				button.Setup(building, BuildingPlacementManager);
			}
		}
	}
}
