using JetBrains.Annotations;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Signature de toute fonction désirant être recevoir un un évènement publié sur un <see cref="EventChannel{T}"/>.
    /// </summary>
    public delegate void EventChannelHandler<in T>([NotNull] T newEvent) where T : IEvent;

    /// <summary>
    /// Représente un EventChannel Unity.
    /// </summary>
    /// <typeparam name="T">Type d'événement circulant sur le canal.</typeparam>
    /// <inheritdoc />
    public abstract class EventChannel<T> : MonoBehaviour where T : IEvent
    {
        /// <summary>
        /// Évènement déclanché lorsqu'un <see cref="IEvent"/> est publié sur ce canal.
        /// </summary>
        public virtual event EventChannelHandler<T> OnEventPublished;

        /// <summary>
        /// Publie un évènement sur le canal. Tout ceux enregistré auprès du canal recevront cet événement.
        /// </summary>
        /// <param name="newEvent"><see cref="IEvent"/> à publier sur le canal.</param>
        public virtual void Publish(T newEvent)
        {
            // ReSharper disable once PolymorphicFieldLikeEventInvocation
            if (OnEventPublished != null) OnEventPublished(newEvent);
        }
    }
}