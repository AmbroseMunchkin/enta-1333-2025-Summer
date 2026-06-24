using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SlidePanelScript : MonoBehaviour
{
    [SerializeField] private float _openPosition;
    [SerializeField] private float _closePosition;
    [SerializeField] private float _transitionDuration = 1f;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private CameraController _cameraController;
    private RectTransform _recTransform;

    private void Awake()
    {
        _recTransform = GetComponent<RectTransform>();
        _closePosition = -_recTransform.rect.width;
        _openPosition = 0;
    }
    public void Pause()
    {
        _cameraController.isPaused = true;
        //slide the panel in using a tween
        _recTransform.DOAnchorPosX(_openPosition, _transitionDuration).SetEase(Ease.OutBounce).OnComplete(() => { 
            //once panel completes its slide transtition, make its button clickable and animate the button in
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            _continueButton.GetComponent<RectTransform>().DOAnchorPosX(0.5f, 0.5f).SetEase(Ease.InQuad);
            _mainMenuButton.GetComponent<RectTransform>().DOAnchorPosX(0.5f, 0.5f).SetEase(Ease.InQuad);
            _exitButton.GetComponent<RectTransform>().DOAnchorPosX(0.5f, 0.5f).SetEase(Ease.InQuad);
        });
    }

    // Update is called once per frame
    public void Continue()
    {
        //make any buttons in the panel un-clickable
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        //slide the panel out, reset the button to its default position
        _recTransform.DOAnchorPosX(_closePosition, _transitionDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            _continueButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 150);
            _mainMenuButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, -50);
            _exitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, -200);
        });
        _cameraController.isPaused = false;
    }
    public void MainMenu()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        //slide the panel out, reset the button to its default position
        _recTransform.DOAnchorPosX(_closePosition, _transitionDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            _continueButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 150);
            _mainMenuButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, -50);
            _exitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, -200);
        }).OnComplete(() =>
        { _mainMenu.ReturnToMenu(); });
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // This closes the actual built game application executable
        Application.Quit();
#endif
    }
}
