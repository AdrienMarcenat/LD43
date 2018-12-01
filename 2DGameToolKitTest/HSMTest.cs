using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace _2DGameToolKitTest
{
    [TestClass]
    public class HSMTest
    {
        class DummyStateBase : HSMState
        {
            public override void OnEnter ()
            {
                m_IsInStack = true;
            }

            public override void OnExit ()
            {
                m_IsInStack = false;
            }

            public bool m_IsInStack;
        }

        class DummyState : DummyStateBase
        {
            private bool m_FirtsOnEnter = true;

            public override void OnEnter ()
            {
                base.OnEnter ();
                if (m_FirtsOnEnter)
                {
                    ChangeNextTransition (HSMTransition.EType.Child, typeof (DummyStateChild));
                    m_FirtsOnEnter = false;
                }
                else
                {
                    ChangeNextTransition (HSMTransition.EType.Exit);
                }
            }
        }

        class DummyStateChild : DummyStateBase
        {
            public override void OnEnter ()
            {
                base.OnEnter ();
                ChangeNextTransition (HSMTransition.EType.Siblings, typeof (DummyStateSibling));
            }
        }

        class DummyStateSibling : DummyStateBase
        {
            public override void OnEnter ()
            {
                base.OnEnter ();
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (DummyState));
            }
        }

        class DummyHSM : HSM
        {
            public DummyHSM ()
               : base (new DummyState ()
                     , new DummyStateChild ()
                     , new DummyStateSibling ()
               )
            { }
        }
        
        private DummyHSM m_HSM;
        private Dictionary<System.Type, HSMState> m_States;

        private bool CheckIsInStack<T>() where T : DummyStateBase
        {
            return ((T)m_States[typeof(T)]).m_IsInStack;
        }

        private void PerformHSMUpdate(int numberOfSteps)
        {
            for(int i = 0; i < numberOfSteps; i++)
            {
                m_HSM.UpdateLast ();
            }
        }

        [TestInitialize]
        public void TestInitialize ()
        {
            UpdaterProxy.Open (new Updater());
            LoggerProxy.Open (new NullLogger ());
            m_HSM = new DummyHSM ();
            m_States = m_HSM.GetStates ();
        }

        [TestCleanup]
        public void TestCleanup ()
        {
            LoggerProxy.Close ();
            UpdaterProxy.Close ();
        }
        
        [TestMethod]
        public void TestStart ()
        {
            Assert.IsTrue (m_HSM.IsEmpty ());
            m_HSM.Start (typeof (DummyState));
            HSMState[] currentStack = m_HSM.GetStack ();
            Assert.IsTrue (currentStack.Length == 1);
            Assert.IsTrue (currentStack[0].GetID () == typeof (DummyState));
            Assert.IsTrue (CheckIsInStack<DummyState> ());
        }

        [TestMethod]
        public void TestChildTransition ()
        {
            m_HSM.Start (typeof (DummyState));
            PerformHSMUpdate (1);
            HSMState[] currentStack = m_HSM.GetStack ();
            Assert.IsTrue (currentStack.Length == 2);
            Assert.IsTrue (currentStack[1].GetID () == typeof (DummyState));
            Assert.IsTrue (CheckIsInStack<DummyState> ());
            Assert.IsTrue (currentStack[0].GetID () == typeof (DummyStateChild));
            Assert.IsTrue (CheckIsInStack<DummyStateChild> ());
        }

        [TestMethod]
        public void TestSiblingTransition ()
        {
            m_HSM.Start (typeof (DummyState));
            PerformHSMUpdate (2);
            HSMState[] currentStack = m_HSM.GetStack ();
            Assert.IsTrue (currentStack.Length == 2);
            Assert.IsTrue (currentStack[1].GetID () == typeof (DummyState));
            Assert.IsTrue (CheckIsInStack<DummyState> ());
            Assert.IsTrue (currentStack[0].GetID () == typeof (DummyStateSibling));
            Assert.IsTrue (CheckIsInStack<DummyStateSibling> ());
            Assert.IsFalse (CheckIsInStack<DummyStateChild> ());
        }

        [TestMethod]
        public void TestClearTransition ()
        {
            m_HSM.Start (typeof (DummyState));
            PerformHSMUpdate (3);
            HSMState[] currentStack = m_HSM.GetStack ();
            Assert.IsTrue (currentStack.Length == 1);
            Assert.IsTrue (currentStack[0].GetID () == typeof (DummyState));
            Assert.IsTrue (CheckIsInStack<DummyState> ());
            Assert.IsFalse (CheckIsInStack<DummyStateChild> ());
            Assert.IsFalse (CheckIsInStack<DummyStateSibling> ());
        }

        [TestMethod]
        public void TestExitTransition ()
        {
            m_HSM.Start (typeof (DummyState));
            PerformHSMUpdate (4);
            HSMState[] currentStack = m_HSM.GetStack ();
            Assert.IsTrue (currentStack.Length == 0);
            Assert.IsFalse (CheckIsInStack<DummyState> ());
            Assert.IsFalse (CheckIsInStack<DummyStateChild> ());
            Assert.IsFalse (CheckIsInStack<DummyStateSibling> ());
        }

        [TestMethod]
        public void TestStop ()
        {
            m_HSM.Start (typeof (DummyState));
            PerformHSMUpdate (3);
            m_HSM.Stop ();
            HSMState[] currentStack = m_HSM.GetStack ();
            Assert.IsTrue (currentStack.Length == 0);
            Assert.IsFalse (CheckIsInStack<DummyState> ());
            Assert.IsFalse (CheckIsInStack<DummyStateChild> ());
            Assert.IsFalse (CheckIsInStack<DummyStateSibling> ());
        }
    }
}
