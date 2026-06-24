using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private float _openPosition;
    [SerializeField] private float _closePosition;
    [SerializeField] private float _transitionDuration = 1f;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private SettingsUI _settingsUI;
    [SerializeField] private CameraController _cameraController;
    private RectTransform _recTransform;

    private void Awake()
    {
        _recTransform = GetComponent<RectTransform>();
        _closePosition = -_recTransform.rect.width - 300;
        _openPosition = 0;
    }
    private void Start()
    {
        _recTransform.DOAnchorPosY(_openPosition, _transitionDuration).SetEase(Ease.OutBounce).OnComplete(() => {
            //once panel completes its slide transtition, make its button clickable and animate the button in
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            _playButton.GetComponent<RectTransform>().DOAnchorPosX(0.5f, 0.5f).SetEase(Ease.InQuad);
            _settingsButton.GetComponent<RectTransform>().DOAnchorPosX(0.5f, 0.5f).SetEase(Ease.InQuad);
            _quitButton.GetComponent<RectTransform>().DOAnchorPosX(0.5f, 0.5f).SetEase(Ease.InQuad);
        });
    }
    public void ReturnToMenu()
    {

        _recTransform.DOAnchorPosY(_openPosition, _transitionDuration).SetEase(Ease.InOutBack).OnComplete(() => {
            //once panel completes its slide transtition, make its button clickable and animate the button in
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            _playButton.GetComponent<RectTransform>().DOAnchorPosX(0.5f, 0.5f).SetEase(Ease.InQuad);
            _settingsButton.GetComponent<RectTransform>().DOAnchorPosX(0.5f, 0.5f).SetEase(Ease.InQuad);
            _quitButton.GetComponent<RectTransform>().DOAnchorPosX(0.5f, 0.5f).SetEase(Ease.InQuad);
        });
    }
    public void Play()
    {
        //make any buttons in the panel un-clickable
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        //slide the panel out, reset the button to its default position
        _recTransform.DOAnchorPosY(_closePosition, _transitionDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            _playButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(1070, -65);
            _settingsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1070, 0);
            _quitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(1070, 65);
        });
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        _cameraController.isPaused = false;
    }
    public void Settings()
    {
        _settingsUI.ShowSettings();
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
