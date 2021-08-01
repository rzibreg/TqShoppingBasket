﻿using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace Tq.ShoppingBasket.Tests
{
    public static class LoggerTestExtensions
    {
        public static Mock<ILogger<T>> VerifyInformationLogWasCalled<T>(this Mock<ILogger<T>> logger, string expectedMessage)
        {
            Func<object, Type, bool> state = (v, t) => string.Compare(v.ToString(), expectedMessage, StringComparison.Ordinal) == 0;

            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

            return logger;
        }

        public static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, string expectedMessage, LogLevel expectedLogLevel = LogLevel.Information, Times? times = null)
        {
            times ??= Times.Once();

            Func<object, Type, bool> state = (v, t) => string.Compare(v.ToString(), expectedMessage, StringComparison.Ordinal) == 0;

            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == expectedLogLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), (Times)times);

            return logger;
        }
    }
}
