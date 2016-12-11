using System.Collections.Generic;
using AI.Pathfinding;
using NUnit.Framework;

namespace Test.AI.Pathfinding
{
    public class PathfindingNodeTest
    {
        [Test]
        public void givenTwoNodesThatAreNotEqual_assertNotEqual()
        {
            PathfindingNode one = new PathfindingNode(null, 1.0f, 1.0f);
            PathfindingNode two = new PathfindingNode(null, 1.0f, 2.0f);
            Assert.False(one.Equals(two));
        }

        [Test]
        public void givenTwoNodesThatAreEqual_assertEqual()
        {
            PathfindingNode one = new PathfindingNode(null, 1.0f, 1.0f);
            PathfindingNode two = new PathfindingNode(null, 1.0f, 1.0f);
            Assert.True(one.Equals(two));

        }

    }
}