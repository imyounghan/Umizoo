﻿// Copyright © 2015 ~ 2017 Sunsoft Studio, All rights reserved.
// Umizoo is a framework can help you develop DDD and CQRS style applications.
// 
// Created by young.han with Visual Studio 2017 on 2017-08-07.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Umizoo.Infrastructure.Composition.Interception
{
    public interface IMethodInvocation
    {
        /// <summary>
        /// 获取所有的参数集合
        /// </summary>
        IParameterCollection Arguments { get; }
        /// <summary>
        /// 获取输入的参数集合
        /// </summary>
        IParameterCollection Inputs { get; }
        /// <summary>
        /// 当前上下文数据
        /// </summary>
        IDictionary<string, object> InvocationContext { get; }
        /// <summary>
        /// 调用方法的信息
        /// </summary>
        MethodBase MethodBase { get; }
        /// <summary>
        /// 调用方法所在的实例
        /// </summary>
        object Target { get; }

        /// <summary>
        /// 创建一个带有异常信息的结果
        /// </summary>
        IMethodReturn CreateExceptionMethodReturn(Exception ex);
        /// <summary>
        /// 创建一个正确返回的结果
        /// </summary>
        IMethodReturn CreateMethodReturn(object returnValue, params object[] outputs);
    }
}