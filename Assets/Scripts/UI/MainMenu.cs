using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.SceneManagement;

namespace RPG.UI
{
    public class MainMenu : MonoBehaviour
    {
        SavingWrapper wrapper;
        Portal portal;

        private void Start() 
        {
            wrapper = GameObject.FindObjectOfType<SavingWrapper>();
            portal = GameObject.FindObjectOfType<Portal>();
        }

        public void NewGame()
        {
            portal.NewGame();
        }

        public void ContinueGame()
        {
            wrapper.LoadGame();
        }

        public void Tutorial()
        {
            portal.Tutorial();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
