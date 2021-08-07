using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager instance;

    [SerializeField]
    public Stage stage;

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


   public void NextScene(SceneName NextSceneName)
    {
        SceneManager.LoadScene(NextSceneName.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //NextScene(SceneName.Main);
            PreparateStageScene(SceneName.Main);
        }
    }
    /// <summary>
    /// Stageシーンへの遷移準備
    /// </summary>
    /// <param name="nextLoadSceneName"></param>
    public void PreparateStageScene(SceneName nextLoadSceneName)
    {
        StartCoroutine(LoadStageScene(nextLoadSceneName));
    }
    /// <summary>
    /// Stageシーンへ遷移
    /// </summary>
    /// <param name="nextLoadSceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadStageScene(SceneName nextLoadSceneName)
    {
        string oldSceneName = SceneManager.GetActiveScene().name;//Battleの名前を保存

        Scene scene = SceneManager.GetSceneByName(nextLoadSceneName.ToString());//これから読み込むシーンの情報

        while (!scene.isLoaded)//TRUEかどうかの確認
        {
            yield return null;
        }

        SceneManager.SetActiveScene(scene);//Mainを見えるようにする

        stage.gameObject.SetActive(true);//チェックを付ける

        SceneManager.UnloadSceneAsync(oldSceneName);//Battleシーンの破棄
    }
    /// <summary>
    /// Battleシーンへの遷移準備
    /// </summary>
    public void PreparateBatlleScene()
    {
        Debug.Log("Load Battle Scene");

        StartCoroutine(LoadBattleScene());
    }
    /// <summary>
    /// Battleシーンへの遷移
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadBattleScene()
    {
        SceneManager.LoadScene(SceneName.Battle.ToString(), LoadSceneMode.Additive);

        Scene scene = SceneManager.GetSceneByName(SceneName.Battle.ToString());

        yield return new WaitUntil(() => scene.isLoaded);

        stage.gameObject.SetActive(false);

        SceneManager.SetActiveScene(scene);
    }
}
