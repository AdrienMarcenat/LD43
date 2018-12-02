using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour
{
   [SerializeField] private AudioSource m_EfxSource;
   [SerializeField] private AudioSource m_MusicSource;

    private UnityLogger m_Logger;
    private Updater m_Updater;
    private GameEventManager m_GameEventManager;
    private InputManager m_InputManager;
    private CommandStack m_CommandStack;
    private SoundManager m_SoundManager;
    private LevelManager m_LevelManager;
    private TeamManager m_TeamManager;

    private GameFlowHSM m_GameFlowHSM;

    private static World ms_Instance;

    // This should be called before any other gameobject awakes
    private void Awake ()
    {
        // Singleton pattern : this is the only case where it should be used
        if(ms_Instance == null)
        {
            ms_Instance = this;
            DontDestroyOnLoad (gameObject);
            
            // Keep the Updater first, as the other members might want to register to it
            m_Logger = new UnityLogger ();
            LoggerProxy.Open (m_Logger);
            m_Updater = new Updater ();
            UpdaterProxy.Open (m_Updater);
            m_GameEventManager = new GameEventManager ();
            GameEventManagerProxy.Open (m_GameEventManager);
            m_InputManager = new InputManager ();
            InputManagerProxy.Open (m_InputManager);
            m_CommandStack = new CommandStack ();
            CommandStackProxy.Open (m_CommandStack);
            m_SoundManager = new SoundManager ();
            SoundManagerProxy.Open (m_SoundManager);
            m_SoundManager.SetMusicSource (m_MusicSource);
            m_SoundManager.SetFXSource (m_EfxSource);
            m_LevelManager = new LevelManager ();
            LevelManagerProxy.Open (m_LevelManager);
            m_TeamManager = new TeamManager ();
            TeamManagerProxy.Open (m_TeamManager);

            m_GameFlowHSM = new GameFlowHSM ();
            m_GameFlowHSM.Start (typeof (GameFlowMenuState));
        }
        else if (ms_Instance != this)
        {
            Destroy (gameObject);
            return;
        }
    }

    void Update ()
    {
        m_Updater.Update ();
    }
}
