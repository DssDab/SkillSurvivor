using System.Collections;
using TMPro;
using UnityEngine;

public class IntroSceneController : MonoBehaviour
{
    [SerializeField]
    private SceneNames nextScene;
    [SerializeField]
    private TextMeshProUGUI textPressAnyKey;
    private void Awake()
    {
        Application.targetFrameRate = 60;

        Database.Read();
    }
    private IEnumerator Start()
    {
        while (true)
        {
            yield return StartCoroutine(FadeEffect.Fade(textPressAnyKey, 1, 0));

            yield return StartCoroutine(FadeEffect.Fade(textPressAnyKey, 0, 1));
        }
    }

    private void Update()
    {
        if (Utils.IsAnyInputDown())
        {
            SceneLoader.Instance.LoadScene(nextScene);
        }
    }
}
