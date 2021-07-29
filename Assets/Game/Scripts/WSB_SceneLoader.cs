using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WSB_SceneLoader : MonoBehaviour
{
    public Collider2D Trigger = null;
    public bool LoadOnPlay = false;
    public List<ScenePicker> AllScenesToLoadInOrder = new List<ScenePicker>();
    public int LoadListSize = 0;
    public List<ScenePicker> AllScenesToUnloadInOrder = new List<ScenePicker>();
    public int UnloadListSize = 0;
    public BoxCollider2D BlockingCollider = null;

    public event Action OnScenesReady = null;

    bool hasLux = false;
    bool hasBan = false;

    void Start()
    {
        if (LoadOnPlay)
            NextScene();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Trigger)
            return;

        if (collision.GetComponent<WSB_Lux>())
            hasLux = true;
        if (collision.GetComponent<WSB_Ban>())
            hasBan = true;

        if (hasLux && hasBan)
        {
            if(Trigger)
            {
                if (Trigger.enabled)
                    NextScene();
            }
            else
                NextScene();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!Trigger)
            return;

        if (collision.GetComponent<WSB_Lux>())
            hasLux = false;
        if (collision.GetComponent<WSB_Ban>())
            hasBan = false;
    }

    List<AsyncOperation> loadingScenes = new List<AsyncOperation>();

    public void NextScene()
    {
        if(Trigger)
            Trigger.enabled = false;
        for (int i = 0; i < AllScenesToLoadInOrder.Count; i++)
        {
            loadingScenes.Add(SceneManager.LoadSceneAsync(AllScenesToLoadInOrder[i].SceneName, LoadSceneMode.Additive));
        }
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        while(loadingScenes.Any(s => !s.isDone))
            yield return new WaitForEndOfFrame();

        Unload();
    }

    void Unload()
    {
        if (BlockingCollider)
            BlockingCollider.gameObject.SetActive(false);

        for (int i = 0; i < AllScenesToUnloadInOrder.Count; i++)
        {
            SceneManager.UnloadSceneAsync(AllScenesToUnloadInOrder[i].SceneName);
        }

        OnScenesReady?.Invoke();

        if(LoadOnPlay && WSB_GameManager.I)
        {
            WSB_GameManager.I.StartGame();
            SceneManager.UnloadSceneAsync("Loading");
        }
    }
}

[Serializable]
public class ScenePicker
{
    public string ScenePath;
    public string SceneName;

    public ScenePicker(string _path, string _name)
    {
        ScenePath = _path;
        SceneName = _name;
    }

    public ScenePicker()
    {
        ScenePath = string.Empty;
        SceneName = string.Empty;
    }
}