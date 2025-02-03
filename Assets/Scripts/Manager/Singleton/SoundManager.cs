using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bk_source;
    public AudioSource ef_source;

    [SerializeField] private List<AudioClip> bk_music;
    [SerializeField] private List<AudioClip> ef_music;

    public Slider bkSlider;
    public Slider efSlider;

    protected override void Awake()
    {
        base.Awake();
        ef_source = this.AddComponent<AudioSource>();
        bk_source = this.AddComponent<AudioSource>();
        //bk_source.clip = bk_music[-1];
        bk_source.loop = true;
        if(bk_music.Count !=0)
        {
            bk_source.Play();
        }
    }
    public void BkgndMusicPlay(int index)
    {
        if (bk_music.Count < +index)
        {
            bk_source.Stop();
            bk_source.clip=bk_music[index];
            bk_source.Play();
        }
        else
        {
            Debug.Log(index + " is Wrong Index [BackGround]" );
        }
        
    }
    public void EffectSoundPlay(int index)
    {
        if (ef_music.Count < +index)
        {
            ef_source.PlayOneShot(ef_music[index]);
        }
        else
        {
            Debug.Log(index + " is Wrong Index [Effect]");
        }
        
    }
    public void SetBkgndVolume()
    {
        bk_source.volume = bkSlider.value;
    }
    public void SetEffectVolume()
    {
        ef_source.volume= efSlider.value;
    }
}