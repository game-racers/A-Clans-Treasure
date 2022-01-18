using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour 
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;
        [SerializeField] bool isMainMenu = false;
        [SerializeField] bool isEndGame = false;
        bool isTutorial = false;

        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag == "Player")
            {
                GameObject endGame = GameObject.FindGameObjectWithTag("End Game");
                if (endGame != null)
                {
                    isEndGame = true;
                }
                StartCoroutine(Transition());
            }
        }

        public void NewGame()
        {
            StartCoroutine(Transition());
        }

        public void Tutorial()
        {
            isTutorial = true;
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("ERROR: Scene to load is not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            if (!isMainMenu)
            {
                PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                playerController.enabled = false;
            }
            else
            {
                wrapper.Delete();
            }

            yield return fader.FadeOut(fadeOutTime);

            wrapper.Save();

            if (isTutorial)
            {
                yield return SceneManager.LoadSceneAsync(3);
            }
            else
            {
                yield return SceneManager.LoadSceneAsync(sceneToLoad);
            }

            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);

            fader.FadeIn(fadeInTime);

            newPlayerController.enabled = true;
            if (isEndGame)
            {
                otherPortal.isEndGame = true;
            }
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<PlayerController>().isEndGame = isEndGame;
            if (player.GetComponent<PlayerController>().isEndGame)
            {
                player.GetComponent<PlayerController>().EndGame();
            }
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}