namespace Harmony
{
    /// <summary>
    /// EventHandler sans paramètres.
    /// </summary>
    public delegate void EventHandler();

    /// <summary>
    /// EventHandler avec paramètres.
    /// </summary>
    public delegate void EventHandler<in T>(T eventParams);
}