using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnitManager _unitMgr;
    [SerializeField] private GridManager _gridMgr;

    private void Awake()
    {
        _gridMgr.InitializeGrid();
    }
}
