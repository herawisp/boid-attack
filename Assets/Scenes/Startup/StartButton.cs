using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public sealed class StartButton : MonoBehaviour
{
    [SerializeField] private string sceneName = "Main";
    [SerializeField] private float clickDelay = 0.12f;
    [SerializeField] private float pressedScale = 0.92f;

    private Button _button;
    private Vector3 _initialScale;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _initialScale = transform.localScale;
        _button.onClick.AddListener(HandleClick);
    }

    private void OnDestroy()
    {
        if (_button != null) _button.onClick.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        _button.interactable = false;
        StartCoroutine(LoadSceneAfterClick());
    }

    private IEnumerator LoadSceneAfterClick()
    {
        transform.localScale = _initialScale * pressedScale;
        yield return new WaitForSecondsRealtime(clickDelay);
        SceneManager.LoadScene(sceneName);
    }
}