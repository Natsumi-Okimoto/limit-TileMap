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
    /// Stage�V�[���ւ̑J�ڏ���
    /// </summary>
    /// <param name="nextLoadSceneName"></param>
    public void PreparateStageScene(SceneName nextLoadSceneName)
    {
        StartCoroutine(LoadStageScene(nextLoadSceneName));
    }
    /// <summary>
    /// Stage�V�[���֑J��
    /// </summary>
    /// <param name="nextLoadSceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadStageScene(SceneName nextLoadSceneName)
    {
        string oldSceneName = SceneManager.GetActiveScene().name;//Battle�̖��O��ۑ�

        Scene scene = SceneManager.GetSceneByName(nextLoadSceneName.ToString());//���ꂩ��ǂݍ��ރV�[���̏��

        while (!scene.isLoaded)//TRUE���ǂ����̊m�F
        {
            yield return null;
        }

        SceneManager.SetActiveScene(scene);//Main��������悤�ɂ���

        stage.gameObject.SetActive(true);//�`�F�b�N��t����

        SceneManager.UnloadSceneAsync(oldSceneName);//Battle�V�[���̔j��
    }
    /// <summary>
    /// Battle�V�[���ւ̑J�ڏ���
    /// </summary>
    public void PreparateBatlleScene()
    {
        Debug.Log("Load Battle Scene");

        StartCoroutine(LoadBattleScene());
    }
    /// <summary>
    /// Battle�V�[���ւ̑J��
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
