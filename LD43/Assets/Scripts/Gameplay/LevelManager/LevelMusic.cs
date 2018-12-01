using UnityEngine;

public class LevelMusic : MonoBehaviour
{
	[SerializeField] private AudioClip m_LevelMusic;

	void Start ()
	{
		SoundManagerProxy.Get().PlayMusic (m_LevelMusic);
	}
}

