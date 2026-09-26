using UnityEngine;
using UnityEngine.UI;

public sealed class AnimatedLogo : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float framesPerSecond = 12f;

    private Image _image;
    private float _timer;
    private int _frameIndex;

    public void SetFrames(Sprite[] slicedFrames)
    {
        frames = slicedFrames;
        _frameIndex = 0;
        _timer = 0f;
        ApplyFrame();
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
        ApplyFrame();
    }

    private void Update()
    {
        if (_image == null || frames == null || frames.Length == 0 || framesPerSecond <= 0f) return;

        _timer += Time.deltaTime;
        if (_timer < 1f / framesPerSecond) return;

        _timer = 0f;
        _frameIndex = (_frameIndex + 1) % frames.Length;
        ApplyFrame();
    }

    private void ApplyFrame()
    {
        if (_image != null && frames != null && frames.Length > 0)
            _image.sprite = frames[_frameIndex];
    }
}