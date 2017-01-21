using UnityStandardAssets.CrossPlatformInput;

public class TriggerButton
{
    private readonly string _triggerName;
    private bool _wasPressedLastFrame;

    public TriggerButton(string triggerName)
    {
        _triggerName = triggerName;
    }

    public bool IsPressed()
    {
        var isCurrentlyPressing = CrossPlatformInputManager.GetAxis(_triggerName) > 0;
        var isPressed = !_wasPressedLastFrame && isCurrentlyPressing;

        _wasPressedLastFrame = isCurrentlyPressing;
        return isPressed;
    }
}