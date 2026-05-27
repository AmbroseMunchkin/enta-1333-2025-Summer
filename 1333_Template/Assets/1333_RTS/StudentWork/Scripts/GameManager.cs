using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnitManager _unitMgr;
    [SerializeField] private GridManager _gridMgr;

    private void Awake()
    {
        //gridMgr.InitializeGrid(); <---- TO DO (Make this)
    }
}
