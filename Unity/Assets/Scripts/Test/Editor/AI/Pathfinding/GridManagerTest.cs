using System.Collections.Generic;
using AI.Pathfinding;
using NUnit.Framework;

namespace Test.AI.Pathfinding
{
    class GridManagerTest
    {

        
        private GridManager _gridManager;
        private List<Node> _expectedNodes;
        private float _currentX;
        private float _currentZ;

        private void AddExpectedNode(float x, float z)
        {
            Node node = new Node(x, z);
            _expectedNodes.Add(node);
            _gridManager.AddNode(node.x, node.z);
        }

        private void AssertNodesCorrect()
        {
            foreach (Node node in _expectedNodes)
            {
                AssertNodeInGridManager(node);
            }
        }

        private void AssertNodesWalkable()
        {
            foreach (Node node in _expectedNodes)
            {
                Assert.True(_gridManager.IsWalkable(node.x, node.z));
            }
        }

        private void AssertNotWalkable(float x, float z)
        {
            Assert.False(_gridManager.IsWalkable(x, z));
        }

        private void AssertClosestNode(Node currentNode, Node expectedNode)
        {
            Node actualNode = _gridManager.GetClosestNode(currentNode.x, currentNode.z);
            Assert.AreEqual(expectedNode.x, actualNode.x);
            Assert.AreEqual(expectedNode.z, actualNode.z);
        }

        private void AssertNodeInGridManager(Node expectedNode)
        {
            List<float> zValues;
            Assert.True(_gridManager.GetNodes().TryGetValue(expectedNode.x, out zValues));
            Assert.True(zValues.Contains(expectedNode.z));
        }

        [SetUp]
        public void SetUp()
        {
            _gridManager = new GridManager();
            _expectedNodes = new List<Node>();
        }

        [Test]
        public void givenSingleNode_whenAddingNode_correctlyAddNodeToNodes()
        { 
            AddExpectedNode(1.0f, 1.0f);
            AssertNodesCorrect();
        }

        [Test]
        public void givenMultipleNodes_whenAddingNodes_correctlyAddNodes()
        {
            AddExpectedNode(1.0f, 1.0f);
            AddExpectedNode(2.0f, 2.0f);
            AssertNodesCorrect();
        }

        [Test]
        public void givenMultipleNodes_whenAddingNodes_sortZValuesCorrectly()
        {
            _gridManager.AddNode(1.0f, 1.0f);
            _gridManager.AddNode(1.0f, 0.0f);
            _gridManager.AddNode(1.0f, 2.0f);
            List<float> zValues = _gridManager.GetNodes()[1.0f];
            var expected = new List<float>() { 0.0f, 1.0f, 2.0f };
            CollectionAssert.AreEqual(expected , zValues);
        }

        [Test]
        public void givenNode_whenGettingIsWalkable_returnWalkable()
        {
            AddExpectedNode(1.5f, 1.5f);
            AssertNodesWalkable();
        }

        [Test]
        public void givenNodes_whenGettingIsWalkable_returnWalkable()
        {
            AddExpectedNode(1.5f, 1.5f);
            AddExpectedNode(1.0f, 1.0f);
            AssertNodesWalkable();
        }

        [Test]
        public void givenNode_whenGettingNonExistingNodeWalkable_returnNotWalkable()
        {
            _gridManager.AddNode(1.5f, 1.5f);
            AssertNotWalkable(1.5f, 1.0f);
        }

        [Test]
        public void givenNode_whenGettingClosestNode_returnNode()
        {
            Node closestNode = new Node(1.5f, 1.5f);
            _gridManager.AddNode(closestNode.x, closestNode.z);
            AssertClosestNode(closestNode, closestNode);
        }

        [Test]
        public void givenTwoNodes_whenGettingClosestNode_returnClosestNode()
        {
            Node closestNode = new Node(1.5f, 1.5f);
            Node currentNode = new Node(2f, 2f);
            _gridManager.AddNode(closestNode.x, closestNode.z);
            _gridManager.AddNode(-10f, -10f);
            AssertClosestNode(currentNode, closestNode);
        }
    }
}