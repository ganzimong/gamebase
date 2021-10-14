using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class SceneLoader : MonoSingleton<SceneLoader>
{
    public enum SceneType
    {
        Entry,
        Lobby,
        Ingame,
        //..Add scene name
    }

    public SceneType CurrentScene { get; private set; }
    public SceneType PreviousScene { get; private set; }
    public bool IsLoading { get; private set; }
    public bool IsFirstSceneAfterLaunchingApp { get { return CurrentScene == SceneType.Entry && PreviousScene == SceneType.Entry; } }

    public float fadeSpeed = 2f;
    public float minLoadingTime = 2f;

    private Action _completeCallback;
    private CanvasGroup _canvasGroup;
    private float _elapsedTime;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        CurrentScene = SceneType.Entry;
        PreviousScene = SceneType.Entry;
    }

    public void LoadScene(SceneType scene, Action callback)
    {
        if (IsLoading)
        {
            Debug.LogError("Another scene is loading");
            return;
        }

        StartCoroutine(CoLoad(scene, callback));
    }

    private IEnumerator CoLoad(SceneType scene, Action callback)
    {
        Debug.LogFormat("Start to load {0} scene", scene);

        _completeCallback = callback;
        IsLoading = true;

        yield return CoPreLoad();

        float startTime = Time.realtimeSinceStartup;

        var async = SceneManager.LoadSceneAsync(scene.ToString());
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            Debug.LogFormat("{0}%", async.progress * 100);

            yield return null;
        }

        async.allowSceneActivation = true;

        GC.Collect();

        PreviousScene = CurrentScene;
        CurrentScene = scene;

        _elapsedTime = Time.realtimeSinceStartup - startTime;

        if (_elapsedTime < minLoadingTime)
        {
            yield return new WaitForSeconds(minLoadingTime - _elapsedTime);
        }

        yield return CoPostLoad();

        IsLoading = false;

        Debug.LogFormat("Load complete({0})", _elapsedTime);

        if (_completeCallback != null)
        {
            _completeCallback();
        }
    }

    private IEnumerator CoPreLoad()
    {
        yield return CoFade(true);
    }

    private IEnumerator CoPostLoad()
    {
        yield return CoFade(false);
    }

    private IEnumerator CoFade(bool isFadeIn)
    {
        _canvasGroup.alpha = isFadeIn ? 0f : 1f;

        float to = 1f - _canvasGroup.alpha;

        while (Mathf.Abs(_canvasGroup.alpha - to) > 0.01f)
        {
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, to, Time.deltaTime * fadeSpeed);
            yield return null;
        }
    }
}
