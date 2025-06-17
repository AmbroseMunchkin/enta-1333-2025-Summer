using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RTS_1333
{
	public class SelectBuildingButton : MonoBehaviour
	{
		[SerializeField] private Image BuildingSprite;
		[SerializeField] private TMP_Text BuildingText;

		private BuildingData _data;
		private BuildingPlacementManager _manager;

		public void Setup(BuildingData data, BuildingPlacementManager manager)
		{
			if (data == null)
			{
				Debug.LogError("data is null");
				Destroy(gameObject);
				return;
			}

			_data = data;
			_manager = manager;
			// setup ui of the button
			BuildingSprite.sprite = data.BuildingSprite;
			BuildingText.text = data.BuildingName;
		}

		public void OnButtonSelected()
		{
			Debug.Log($"Selected {_data.BuildingName} to place");
			_manager.OnNewBuildingSelected(_data);
		}
	}
}
