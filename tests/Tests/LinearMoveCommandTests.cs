using FluentAssertions;
using PrusaMiniBowdenCompensation.Processor;

namespace Tests
{
    [TestClass]
    public class LinearMoveCommandTests
    {
        [TestMethod]
        public void NumberFormattingTest()
        {
            LinearMoveCommand e(double d) =>
                new(IsRapid: false,
                    X: null,
                    Y: null,
                    Z: null,
                    E: d,
                    F: null);

            // current formatting is suboptimal, but it works
            // standard numeric formats do not offer exactly what we need

            e(0.0000005).ToString().Should().BeEquivalentTo("G1 E0.000000");
            e(0.000005).ToString().Should().BeEquivalentTo("G1 E0.000005");
            e(0.00005).ToString().Should().BeEquivalentTo("G1 E0.000050");
            e(1).ToString().Should().BeEquivalentTo("G1 E1.000000");
            e(0.1).ToString().Should().BeEquivalentTo("G1 E0.100000");
            e(-0.1).ToString().Should().BeEquivalentTo("G1 E-0.100000");
            e(123.456).ToString().Should().BeEquivalentTo("G1 E123.456000");
            e(1234567).ToString().Should().BeEquivalentTo("G1 E1234567.000000");
        }
    }
}