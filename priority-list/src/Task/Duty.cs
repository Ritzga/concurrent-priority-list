using System.Numerics;

namespace World.Tasks
{
    public class Duty
    {
        public Duty(Vector2 worldPosition, DutyType dutyType, int difficult, bool custom = false)
        {
            this.worldPosition = worldPosition;
            this.difficult = difficult;
            this.dutyType = dutyType;
            Custom = custom;
        }

        public Vector2 worldPosition;
        public int difficult;
        public readonly DutyType dutyType;

        public int Priority { get; set; } = 0;
        public bool Used { get; set; } = false;

        public bool Custom { get; set; } = false;

        //just random names for duties to categorize 
        public enum DutyType
        {
            DIGGING,
            CLEANING,
            REPAIRING,
            PROTECTING,
            BUILDING,
            COLLECTING,
            COLLECTING_ORES,
            COLLECTING_CRYSTALS,
            EXPLORING,
            PERFORM_JOBS
        }
    }
}