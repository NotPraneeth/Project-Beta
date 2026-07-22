public class InputButton
{
    private bool pressed;

    public bool IsPressed => pressed;

    public void Press() => pressed = true;

    public void Release() => pressed = false;

    public bool Consume()
    {
        if (!pressed)
            return false;

        pressed = false;
        return true;
    }
}