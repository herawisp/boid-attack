#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

[InitializeOnLoad]
internal static class StartupCloudScrollerInstaller
{
    static StartupCloudScrollerInstaller()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
        EditorApplication.delayCall += InstallActiveScene;
    }

    private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        EditorApplication.delayCall += () => Install(scene);
    }

    [MenuItem("Tools/Startup/Install Cloud Scroller")]
    private static void InstallActiveScene()
    {
        Install(SceneManager.GetActiveScene());
    }

    private static void Install(Scene scene)
    {
        EditorApplication.delayCall -= InstallActiveScene;

        if (!scene.IsValid() || scene.name != "Startup") return;

        GameObject cloud = FindCloud(scene);
        if (cloud == null) return;

        RectTransform rect = cloud.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.sizeDelta = new Vector2(1920f, 500f);
            rect.anchoredPosition = Vector2.zero;
        }

        if (cloud.GetComponent<InfiniteCloudScroll>() == null)
            Undo.AddComponent<InfiniteCloudScroll>(cloud);

        InstallLogo(scene);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    private static GameObject FindCloud(Scene scene)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == "clouds") return child.gameObject;
            }
        }

        return null;
    }

    private static void InstallLogo(Scene scene)
    {
        GameObject canvas = FindObject(scene, "Canvas");
        if (canvas == null) return;
        canvas.GetComponent<RectTransform>().localScale = Vector3.one;

        GameObject logo = FindObject(scene, "Logo");
        if (logo == null)
        {
            logo = new GameObject("Logo", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(AnimatedLogo));
            logo.transform.SetParent(canvas.transform, false);
            Undo.RegisterCreatedObjectUndo(logo, "Create startup logo");
        }

        RectTransform rect = logo.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(1920f, 1080f);

        Image image = logo.GetComponent<Image>();
        image.raycastTarget = false;
        image.preserveAspect = true;

        Sprite[] frames = AssetDatabase.LoadAllAssetsAtPath("Assets/Scenes/Assets/Logo animated-Sheet.png")
            .OfType<Sprite>()
            .OrderBy(sprite => int.Parse(sprite.name.Substring(sprite.name.LastIndexOf('_') + 1)))
            .ToArray();
        if (frames.Length == 0) return;

        image.sprite = frames[0];
        logo.GetComponent<AnimatedLogo>().SetFrames(frames);
        logo.transform.SetAsLastSibling();

        InstallStartButton(scene, canvas.transform);
        InstallSettingsAndExit(scene, canvas.transform);
    }

    private static void InstallStartButton(Scene scene, Transform canvasTransform)
    {
        GameObject startButton = FindObject(scene, "StartButton");
        if (startButton == null)
        {
            startButton = new GameObject("StartButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(StartButton));
            startButton.transform.SetParent(canvasTransform, false);
            Undo.RegisterCreatedObjectUndo(startButton, "Create startup button");
        }

        RectTransform rect = startButton.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, -260f);
        rect.sizeDelta = new Vector2(360f, 90f);

        Sprite normalSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Scenes/Assets/button.png");
        Sprite pressedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Scenes/Assets/button_pressed.png");
        Image image = startButton.GetComponent<Image>();
        image.sprite = normalSprite;
        image.preserveAspect = false;
        image.raycastTarget = true;

        Button button = startButton.GetComponent<Button>();
        button.targetGraphic = image;
        button.transition = Selectable.Transition.SpriteSwap;
        button.spriteState = new SpriteState
        {
            pressedSprite = pressedSprite,
            highlightedSprite = normalSprite,
            selectedSprite = normalSprite
        };
        GetOrCreateText(startButton.transform, "ButtonLabel", "START", 28);
        startButton.transform.SetAsLastSibling();
    }

    private static void InstallSettingsAndExit(Scene scene, Transform canvasTransform)
    {
        Sprite normalSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Scenes/Assets/button.png");
        Sprite pressedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Scenes/Assets/button_pressed.png");

        GameObject settings = FindObject(scene, "SettingsButton");
        if (settings == null)
        {
            settings = new GameObject("SettingsButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(SettingsMenu));
            settings.transform.SetParent(canvasTransform, false);
            Undo.RegisterCreatedObjectUndo(settings, "Create settings button");
        }
        ConfigureButton(settings, new Vector2(0f, -370f), normalSprite, pressedSprite);

        GameObject exit = FindObject(scene, "ExitButton");
        if (exit == null)
        {
            exit = new GameObject("ExitButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(ExitButton));
            exit.transform.SetParent(canvasTransform, false);
            Undo.RegisterCreatedObjectUndo(exit, "Create exit button");
        }
        ConfigureButton(exit, new Vector2(0f, -480f), normalSprite, pressedSprite);

        GameObject panel = FindObject(scene, "SettingsPanel");
        if (panel == null)
        {
            panel = new GameObject("SettingsPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Slider));
            panel.transform.SetParent(canvasTransform, false);
            Undo.RegisterCreatedObjectUndo(panel, "Create settings panel");
        }

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = new Vector2(0f, -590f);
        panelRect.sizeDelta = new Vector2(360f, 70f);

        Image panelImage = panel.GetComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.65f);
        panelImage.raycastTarget = true;

        Slider slider = panel.GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 1f;
        slider.direction = Slider.Direction.LeftToRight;

        Image trackImage = GetOrCreateImage(panel.transform, "VolumeTrack");
        RectTransform trackRect = trackImage.rectTransform;
        trackRect.anchorMin = new Vector2(0.1f, 0.35f);
        trackRect.anchorMax = new Vector2(0.9f, 0.65f);
        trackRect.offsetMin = Vector2.zero;
        trackRect.offsetMax = Vector2.zero;
        trackImage.color = new Color(1f, 1f, 1f, 0.35f);

        Image handleImage = GetOrCreateImage(panel.transform, "VolumeHandle");
        RectTransform handleRect = handleImage.rectTransform;
        handleRect.sizeDelta = new Vector2(24f, 44f);
        handleImage.color = Color.white;
        slider.handleRect = handleRect;
        slider.targetGraphic = handleImage;
        Text volumeLabel = GetOrCreateText(panel.transform, "VolumeLabel", "VOLUME", 18);
        volumeLabel.rectTransform.anchorMin = new Vector2(0f, 0.55f);
        volumeLabel.rectTransform.anchorMax = new Vector2(1f, 1f);
        volumeLabel.rectTransform.offsetMin = Vector2.zero;
        volumeLabel.rectTransform.offsetMax = Vector2.zero;

        SettingsMenu menu = settings.GetComponent<SettingsMenu>();
        SerializedObject serializedMenu = new SerializedObject(menu);
        serializedMenu.FindProperty("panel").objectReferenceValue = panel;
        serializedMenu.FindProperty("volumeSlider").objectReferenceValue = slider;
        serializedMenu.ApplyModifiedPropertiesWithoutUndo();

        Button settingsButton = settings.GetComponent<Button>();
        settingsButton.onClick.RemoveAllListeners();
        settingsButton.onClick.AddListener(menu.Toggle);

        Button exitButton = exit.GetComponent<Button>();
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(exit.GetComponent<ExitButton>().ExitGame);

        panel.SetActive(false);
        settings.transform.SetAsLastSibling();
        exit.transform.SetAsLastSibling();
    }

    private static Image GetOrCreateImage(Transform parent, string objectName)
    {
        Transform existing = parent.Find(objectName);
        if (existing != null) return existing.GetComponent<Image>();

        GameObject child = new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        child.transform.SetParent(parent, false);
        return child.GetComponent<Image>();
    }

    private static Text GetOrCreateText(Transform parent, string objectName, string content, int fontSize)
    {
        Transform existing = parent.Find(objectName);
        Text label = existing != null ? existing.GetComponent<Text>() : null;
        if (label == null)
        {
            GameObject child = new GameObject(objectName, typeof(RectTransform), typeof(Text));
            child.transform.SetParent(parent, false);
            label = child.GetComponent<Text>();
        }

        RectTransform rect = label.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        label.text = content;
        label.fontSize = fontSize;
        label.alignment = TextAnchor.MiddleCenter;
        label.color = Color.white;
        label.raycastTarget = false;
        return label;
    }

    private static void ConfigureButton(GameObject buttonObject, Vector2 position, Sprite normalSprite, Sprite pressedSprite)
    {
        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(360f, 90f);

        Image image = buttonObject.GetComponent<Image>();
        image.sprite = normalSprite;
        image.preserveAspect = false;
        image.raycastTarget = true;

        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = image;
        button.transition = Selectable.Transition.SpriteSwap;
        button.spriteState = new SpriteState
        {
            pressedSprite = pressedSprite,
            highlightedSprite = normalSprite,
            selectedSprite = normalSprite
        };
        GetOrCreateText(buttonObject.transform, "ButtonLabel", buttonObject.name.Replace("Button", "").ToUpperInvariant(), 28);
    }

    private static GameObject FindObject(Scene scene, string objectName)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == objectName) return child.gameObject;
            }
        }

        return null;
    }
}
#endif
