using System.Collections;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI 
{
    public class DeathScreen : MonoBehaviour 
    {
        [SerializeField] float fadeTime = 1.0f;
        [SerializeField] float alpha = 1f; 
        CanvasGroup canvasGroup;

        private void Awake() 
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
        }

        private void Start() 
        {
            StartCoroutine(FadeRoutine(alpha, fadeTime));
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

        public void LoadGame()
        {
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.LoadGame();
            StartCoroutine(FadeRoutine(0f, fadeTime));
        }
    }
}