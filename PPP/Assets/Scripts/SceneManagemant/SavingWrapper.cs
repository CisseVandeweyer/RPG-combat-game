using System;
using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {

        const string saveFileName = "save";

        [SerializeField] float fadeInTime = 2f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }


        IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(saveFileName);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();

            yield return fader.FadeIn(fadeInTime);

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }

        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(saveFileName);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(saveFileName);
        }
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(saveFileName);
        }
    }
}