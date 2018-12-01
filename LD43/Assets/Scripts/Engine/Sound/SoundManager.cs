using UnityEngine;

public class SoundManager
{
    private AudioSource m_EfxSource;
    private AudioSource m_MusicSource;

    public SoundManager ()
    {
    }

    public void PlaySingle (AudioClip clip)
    {
        m_EfxSource.clip = clip;
        m_EfxSource.Play ();
    }

    public void PlayMultiple (AudioClip clip)
    {
        m_EfxSource.PlayOneShot (clip);
    }

    public void PlayMusic (AudioClip clip)
    {
        if (m_MusicSource.clip != clip)
        {
            m_MusicSource.clip = clip;
            m_MusicSource.Play ();
            m_MusicSource.loop = true;
        }
    }

    public void SetMusicSource (AudioSource source)
    {
        m_MusicSource = source;
    }

    public void SetFXSource (AudioSource source)
    {
        m_EfxSource = source;
    }
}

public class SoundManagerProxy : UniqueProxy<SoundManager>
{ }