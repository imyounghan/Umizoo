﻿// Copyright © 2015 ~ 2017 Sunsoft Studio, All rights reserved.
// Umizoo is a framework can help you develop DDD and CQRS style applications.
// 
// Created by young.han with Visual Studio 2017 on 2017-08-07.

using System;

namespace Umizoo.Infrastructure.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(string name);

        ILogger CreateLogger(Type type);
    }
}