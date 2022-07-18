namespace LandingAssistant.Interfaces.Validators
{
    public interface ILandingAreaValidator
    {
        public bool IsPlatformPositionValid(Position landingPlatformPosition);

        public bool IsPlatformAreaInRange(Area landingArea, Area landingPlatformArea, Position landingPlatformPosition);
    }
}
