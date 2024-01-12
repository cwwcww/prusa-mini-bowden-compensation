using FluentAssertions;
using PrusaMiniBowdenCompensation.Processor;

namespace Tests
{
    [TestClass]
    public class BowdenCompensationTests
    {
        [TestMethod]
        public void BasicTest()
        {
            BowdenCompensation.GetAdditionalExtruderMoveLength(180, 1, 1).Should().Be(1);
            BowdenCompensation.GetAdditionalExtruderMoveLength(-180, 1, 1).Should().Be(-1);
            BowdenCompensation.GetAdditionalExtruderMoveLength(90, 0.2, 0.3).Should().Be(0.1);
            BowdenCompensation.GetAdditionalExtruderMoveLength(-90, 0.2, 0.3).Should().Be(-0.15);
        }
    }
}