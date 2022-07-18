using LandingAssistant.Interfaces.Validators;

namespace LandingAssistant.Validators
{
    public class LandingAreaValidator : ILandingAreaValidator
    {
        public bool IsPlatformPositionValid(Position landingPlatformPosition)
        {
            if (
                    landingPlatformPosition.X < 0 ||
                    landingPlatformPosition.Y < 0 ||
                    landingPlatformPosition.X != landingPlatformPosition.Y
               )
            {
                return false;
            }

            return true;
        }

        public bool IsPlatformAreaInRange(Area landingArea, Area landingPlatformArea, Position landingPlatformPosition)
        {
            var landingPlatformMaxHeight = landingArea.Height - landingPlatformPosition.Y;
            var landingPlatformMaxWidth = landingArea.Width - landingPlatformPosition.X;

            var isPlatformAreaHeightInRange = landingPlatformArea.Height <= landingPlatformMaxHeight;
            var isPlatformAreaWidthInRange = landingPlatformArea.Width <= landingPlatformMaxWidth;

            var isPlatformAreaInRange = isPlatformAreaHeightInRange && isPlatformAreaWidthInRange;

            return isPlatformAreaInRange;
        }
    }
}
