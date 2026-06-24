using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private float _openPosition;
    [SerializeField] private float _closePosition;
    [SerializeField] private float _transitionDuration = 1f;
    [SerializeField] private Button _returnButton;

    private RectTransform _recTransform;

    private void Awake()
    {
        _recTransform = GetComponent<RectTransform>();
        _closePosition = -_recTransform.rect.width - 300;
        _openPosition = 0;
    }
    public void BackToMenu()
    {
        //make any buttons in the panel un-clickable
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        //slide the panel out, reset the button to its default position
        _recTransform.DOAnchorPosY(_closePosition, _transitionDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            _returnButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -240);
        });
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void ShowSettings()
    {
        _recTransform.DOAnchorPosY(_openPosition, _transitionDuration).SetEase(Ease.InOutBack).OnComplete(() => {
            //once panel completes its slide transtition, make its button clickable and animate the button in
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            _returnButton.GetComponent<RectTransform>().DOAnchorPosY(0.5f, 0.5f).SetEase(Ease.InQuad);
        });
    }
}
