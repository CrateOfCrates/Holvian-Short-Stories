using UnityEngine;
using UnityEngine.SceneManagement; 

public class loadTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadSceneByName("CombatTestScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoadSceneByName(string sceneName)
    {
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
