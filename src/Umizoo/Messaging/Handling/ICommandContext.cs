﻿// Copyright © 2015 ~ 2017 Sunsoft Studio, All rights reserved.
// Umizoo is a framework can help you develop DDD and CQRS style applications.
// 
// Created by young.han with Visual Studio 2017 on 2017-08-09.

using System;
using Umizoo.Seeds;

namespace Umizoo.Messaging.Handling
{
    /// <summary>
    ///     表示继承该接口的是一个命令上下文
    /// </summary>
    public interface ICommandContext
    {
        /// <summary>
        ///     命令标识
        /// </summary>
        string CommandId { get; }

        /// <summary>
        ///     添加该聚合根到当前上下文中。
        /// </summary>
        void Add(IAggregateRoot aggregateRoot);

        /// <summary>
        ///     从当前上下文获取聚合根，如果不存在，则可能从缓存中缓存，缓存中没有的话则从数据库中获取。
        ///     如果还不存在的话则抛出异常。
        /// </summary>
        /// <exception cref="EntityNotFoundException" />
        TEventSourced Get<TEventSourced, TIdentify>(TIdentify id) where TEventSourced : class, IEventSourced;

        /// <summary>
        ///     从当前上下文获取聚合根，如果不存在，则可能从缓存中缓存，缓存中没有的话则从数据库中获取。
        ///     如果还不存在的话则返回一个空的引用。
        /// </summary>
        TAggregateRoot Find<TAggregateRoot, TIdentify>(TIdentify id) where TAggregateRoot : class, IAggregateRoot;


        void SetResult(HandleStatus status, string errorMessage = null, string result = null, string resultType = null);
    }
}