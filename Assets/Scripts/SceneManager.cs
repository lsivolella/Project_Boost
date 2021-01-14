using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    int currentScene;

    SceneManager[] sceneManager;

    // Start is called before the first frame update
    void Start()
    {
        SetUpSingleton();
        GetCurrentScene();
    }

    private void Update()
    {
        ForceLoadNextLevel();
    }

    private void SetUpSingleton()
    {
        sceneManager = FindObjectsOfType<SceneManager>();

        if (sceneManager.Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadNextScene()
    {
        GetCurrentScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene + 1);
    }

    public void ReloadScene()
    {
        GetCurrentScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }

    private void GetCurrentScene()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }

    private void ForceLoadNextLevel()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                GetCurrentScene();
                int nextSceneIndex = currentScene + 1;

                if (nextSceneIndex >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
                {
                    nextSceneIndex = 0;
                }

                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }
}
