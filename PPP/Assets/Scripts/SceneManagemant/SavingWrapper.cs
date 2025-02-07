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


        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();

            yield return GetComponent<SavingSystem>().LoadLastScene(saveFileName);
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

        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(saveFileName);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(saveFileName);
        }
    }
}