using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsMenu : MonoBehaviour
    {
        private static bool SkipCutscenes { get; set; }
        
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle skipCutscenesToggle;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        private FMOD.Studio.VCA musicVca;
        private FMOD.Studio.VCA sfxVca;

        void Start()
        {
            fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
            
            skipCutscenesToggle.SetIsOnWithoutNotify(SkipCutscenes);
            skipCutscenesToggle.onValueChanged.AddListener(SetSkipCutscenes);

            musicVca = RuntimeManager.GetVCA("vca:/Music");
            sfxVca = RuntimeManager.GetVCA("vca:/SFX");

            musicVca.getVolume(out float music, out _);
            musicSlider.SetValueWithoutNotify(music);
            musicSlider.onValueChanged.AddListener(SetMusic);
            
            sfxVca.getVolume(out float sfx, out _);
            sfxSlider.SetValueWithoutNotify(sfx);
            sfxSlider.onValueChanged.AddListener(SetSfx);
        }

        private static void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        private static void SetSkipCutscenes(bool isSkipCutscenes)
        {
            SkipCutscenes = isSkipCutscenes;
        }
        
        private void SetMusic(float val)
        {
            musicVca.setVolume(val);
        }
        
        private void SetSfx(float val)
        {
            sfxVca.setVolume(val);
        }
    }
}