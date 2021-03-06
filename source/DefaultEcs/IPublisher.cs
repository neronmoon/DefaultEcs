﻿using System;

namespace DefaultEcs
{
    /// <summary>
    /// Encapsulates a method that has a single in parameter and does not return a value used for <see cref="World.Subscribe{T}(ActionIn{T})"/> method.
    /// </summary>
    /// <typeparam name="T">The type of message to subscribe to.</typeparam>
    /// <param name="message">The recieved message.</param>
    public delegate void ActionIn<T>(in T message);

    /// <summary>
    /// Exposes methods to subscribe to <see cref="ActionIn{T}"/> and publish message to callback those subscriptions.
    /// </summary>
    public interface IPublisher : IDisposable
    {
        /// <summary>
        /// Subscribes an <see cref="ActionIn{T}"/> to be called back when a <typeparamref name="T"/> object is published.
        /// </summary>
        /// <typeparam name="T">The type of the object to be called back with.</typeparam>
        /// <param name="action">The delegate to be called back.</param>
        /// <returns>An <see cref="IDisposable"/> object used to unsubscribe.</returns>
        IDisposable Subscribe<T>(ActionIn<T> action);

        /// <summary>
        /// Publishes a <typeparamref name="T"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the object to publish.</typeparam>
        /// <param name="message">The object to publish.</param>
        void Publish<T>(in T message);
    }
}
