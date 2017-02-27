using System.Collections.Generic;
using AI.Pathfinding;
using NUnit.Framework;

namespace Test.AI.Pathfinding
{
    class CalculatePathTest
    {
        private GridManager _gridManager;
        private CalculatePath _calculatePath;
        private PathfindingNode _startingNode;
        private PathfindingNode _destinationNode;

        private void AssertPathEquals(List<PathfindingNode> expectedPath)
        {
            List<PathfindingNode> actualPath = _calculatePath.GetPathToDestination(_startingNode.X, _startingNode.Z,
                _destinationNode.X, _destinationNode.Z);
            Assert.AreEqual(expectedPath.Count, actualPath.Count);
            for (int i = 0; i < expectedPath.Count; i++)
            {
                Assert.True(expectedPath[i].Equals(actualPath[i]));
            }
        }

        [SetUp]
        public void SetUp()
        {
            _gridManager = new GridManager();
            _calculatePath = new CalculatePath(_gridManager);
            _startingNode = new PathfindingNode(0f, 0f);
        }

        [Test]
        public void GivenStartingNodeNextToDestinationNode_returnDestinationNode()
        {
            _startingNode = new PathfindingNode(0, 0);
            _destinationNode = new PathfindingNode(0, 0.5f);
            _gridManager.AddNode(0, 0);
            _gridManager.AddNode(0, 0.5f);
            AssertPathEquals(new List<PathfindingNode>() { new PathfindingNode(0,0.5f)});
        }

        [Test]
        public void GivenStartingNodeIsDestinationNode_returnDestinationNode()
        {
            _startingNode = new PathfindingNode(0, 0);
            _destinationNode = _startingNode;
            _gridManager.AddNode(0, 0);
            AssertPathEquals(new List<PathfindingNode>() { new PathfindingNode(0, 0) });
        }

        [Test]
        public void TestSimpleCorridor()
        {
            _startingNode = new PathfindingNode(0,0);
            _destinationNode = new PathfindingNode(0,2);
            _gridManager.AddNode(0, 0);
            _gridManager.AddNode(0, 0.5f);
            _gridManager.AddNode(0, 1);
            _gridManager.AddNode(0, 1.5f);
            _gridManager.AddNode(0, 2);
            List<PathfindingNode> expectedPath = new List<PathfindingNode>()
            {
                new PathfindingNode(0, 0.5f),
                new PathfindingNode(0, 1),
                new PathfindingNode(0, 1.5f),
                _destinationNode
            };

            AssertPathEquals(expectedPath);
        }

        [Test]
        public void TestComplex()
        {
            _startingNode = new PathfindingNode(0,0);
            _destinationNode = new PathfindingNode(2,-0.5f);
            _gridManager.AddNode(-0.5f, 0.5f);
            _gridManager.AddNode(0, 0.5f);
            _gridManager.AddNode(0.5f, 0.5f);

            _gridManager.AddNode(0,0);

            _gridManager.AddNode(-0.5f, 0);
            _gridManager.AddNode(0.5f, 0);

            _gridManager.AddNode(-0.5f, -0.5f);
            _gridManager.AddNode(0.5f, -0.5f);
            _gridManager.AddNode(1.5f, -0.5f);
            _gridManager.AddNode(2, -0.5f);
            _gridManager.AddNode(2.5f, -0.5f);

            _gridManager.AddNode(-0.5f, -1);
            _gridManager.AddNode(0, -1);
            _gridManager.AddNode(0.5f, -1);
            _gridManager.AddNode(1, -1);
            _gridManager.AddNode(1.5f, -1);
            _gridManager.AddNode(2, -1);
            _gridManager.AddNode(2.5f, -1);

            List<PathfindingNode> expectedPath = new List<PathfindingNode>()
            {
                new PathfindingNode(0.5f, -0.5f),
                new PathfindingNode(1, -1),
                new PathfindingNode(1.5f, -0.5f),
                _destinationNode
            };

            AssertPathEquals(expectedPath);
        }
    }
}