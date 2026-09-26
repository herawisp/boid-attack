using UnityEngine;

public sealed class InfiniteCloudScroll : MonoBehaviour
{
    [SerializeField] private float speed = 35f;
    [SerializeField] private float wrapWidth = 1920f;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_rectTransform == null || wrapWidth <= 0f) return;

        Vector2 position = _rectTransform.anchoredPosition;
        position.x += speed * Time.deltaTime;
        if (position.x >= wrapWidth) position.x -= wrapWidth;
        _rectTransform.anchoredPosition = position;
    }
}