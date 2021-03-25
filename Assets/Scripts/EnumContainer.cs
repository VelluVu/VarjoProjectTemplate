/// <summary>
/// @Author: Veli-Matti Vuoti
/// Collection of enums which used in the project
/// </summary>

/// <summary>
/// Used to check the matching hand or device,
/// can use Inputdevicecharacteristics instead.
/// </summary>
public enum PreferredHand
{
    Left,
    Right,
    Hmd,
}

/// <summary>
/// Enum for the implemented movement types
/// </summary>
public enum MovementType
{
    Gaze,
    POIGaze,
    Lerp,
    Teleport,
}