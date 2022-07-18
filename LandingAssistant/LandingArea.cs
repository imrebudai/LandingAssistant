using LandingAssistant.Enums;
using LandingAssistant.Interfaces;
using LandingAssistant.Interfaces.Validators;
using System;

namespace LandingAssistant
{
    public class LandingArea : ILandingArea
    {
        private int? _idOfLastRocketPerformingTrajectoryQuery;

        private Position? _lastlyCheckedPosition;

        private readonly Area _area;

        private readonly Position _landingPlatformPosition;

        private readonly LandingPlatform _landingPlatform;

        public LandingArea(Area area, Area landingPlatformArea, Position landingPlatformPosition, ILandingAreaValidator validator)
        {
            if (!validator.IsPlatformPositionValid(landingPlatformPosition))
            {
                throw new ArgumentOutOfRangeException("Platform position parameter(s) are out of range");
            }

            if (!validator.IsPlatformAreaInRange(area, landingPlatformArea, landingPlatformPosition))
            {
                throw new ArgumentOutOfRangeException("Platform area out of range");
            }

            _area = area;
            _landingPlatformPosition = landingPlatformPosition;
            _landingPlatform = new LandingPlatform(landingPlatformArea);

            _lastlyCheckedPosition = null;
        }

        public Trajectory QueryTrajectory(Position possibleLandingPosition, int rocketId)
        {
            var isPositionClashCheckNeeded = _lastlyCheckedPosition.HasValue
                                          && _idOfLastRocketPerformingTrajectoryQuery.HasValue
                                          && _idOfLastRocketPerformingTrajectoryQuery != rocketId;

            var arePositionsClashing = isPositionClashCheckNeeded
                ? ArePositionsClashing(_lastlyCheckedPosition.Value, possibleLandingPosition)
                : false;

            _lastlyCheckedPosition = possibleLandingPosition;
            _idOfLastRocketPerformingTrajectoryQuery = rocketId;

            if (arePositionsClashing)
            {
                return Trajectory.Clash;
            }
            else if (IsPositionInPlatformsRange(possibleLandingPosition))
            {
                return Trajectory.Ok;
            }
            else
            {
                return Trajectory.OutOfPlatform;
            }
        }

        private bool ArePositionsClashing(Position firstPosition, Position secondPosition)
        {
            var isVerticalPositionClashing = Math.Abs(firstPosition.Y - secondPosition.Y) <= 1;
            var isHorizontalPositionClashing = Math.Abs(firstPosition.X - secondPosition.X) <= 1;

            return isVerticalPositionClashing || isHorizontalPositionClashing;
        }

        private bool IsPositionInPlatformsRange(Position position)
        {
            var isVerticalPositionInPlatformRange = position.Y >= _landingPlatformPosition.Y
                                                 && position.Y < _landingPlatformPosition.Y + _landingPlatform.Height;

            var isHorizontalPositionInPlatformRange = position.X >= _landingPlatformPosition.X
                                                   && position.X < _landingPlatformPosition.X + _landingPlatform.Width;

            return isVerticalPositionInPlatformRange && isHorizontalPositionInPlatformRange;
        }
    }
}
