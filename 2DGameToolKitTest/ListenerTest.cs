using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security;

namespace _2DGameToolKitTest
{
    [TestClass]
    public class ListenerTest
    {
        class DummyGameEvent : GameEvent
        {
            public DummyGameEvent () : base ("Dummy", EProtocol.Instant)
            { }
        }

        class SecondDummyGameEvent : GameEvent
        {
            public SecondDummyGameEvent () : base ("Dummy", EProtocol.Instant)
            { }
        }

        class ThirdDummyGameEvent : GameEvent
        {
            public ThirdDummyGameEvent () : base ("Dummy", EProtocol.Delayed)
            { }
        }

        class DummyListener
        {
            public DummyListener ()
            {
                m_DummyEventReceived = false;
            }

            public void OnGameEvent (DummyGameEvent dummyEvent)
            {
                m_DummyEventReceived = true;
            }

            public void OnGameEvent (ThirdDummyGameEvent dummyEvent)
            {
                // This should not be allowed in an event callback
                this.RegisterAsListener ("Dummy", typeof (DummyGameEvent));
            }

            public bool m_DummyEventReceived;
        }
        
        private GameEventManager m_EventManager;
        private Updater m_Updater;

        [TestInitialize]
        public void TestInitialize ()
        {
            m_Updater = new Updater ();
            UpdaterProxy.Open (m_Updater);
            m_EventManager = new GameEventManager ();

            GameEventManagerProxy.Open (m_EventManager);
        }

        [TestCleanup]
        public void TestCleanup ()
        {
            GameEventManagerProxy.Close ();
            UpdaterProxy.Close ();
        }
        
        [TestMethod]
        public void TestEventReception ()
        {
            DummyListener listener = new DummyListener ();
            listener.RegisterAsListener ("Dummy", typeof (DummyGameEvent));

            new DummyGameEvent ().Push ();
            Assert.IsTrue (listener.m_DummyEventReceived);
        }

        [TestMethod]
        public void TestTag ()
        {
            DummyListener listener = new DummyListener ();
            listener.RegisterAsListener ("Not Dummy", typeof (DummyGameEvent));

            new DummyGameEvent ().Push ();
            Assert.IsFalse (listener.m_DummyEventReceived);
        }

        [TestMethod]
        public void TestUnregister ()
        {
            DummyListener listener = new DummyListener ();
            listener.RegisterAsListener ("Dummy", typeof (DummyGameEvent));
            Assert.ThrowsException<SecurityException> (delegate { listener.UnregisterAsListener ("Not Dummy"); });

            listener.UnregisterAsListener ("Dummy");
            new DummyGameEvent ().Push ();
            Assert.IsFalse (listener.m_DummyEventReceived);
        }

        [TestMethod]
        public void TestReflectionError ()
        {
            DummyListener listener = new DummyListener ();
            listener.RegisterAsListener ("Dummy", typeof (SecondDummyGameEvent));
            Assert.ThrowsException<SecurityException> (delegate { new SecondDummyGameEvent ().Push (); });
        }

        [TestMethod]
        public void TestNoReentrancy ()
        {
            DummyListener listener = new DummyListener ();
            listener.RegisterAsListener ("Dummy", typeof (ThirdDummyGameEvent));
            new ThirdDummyGameEvent ().Push ();
            Assert.ThrowsException<SecurityException> (delegate { m_EventManager.UpdateFirst (); });
        }
    }
}
