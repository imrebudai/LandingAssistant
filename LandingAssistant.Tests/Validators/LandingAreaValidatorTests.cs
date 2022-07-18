using LandingAssistant.Validators;
using Xunit;

namespace LandingAssistant.Tests.Validators
{
    public class LandingAreaValidatorTests
    {
        private readonly LandingAreaValidator _validator;

        public LandingAreaValidatorTests()
        {
            _validator = new LandingAreaValidator();
        }

        [Theory]
        [InlineData(5, 5, true)]
        [InlineData(-1, -1, false)]
        [InlineData(5, 6, false)]
        public void IsPlatformPositionValid_ShouldReturnExpectedResult(int positionX, int positionY, bool expectedResult)
        {
            // Arrange         
            var landingPlatformPosition = new Position(positionX, positionY);

            // Act
            var result = _validator.IsPlatformPositionValid(landingPlatformPosition);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        // TODO: to simplify/reduce method parameters a test data class could be defined and used with the ClassData attribute
        [Theory]
        [InlineData(100, 100, 100, 100, 0, 0, true)]
        [InlineData(100, 100, 101, 100, 0, 0, false)]
        [InlineData(100, 100, 100, 101, 0, 0, false)]
        [InlineData(100, 100, 100, 100, 1, 0, false)]
        [InlineData(100, 100, 100, 100, 0, 1, false)]
        public void IsPlatformAreaInRange_ShouldReturnExpectedResult(
                int landingAreaHeight,
                int landingAreaWidth,
                int platformAreaHeight,
                int platformAreaWidth,
                int landingPositionX,
                int landingPositionY,
                bool expectedResult
            )
        {
            // Arrange
            var landingArea = new Area(landingAreaHeight, landingAreaWidth);
            var landingPlatformArea = new Area(platformAreaHeight, platformAreaWidth);
            var landingPlatformPosition = new Position(landingPositionX, landingPositionY);

            // Act
            var result = _validator.IsPlatformAreaInRange(landingArea, landingPlatformArea, landingPlatformPosition);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
