
namespace RedApple.SubwaySurfer
{
    public enum BlockType
    {
        City1,
        City2,
        City3,
        Crossing1,
        Crossing2,
        Urban1,
        Urban2,
        Bridge1,
        Bridge2,
        Tunnel
    }

    public enum LaneDirection
    {
        forward,
        left,
        right
    }

    public enum TransformProperty
    {
        count,
        position,
        rotation
    }

    public enum Lanes
    {
        Left,
        Middle,
        Right,
    }

    public enum VehicleType
    {
        VehicleSet1,
        VehicleSet2,
        VehicleSet3,
        VehicleSet4,
        VehicleSet5,
        VehicleSet6,
        VehicleSet7,
        VehicleSet8,
        VehicleSet9
    }

    public enum PoolObjectType
    {
        City1,
        City2,
        City3,
        Crossing1,
        Crossing2,
        Urban1,
        Urban2,
        Bridge1,
        Bridge2,
        Tunnel,
        Slide,
        Jump,
        Car,
        LeftTurn,
        RightTurn,
    }

    public enum TypeOfObstacle
    {
        StaticObs,
        MoveableObs,
        BlankObs,
        ClimbableObs
    }
}