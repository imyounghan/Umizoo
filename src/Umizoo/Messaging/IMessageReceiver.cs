﻿// Copyright © 2015 ~ 2017 Sunsoft Studio, All rights reserved.
// Umizoo is a framework can help you develop DDD and CQRS style applications.
// 
// Created by young.han with Visual Studio 2017 on 2017-08-08.

using System;

namespace Umizoo.Messaging
{
    /// <summary>
    ///     表示
    /// </summary>
    public interface IMessageReceiver<TEventArgs> where TEventArgs : EventArgs
    {
        event EventHandler<TEventArgs> MessageReceived;


        /// <summary>
        ///     Starts the listener.
        /// </summary>
        void Start();

        /// <summary>
        ///     Stops the listener.
        /// </summary>
        void Stop();
    }
}