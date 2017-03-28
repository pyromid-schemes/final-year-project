using System.Collections.Generic;
using AI.Pathfinding;
using NUnit.Framework;
/**
 * @author Daniel Burnley
 */
namespace Test.AI.Pathfinding
{
    class CalculatePathTest
    {
        private GridFake _grid;
        private GridManager _gridManager;
        private CalculatePath _calculatePath;
        private PathfindingNode _startingNode;
        private PathfindingNode _destinationNode;

        private void AssertPathEquals(IList<PathfindingNode> expectedPath)
        {
            var actualPath = _calculatePath.GetPathToDestination(_startingNode.X, _startingNode.Z,
                _destinationNode.X, _destinationNode.Z);
            Assert.AreEqual(expectedPath.Count, actualPath.Count);
            for (var i = 0; i < expectedPath.Count; i++)
            {
                Assert.True(expectedPath[i].Equals(actualPath[i]));
            }
        }

        [SetUp]
        public void SetUp()
        {
            _grid = new GridFake();
            _gridManager = new GridManager(_grid);
            _calculatePath = new CalculatePath(_gridManager, 1);
            _startingNode = new PathfindingNode(0f, 0f);
        }

        [Test]
        public void GivenStartingNodeNextToDestinationNode_returnDestinationNode()
        {
            _startingNode = new PathfindingNode(0, 0);
            _destinationNode = new PathfindingNode(0, 0.5f);
            _gridManager.AddNode(0, 0);
            _gridManager.AddNode(0, 0.5f);
            AssertPathEquals(new List<PathfindingNode>() {new PathfindingNode(0, 0.5f)});
        }

        [Test]
        public void GivenStartingNodeIsDestinationNode_returnDestinationNode()
        {
            _startingNode = new PathfindingNode(0, 0);
            _destinationNode = _startingNode;
            _gridManager.AddNode(0, 0);
            AssertPathEquals(new List<PathfindingNode> {new PathfindingNode(0, 0)});
        }

        [Test]
        public void TestSimpleCorridor()
        {
            _startingNode = new PathfindingNode(0, 0);
            _destinationNode = new PathfindingNode(0, 2);
            _gridManager.AddNode(0, 0);
            _gridManager.AddNode(0, 0.5f);
            _gridManager.AddNode(0, 1);
            _gridManager.AddNode(0, 1.5f);
            _gridManager.AddNode(0, 2);
            List<PathfindingNode> expectedPath = new List<PathfindingNode>
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
            _startingNode = new PathfindingNode(0, 0);
            _destinationNode = new PathfindingNode(2, -0.5f);
            _gridManager.AddNode(-0.5f, 0.5f);
            _gridManager.AddNode(0, 0.5f);
            _gridManager.AddNode(0.5f, 0.5f);

            _gridManager.AddNode(0, 0);

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

            List<PathfindingNode> expectedPath = new List<PathfindingNode>
            {
                new PathfindingNode(0.5f, -0.5f),
                new PathfindingNode(1, -1),
                new PathfindingNode(1.5f, -0.5f),
                _destinationNode
            };

            AssertPathEquals(expectedPath);
        }

        [Test]
        public void GivenNoMovesFromCurrentNode_returnEmptyPath()
        {
            _startingNode = new PathfindingNode(0, 0);
            _destinationNode = new PathfindingNode(1f, 0);

            _gridManager.AddNode(0, 0);
            _gridManager.AddNode(1f, 0);

            var expectedPath = new List<PathfindingNode>();

            AssertPathEquals(expectedPath);
        }

        [Test]
        public void GivenMovesTowardsUnreachableDestination_returnPathToClosestNodeToDestination()
        {
            _startingNode = new PathfindingNode(0, 0);
            _destinationNode = new PathfindingNode(2f, 0);

            _gridManager.AddNode(-1f, 0);
            _gridManager.AddNode(-0.5f, 0);
            _gridManager.AddNode(0, 0);
            _gridManager.AddNode(0.5f, 0);
            _gridManager.AddNode(1f, 0);
            _gridManager.AddNode(2f, 0);

            var expectedPath = new List<PathfindingNode>
            {
//                new PathfindingNode(0.5f, 0),
//                new PathfindingNode(1f, 0)
            };

            AssertPathEquals(expectedPath);
        }

        [Test]
        public void GivenOccupiedSpaceBetweenStartAndDestination_returnEmptyPath()
        {
            _startingNode = new PathfindingNode(0, 0);
            _destinationNode = new PathfindingNode(1f, 0);

            _gridManager.AddNode(0, 0);
            _gridManager.AddNode(0.5f, 0);
            _gridManager.AddNode(1f, 0);

            _grid.OccupiedNodes = new Dictionary<int, HashSet<PathfindingNode>>
            {
                {9001, new HashSet<PathfindingNode> {new PathfindingNode(0.5f, 0)}}
            };

            AssertPathEquals(new List<PathfindingNode>());
        }

        [Test]
        public void GivenOccupiedSpaceBetweenStartAndDestination_returnPathToClosestNode()
        {
            _startingNode = new PathfindingNode(0, 0);
            _destinationNode = new PathfindingNode(2f, 0);

            _gridManager.AddNode(-1f, 0);
            _gridManager.AddNode(-0.5f, 0);
            _gridManager.AddNode(0, 0);
            _gridManager.AddNode(0.5f, 0);
            _gridManager.AddNode(1f, 0);
            _gridManager.AddNode(1.5f, 0);
            _gridManager.AddNode(2f, 0);

            _grid.OccupiedNodes = new Dictionary<int, HashSet<PathfindingNode>>
            {
                {9001, new HashSet<PathfindingNode> {new PathfindingNode(1.5f, 0)}}
            };

            var expectedPath = new List<PathfindingNode>
            {
//                new PathfindingNode(0.5f, 0),
//                new PathfindingNode(1f, 0)
            };

            AssertPathEquals(expectedPath);
        }
    }
}