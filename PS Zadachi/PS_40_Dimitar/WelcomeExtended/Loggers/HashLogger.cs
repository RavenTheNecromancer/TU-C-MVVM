﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WelcomeExtended.Loggers
{
	public class HashLogger : ILogger
	{
		private readonly ConcurrentDictionary<int, string> logMessages;

		private readonly string name;


        public HashLogger(string _name)
        {
            name = _name;
            logMessages = new ConcurrentDictionary<int, string>();
        }

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull
		{
			return null;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			var message = formatter(state, exception);
			switch (logLevel)
			{
				case LogLevel.Critical:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case LogLevel.Error:
					Console.ForegroundColor = ConsoleColor.DarkRed;
					break;
				case LogLevel.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				default:
					Console.ForegroundColor = ConsoleColor.White;
					break;

			}

			Console.WriteLine("- LOGGER -");
			var messageToBeLogged = new StringBuilder();
			messageToBeLogged.Append($"[{logLevel}]");
			messageToBeLogged.AppendFormat(" [{0}] ", name);
			Console.WriteLine(messageToBeLogged);
			Console.WriteLine($"{formatter(state, exception)}");
			Console.WriteLine("- LOGGER -");
			Console.ResetColor();
			logMessages[eventId.Id] = message;
		}
	}
}
