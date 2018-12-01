using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security;

namespace _2DGameToolKitTest
{
    using Node = Node<int>;
    using Edge = Edge<int, int>;
    using Graph = Graph<int, int>;

    [TestClass]
    public class GraphTest
    {
        private Graph m_Graph;
        private Node m_Start;
        private Node m_End;
        private Edge m_Edge;

        [TestInitialize]
        public void TestInitialize ()
        {
            m_Graph = new Graph ();
            m_Start = new Node (1);
            m_End = new Node (2);
        }

        [TestCleanup]
        public void TestCleanup ()
        {}

        [TestMethod]
        public void TestNodeCreation ()
        {
            Assert.IsTrue (m_Start.GetData() == 1);
        }

        [TestMethod]
        public void TestUndirectedEdgeCreation ()
        {
            m_Edge = new Edge (3, m_Start, m_End, false);
            Assert.IsTrue (m_Edge.GetData () == 3);
            Assert.IsFalse (m_Edge.IsOriented ());
            Assert.IsTrue (m_Edge.UseNode(m_Start));
            Assert.IsTrue (m_Edge.UseNode (m_End));
            Assert.IsTrue (m_Start.IsNeighbor (m_End));
            Assert.IsTrue (m_End.IsNeighbor (m_Start));
        }

        [TestMethod]
        public void TestUndirectedEdgeDestruction ()
        {
            m_Edge = new Edge (3, m_Start, m_End, false);
            m_Edge.Shutdown ();
            Assert.IsFalse (m_Start.IsNeighbor (m_End));
            Assert.IsFalse (m_End.IsNeighbor (m_Start));
        }

        [TestMethod]
        public void TestDirectedEdgeCreation ()
        {
            m_Edge = new Edge (3, m_Start, m_End, true);
            Assert.IsTrue (m_Edge.GetData () == 3);
            Assert.IsTrue (m_Edge.IsOriented ());
            Assert.IsTrue (m_Edge.UseNode (m_Start));
            Assert.IsTrue (m_Edge.UseNode (m_End));
            Assert.IsTrue (m_Start.IsNeighbor (m_End));
            Assert.IsFalse (m_End.IsNeighbor (m_Start));
        }

        [TestMethod]
        public void TestDirectedEdgeDestruction ()
        {
            m_Edge = new Edge (3, m_Start, m_End, true);
            m_Edge.Shutdown ();
            Assert.IsFalse (m_Start.IsNeighbor (m_End));
            Assert.IsFalse (m_End.IsNeighbor (m_Start));
        }

        [TestMethod]
        public void TestDoubleEdgeAssert ()
        {
            m_Edge = new Edge (3, m_Start, m_End, true);
            Assert.ThrowsException<SecurityException> (delegate { Edge wrongEdge = new Edge (3, m_Start, m_End, true); });
        }

        [TestMethod]
        public void TestAddNodeToGraph ()
        {
            m_Graph.AddNode (m_Start);
            Assert.IsTrue (m_Graph.Contains (m_Start));
        }

        [TestMethod]
        public void TestRemoveNodeFromGraph ()
        {
            m_Graph.AddNode (m_Start);
            Assert.IsTrue (m_Graph.RemoveNode (m_Start));
            Assert.IsFalse (m_Graph.Contains (m_Start));
        }

        [TestMethod]
        public void TestAddEdgeToGraph ()
        {
            m_Edge = new Edge (3, m_Start, m_End, false);
            m_Graph.AddNode (m_Start);
            m_Graph.AddNode (m_End);
            m_Graph.AddEdge (m_Edge);
            Assert.IsTrue (m_Graph.Contains (m_Edge));
        }

        [TestMethod]
        public void TestRemoveEdgeFromGraph ()
        {
            m_Edge = new Edge (3, m_Start, m_End, false);
            m_Graph.AddNode (m_Start);
            m_Graph.AddNode (m_End);
            m_Graph.AddEdge (m_Edge);
            m_Graph.RemoveEdge (m_Edge);
            Assert.IsFalse (m_Graph.Contains (m_Edge));
            Assert.IsFalse (m_Start.IsNeighbor (m_End));
            Assert.IsFalse (m_End.IsNeighbor (m_Start));
        }

        [TestMethod]
        public void TestAssertIfEdgesNodeAreNotInTheGraph ()
        {
            m_Edge = new Edge (3, m_Start, m_End, false);
            Assert.ThrowsException<SecurityException> (delegate { m_Graph.AddEdge (m_Edge); });
        }
    }
}
