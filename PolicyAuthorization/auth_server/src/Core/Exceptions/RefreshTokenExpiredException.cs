﻿using System;
using System.Runtime.Serialization;

namespace Core.Exceptions
{
    public class RefreshTokenExpiredException : Exception
    {
        public RefreshTokenExpiredException()
        {
        }

        public RefreshTokenExpiredException(string message) : base(message)
        {
        }

        public RefreshTokenExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RefreshTokenExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
