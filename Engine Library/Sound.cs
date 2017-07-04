using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AiWonderTest
{
    public enum SoundType
    {
        BGM,
        SFX,
        Voice,
    }

    public class BGMManager
    {
        Cue cue;
        SoundEngine soundEngineDelegate_;
        public BGMManager(SoundEngine delegate_,string initName)
        {
            AIW.Log(this, "init");
            soundEngineDelegate_ = delegate_;
            cue = soundEngineDelegate_.soundBank.GetCue(initName);
        }
        bool isLooping = false;
        public void StartLoopBGM() { isLooping = true; }
        public void StopLoopBGM() { Stop(); isLooping = false; }
        public void UpdateLoopBGM(string name)
        {
            if (isLooping)
                Play(name);
        }
        public bool Play(string name)
        {
            if (cue.IsPaused)
            {
                cue.Resume();

            }
            else if (!cue.IsPlaying)
            {
                cue = soundEngineDelegate_.soundBank.GetCue(name);
                cue.Play();
               
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool Stop()
        {
            if (cue.IsPlaying)
            {
                cue.Stop(AudioStopOptions.Immediate);
                return true;
            }
            else if (cue.IsPaused)
            {
                cue.Resume();
                cue.Stop(AudioStopOptions.Immediate);
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool Pause()
        {

            if (cue.IsPlaying)
            {
                cue.Pause();
                return true;
            }
            return false;
        }

        public bool Resume()
        {
            if (cue.IsPaused)
            {
                cue.Resume();
                return true;
            }
            return false;
        }
    }

    public class SoundEngine
    {
        //Singlaton
        static SoundEngine instanceOfSoundEngine;
        public static SoundEngine sharedSoundEngine
        {
            get
            {
                if (instanceOfSoundEngine == null)
                    instanceOfSoundEngine = new SoundEngine();

                return instanceOfSoundEngine;
            }
        }


        public AudioEngine audioEngine;
        public SoundBank soundBank;
        public WaveBank waveBank;

        public BGMManager BGMManager;

        AudioCategory BGMCategory;
        AudioCategory SFXCategory;

        float BGMVolume = 1;
        float SFXVolume = 1;

        public void Play(string whatSound)
        {
            if (audioEngine != null)
            {
                soundBank.PlayCue(whatSound);
            }
        }

        public void SetVolume(SoundType type, float volume)
        {
            if (audioEngine != null)
            {
                if (volume < 0 || volume > 2)
                    volume = 1;

                if (type == SoundType.BGM)
                {
                    BGMVolume = volume;
                    BGMCategory.SetVolume(BGMVolume);
                }
                if (type == SoundType.SFX)
                {
                    SFXVolume = volume;
                    SFXCategory.SetVolume(SFXVolume);
                }

            }
        }
        void initVolume()
        {
            SetVolume(SoundType.BGM, 1.0f);
            SetVolume(SoundType.SFX, 1.0f);
        }
        public  void LoadContent()
        {
            audioEngine = new AudioEngine("Content\\Audio\\AudioEngine.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");
            BGMCategory = audioEngine.GetCategory("Music");
            SFXCategory = audioEngine.GetCategory("Interface");

            BGMManager = new BGMManager(this, "Shinobi");
            initVolume();
        }

        public void Update(GameTime gt)
        {
            if (audioEngine != null)
                audioEngine.Update();
        }
    }
}
