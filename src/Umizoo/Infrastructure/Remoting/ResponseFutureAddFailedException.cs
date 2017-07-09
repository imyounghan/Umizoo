﻿using System;
using System.Net;

namespace Umizoo.Infrastructure.Remoting
{
    public class ResponseFutureAddFailedException : Exception
    {
        public ResponseFutureAddFailedException(long requestSequence)
            : base(string.Format("Add remoting request response future failed. request sequence:{0}", requestSequence))
        {
        }
    }
}