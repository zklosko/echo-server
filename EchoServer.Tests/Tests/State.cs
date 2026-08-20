using Xunit;
using EchoServer.State;

namespace EchoServer.Tests
{
    public class TestState
    {
        /// <summary>
        /// Create state instance unique to each test
        /// </summary>
        /// <returns>Blank State class</returns>
        private State.State CreateState()
        {
            return new State.State("\r", new List<Subscriber>());
        }

        [Fact]
        public void Test_AllSpacesOffInNewZone()
        {
            // Arrange - set up all things test needs
            var state = CreateState();

            // Act - perform test
            var IsOff = true;
            for (int s = 1; s <= 16; s++)
            {
                if (!state.IsSpaceOff(s))
                {
                    IsOff = false;
                    break;
                }
            }

            // Assert - verify outcome of test
            Assert.True(IsOff);
        }
    }
}
