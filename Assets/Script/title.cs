using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class title : MonoBehaviour
{
    [SerializeField] GameObject setting;
    private void Start()
    {
        setting.SetActive(false);
    }
    public void gamestart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Setting()
    {
        setting.SetActive(true);
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
    public void X()
    {
        setting.SetActive(false);
    }
}
