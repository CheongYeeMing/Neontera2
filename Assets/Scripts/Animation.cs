/// <summary>
/// This interface controls the Animation of all GameObjects.
/// </summary>
public interface Animation
{   
    /// <summary>
    /// This method is called when GameObject is initialised.
    /// </summary>
    public void Start();

    /// <summary>
    /// This method has to be implemented by every GameObject
    /// that has some form of animation.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeAnimationState(string newState);
}
