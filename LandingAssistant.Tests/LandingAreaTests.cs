using LandingAssistant.Enums;
using LandingAssistant.Interfaces.Validators;
using NSubstitute;
using System;
using Xunit;

namespace LandingAssistant.Tests
{
    public class LandingAreaTests
    {
        private readonly Area _landingArea;
        private readonly Area _landingPlatformArea;
        private readonly Position _landingPlatformPosition;
        private readonly ILandingAreaValidator _landingAreaValidator;
        public LandingAreaTests()
        {
            _landingArea = new Area(100, 100);
            _landingPlatformArea = new Area(10, 10);
            _landingPlatformPosition = new Position(5, 5);
            _landingAreaValidator = Substitute.For<ILandingAreaValidator>();
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void Instantiation_ShouldThrowArgumentOutOfRangeException_WhenParametersAreNotValid(bool isPlatformPositionValid, bool isPlatformAreaInRange)
        {
            // Arrange
            _landingAreaValidator.IsPlatformPositionValid(Arg.Any<Position>()).Returns(isPlatformPositionValid);
            _landingAreaValidator.IsPlatformAreaInRange(Arg.Any<Area>(), Arg.Any<Area>(), Arg.Any<Position>()).Returns(isPlatformAreaInRange);

            // Act
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new LandingArea(_landingArea, _landingPlatformArea, _landingPlatformPosition, _landingAreaValidator));

            // Assert
            Assert.NotNull(exception);
        }

        [Theory]
        [InlineData(5, 5, 5, 6)]
        [InlineData(5, 5, 6, 5)]
        [InlineData(5, 5, 6, 7)]
        public void QueryTrajectory_ShouldReturnClash_WhenPreviouslyAndActuallyQueriedPositionsWithDifferingRocketIdsAreTooClose(int prevPosX, int prevPosY, int actualPosX, int actualPosY)
        {
            // Arrange
            _landingAreaValidator.IsPlatformPositionValid(Arg.Any<Position>()).Returns(true);
            _landingAreaValidator.IsPlatformAreaInRange(Arg.Any<Area>(), Arg.Any<Area>(), Arg.Any<Position>()).Returns(true);

            var landingAreaValidator = new LandingArea(_landingArea, _landingPlatformArea, _landingPlatformPosition, _landingAreaValidator);

            var previousRocketId = 0;
            var actualRocketId = 1;

            var previouslyQueriedPosition = new Position(prevPosX, prevPosY);
            var actuallyQueriedPosition = new Position(actualPosX, actualPosY);

            // Act
            var _ = landingAreaValidator.QueryTrajectory(previouslyQueriedPosition, previousRocketId);
            var result = landingAreaValidator.QueryTrajectory(actuallyQueriedPosition, actualRocketId);

            // Assert
            Assert.Equal(Trajectory.Clash, result);
        }

        [Fact]
        public void QueryTrajectory_ShouldReturnOk_WhenPreviouslyAndActuallyQueriedPositionsWithSameRocketIdsAreTooClose()
        {
            // Arrange
            _landingAreaValidator.IsPlatformPositionValid(Arg.Any<Position>()).Returns(true);
            _landingAreaValidator.IsPlatformAreaInRange(Arg.Any<Area>(), Arg.Any<Area>(), Arg.Any<Position>()).Returns(true);

            var landingAreaValidator = new LandingArea(_landingArea, _landingPlatformArea, _landingPlatformPosition, _landingAreaValidator);

            var previousRocketId = 1;
            var actualRocketId = 1;

            var previouslyQueriedPosition = new Position(_landingPlatformPosition.X, _landingPlatformPosition.Y);
            var actuallyQueriedPosition = new Position(_landingPlatformPosition.X, _landingPlatformPosition.Y);

            // Act
            var _ = landingAreaValidator.QueryTrajectory(previouslyQueriedPosition, previousRocketId);
            var result = landingAreaValidator.QueryTrajectory(actuallyQueriedPosition, actualRocketId);

            // Assert
            Assert.Equal(Trajectory.Ok, result);
        }

        [Fact]
        public void QueryTrajectory_ShouldReturnOk_WhenPositionIsInPlatformsRange()
        {
            // Arrange
            _landingAreaValidator.IsPlatformPositionValid(Arg.Any<Position>()).Returns(true);
            _landingAreaValidator.IsPlatformAreaInRange(Arg.Any<Area>(), Arg.Any<Area>(), Arg.Any<Position>()).Returns(true);

            var landingAreaValidator = new LandingArea(_landingArea, _landingPlatformArea, _landingPlatformPosition, _landingAreaValidator);

            var rocketId = 1;
            var queriedPosition = new Position(_landingPlatformPosition.X, _landingPlatformPosition.Y);

            // Act
            var result = landingAreaValidator.QueryTrajectory(queriedPosition, rocketId);

            // Assert
            Assert.Equal(Trajectory.Ok, result);
        }

        [Fact]
        public void QueryTrajectory_ShouldReturnOutOfPlatform_WhenPositionIsOutOfPlatformsRange()
        {
            // Arrange
            _landingAreaValidator.IsPlatformPositionValid(Arg.Any<Position>()).Returns(true);
            _landingAreaValidator.IsPlatformAreaInRange(Arg.Any<Area>(), Arg.Any<Area>(), Arg.Any<Position>()).Returns(true);

            var landingAreaValidator = new LandingArea(_landingArea, _landingPlatformArea, _landingPlatformPosition, _landingAreaValidator);

            var rocketId = 1;
            var queriedPosition = new Position(_landingPlatformPosition.X-1, _landingPlatformPosition.Y-1);

            // Act
            var result = landingAreaValidator.QueryTrajectory(queriedPosition, rocketId);

            // Assert
            Assert.Equal(Trajectory.OutOfPlatform, result);
        }
    }
}
