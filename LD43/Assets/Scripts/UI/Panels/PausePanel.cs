using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private void Awake ()
    {
        gameObject.SetActive (false);
        this.RegisterAsListener ("Player", typeof (PlayerInputGameEvent));
    }

    public void OnGameEvent (PlayerInputGameEvent input)
    {
        if (input.GetInput () == "Escape")
        {
            gameObject.SetActive (true);
        }
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }

    public void Resume()
    {
        gameObject.SetActive (false);
    }
}

