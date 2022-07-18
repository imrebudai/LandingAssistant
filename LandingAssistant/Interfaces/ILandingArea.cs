using LandingAssistant.Enums;

namespace LandingAssistant.Interfaces
{
    public interface ILandingArea
    {
        public Trajectory QueryTrajectory(Position possibleLandingPosition, int rocketId);
    }
}
