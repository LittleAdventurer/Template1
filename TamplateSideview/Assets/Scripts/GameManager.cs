using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //_notiGameOverHandler += new NotiGameOverHandler(PlayerControl.GameOver());
        NotifyGameOver();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Debug_NextScene();
        }
    }

    delegate void NotiGameOverHandler();
    NotiGameOverHandler _notiGameOverHandler;
    public void NotifyGameOver()
    {
        _notiGameOverHandler();
    }

    private void Debug_NextScene()
    {
        Debug.Log("SampleScene2 Load");
        SceneManager.LoadScene("SampleScene2");
    }
}

interface IGameOverObsever
{
    void GameOver();
}