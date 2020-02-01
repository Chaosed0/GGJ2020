public class AnimationStateAttribute : System.Attribute
{
    public string animatorProperty = null;

    public AnimationStateAttribute(string animatorProperty = null)
    {
        this.animatorProperty = animatorProperty;
    }
}
