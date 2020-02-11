using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] GameObject rulePanel;
    public void OnStartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Return()
    {
        rulePanel.SetActive(false);
    }

    public void RuleButton()
    {
        rulePanel.SetActive(true);
    }
}
