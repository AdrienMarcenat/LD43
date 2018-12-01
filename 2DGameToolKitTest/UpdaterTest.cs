using System;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2DGameToolKitTest
{
    [TestClass]
    public class UpdaterTest
    {
        private Updater m_Updater;

        class DummyAILogic
        {
            public DummyAILogic ()
            {
                m_UpdateCount = 0;
            }

            public void UpdateAI ()
            {
                m_UpdateCount++;
            }

            public void UpdateLast ()
            {
                // This should not be allowed in an update callback
                this.RegisterToUpdate (EUpdatePass.First);
            }

            public int m_UpdateCount;
        }

        [TestInitialize]
        public void TestInitialize ()
        {
            m_Updater = new Updater ();
            UpdaterProxy.Open (m_Updater);
        }

        [TestCleanup]
        public void TestCleanup ()
        {
            UpdaterProxy.Close ();
        }

        [TestMethod]
        public void TestRegisterToUpdate ()
        {
            DummyAILogic objectToNotify = new DummyAILogic ();
            objectToNotify.RegisterToUpdate (EUpdatePass.AI);

            m_Updater.Update ();
            Assert.IsTrue (objectToNotify.m_UpdateCount == 1);

            m_Updater.Update ();
            Assert.IsTrue (objectToNotify.m_UpdateCount == 2);
        }

        [TestMethod]
        public void TestUnregisterToUpdate ()
        {
            DummyAILogic objectToNotify = new DummyAILogic ();
            objectToNotify.RegisterToUpdate (EUpdatePass.AI);

            m_Updater.Update ();
            Assert.IsTrue (objectToNotify.m_UpdateCount == 1);

            objectToNotify.UnregisterToUpdate (EUpdatePass.AI);
            m_Updater.Update ();
            Assert.IsTrue (objectToNotify.m_UpdateCount == 1);
        }

        [TestMethod]
        public void TestNoReentrancy ()
        {
            DummyAILogic objectToNotify = new DummyAILogic ();
            objectToNotify.RegisterToUpdate (EUpdatePass.Last);

            Assert.ThrowsException<SecurityException> (delegate { m_Updater.Update (); });
        }
    }
}
