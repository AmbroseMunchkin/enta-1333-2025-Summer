using UnityEngine;

namespace RTS_1333
{
	public class DismantleBuildingUI : MonoBehaviour
	{
		[SerializeField] private GameObject Parent;

		private BuildingBase _placedBuilding;

		public void Open(BuildingBase buildingToDestroy)
		{
			Parent.SetActive(true);
			_placedBuilding = buildingToDestroy;
		}

		private void Close()
		{
			_placedBuilding = null;
			Parent.SetActive(false);
		}

		public void ButtonCancel()
		{
			Close();
		}

		public void ButtonConfirm()
		{
			// todo replace with returning to pool
			//_placedBuilding.OnRemoved();
			Destroy(_placedBuilding?.gameObject);

			Close();
		}
	}
}
