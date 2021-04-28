using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WSB_SceneLoader : MonoBehaviour
{
    const string MAINSCENENAME = "Persos & Cam";

    //[SerializeField] List<string> allScenesInOrder = new List<string>();
    [SerializeField] BoxCollider2D trigger = null;
    [SerializeField] bool loadOnPlay = false;
    [SerializeField] List<SceneAsset> allScenesInOrder = new List<SceneAsset>();

    bool hasLux = false;
    bool hasBan = false;

    void Start()
    {
        if (!loadOnPlay)
            return;

        for (int i = 0; i < allScenesInOrder.Count; i++)
        {
            SceneManager.LoadScene(allScenesInOrder[i].name, LoadSceneMode.Additive);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_Lux>())
            hasLux = true;
        if (collision.GetComponent<WSB_Ban>())
            hasBan = true;

        if (hasLux && hasBan && trigger.enabled)
            NextScene();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_Lux>())
            hasLux = false;
        if (collision.GetComponent<WSB_Ban>())
            hasBan = false;
    }

    List<AsyncOperation> loadingScenes = new List<AsyncOperation>();

    private void NextScene()
    {
        if(trigger)
            trigger.enabled = false;
        for (int i = 0; i < allScenesInOrder.Count; i++)
        {
            loadingScenes.Add(SceneManager.LoadSceneAsync(allScenesInOrder[i].name, LoadSceneMode.Additive));
        }
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        while(loadingScenes.Any(s => !s.isDone))
            yield return new WaitForEndOfFrame();

        Loaded();
    }

    void Loaded()
    {
        WSB_SceneLoader[] _loaders = FindObjectsOfType<WSB_SceneLoader>();
        for (int i = 0; i < _loaders.Length; i++)
        {
            if (_loaders[i] != this)
                _loaders[i].Unload();
        }
    }

    public void Unload()
    {
        for (int i = allScenesInOrder.Count - 1; i >= 0; i--)
        {
            if (allScenesInOrder[i].name == MAINSCENENAME)
                continue;

            SceneManager.UnloadSceneAsync(allScenesInOrder[i].name);
        }
    }
}
