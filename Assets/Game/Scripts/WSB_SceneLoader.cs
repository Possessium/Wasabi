using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WSB_SceneLoader : MonoBehaviour
{
    [SerializeField] List<string> allScenesInOrder = new List<string>();

    [SerializeField] bool loadOnPlay = true;
    void Start()
    {
        if (!loadOnPlay)
            return;

        for (int i = 0; i < allScenesInOrder.Count; i++)
        {
            SceneManager.LoadScene(allScenesInOrder[i], LoadSceneMode.Additive);
        }
    }

}
