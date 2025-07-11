using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class mainmenu : MonoBehaviour
{
    [SerializeField] GameObject setting;
    private void Start()
    {
        setting.gameObject.SetActive(false);
    }
    public void gameStart()
    {
        SceneManager.LoadScene("");
    }
    public void Setting()
    {
        setting.gameObject.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void X()
    {
        setting.gameObject.SetActive(false);
    }
}
