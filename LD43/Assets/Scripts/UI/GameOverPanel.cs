using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    private void Awake ()
    {
        gameObject.SetActive (false);
        this.RegisterAsListener ("Player", typeof (GameOverGameEvent));
    }

    public void OnGameEvent (GameOverGameEvent gameOver)
    {
        gameObject.SetActive (true);
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Player");
    }
}
