using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectSL
{
    public class SoundManager : SingletonBase<SoundManager>
    {
        public float MasterVolume
        {
            get => AudioController.GetGlobalVolume();
            set => AudioController.SetGlobalVolume(value);
        }
        
        public float MusicVolume
        {
            get => AudioController.GetCategoryVolume("BGM");
            set => AudioController.SetCategoryVolume("BGM", value);
        }

        public float SFXVolume
        {
            get => AudioController.GetCategoryVolume("SFX");
            set => AudioController.SetCategoryVolume("SFX", value);
        }

        public void Initialize()
        {
            var audioControllerPrefab = Resources.Load<AudioController>("Sound/SL.AudioController");
            Instantiate(audioControllerPrefab, transform);
        }


        public void PlayMusic(string musicName)
        {
            AudioController.PlayMusic(musicName);
        }

        public void PlaySFX(string sfxName, Vector3 position)
        {
            AudioController.Play(sfxName, position);
        }

        public void PlayUISound(string uiSoundName)
        {
            AudioController.Play(uiSoundName, Camera.main.transform.position);
        }
    }
}
