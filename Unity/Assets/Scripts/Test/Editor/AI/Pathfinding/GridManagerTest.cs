﻿using System.Collections.Generic;
using AI.Pathfinding;
using NUnit.Framework;

/**
 * @author Daniel Burnley
 */
namespace Test.AI.Pathfinding
{
    class GridManagerTest
    {
        private GridManager _gridManager;
        private List<Node> _expectedNodes;

        private void AddExpectedNode(float x, float z)
        {
            Node node = new Node(x, z);
            _expectedNodes.Add(node);
            _gridManager.AddNode(node.X, node.Z);
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
                Assert.True(_gridManager.IsWalkable(node.X, node.Z));
            }
        }

        private void AssertNotWalkable(float x, float z)
        {
            Assert.False(_gridManager.IsWalkable(x, z));
        }

        private void AssertClosestNode(Node currentNode, Node expectedNode)
        {
            Node actualNode = _gridManager.GetClosestNode(currentNode.X, currentNode.Z);
            Assert.AreEqual(expectedNode.X, actualNode.X);
            Assert.AreEqual(expectedNode.Z, actualNode.Z);
        }

        private void AssertNodeInGridManager(Node expectedNode)
        {
            List<float> zValues;
            Assert.True(_gridManager.GetNodes().TryGetValue(expectedNode.X, out zValues));
            Assert.True(zValues.Contains(expectedNode.Z));
        }

        [SetUp]
        public void SetUp()
        {
            _gridManager = new GridManager(new GridFake());
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
            _gridManager.AddNode(closestNode.X, closestNode.Z);
            AssertClosestNode(closestNode, closestNode);
        }

        [Test]
        public void givenTwoNodes_whenGettingClosestNode_returnClosestNode()
        {
            Node closestNode = new Node(1.5f, 1.5f);
            Node currentNode = new Node(2f, 2f);
            _gridManager.AddNode(closestNode.X, closestNode.Z);
            _gridManager.AddNode(-10f, -10f);
            AssertClosestNode(currentNode, closestNode);
        }
    }
}