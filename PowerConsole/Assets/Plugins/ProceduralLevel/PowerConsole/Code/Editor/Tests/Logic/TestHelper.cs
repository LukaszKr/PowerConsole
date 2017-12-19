using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;
using ProceduralLevel.Tokenize;
using System;

namespace ProceduralLevel.PowerConsole.Editor.Test
{
    public static class TestHelper
	{
		public static void CheckQuery(Query query, string name, int argCount, bool isOption = false)
		{
			Assert.AreEqual(name, query.Name.Value);
			Assert.AreEqual(argCount, query.Arguments.Count);
			Assert.AreEqual(isOption, query.IsOption);
		}

		public static void CheckArgument(Argument param, string name, string value)
		{
			Assert.AreEqual(name, param.Name);
			Assert.AreEqual(value, param.Value);
		}

		public static void CheckParameter(CommandParameter param, string name, Type type, object defaultValue)
		{
			Assert.AreEqual(name, param.Name);
			Assert.AreEqual(type, param.Type);
			Assert.AreEqual(defaultValue, param.DefaultValue);
		}

		public static void CheckToken(Token token, bool isSeparator, string value)
		{
			Assert.AreEqual(isSeparator, token.IsSeparator);
			Assert.AreEqual(value, token.Value);
		}

		public static void CheckCommand(ConsoleInstance console, Message expectedResult, string query)
		{
			Message[] expected = new Message[]
			{
				new Message(EMessageType.Execution, query),
				expectedResult
			};

			int current = 0;
			Action<Message> onMessage = (Message message) =>
			{
				CheckMessage(message, expected[current].Type, expected[current].Value);
				current += 1;
			};
			console.OnMessage.RemoveAllListeners();
			console.OnMessage.AddListener(onMessage);
			console.Execute(query);
			console.OnMessage.RemoveListener(onMessage);
			Assert.AreEqual(expected.Length, current);
		}

		public static void CheckMessage(Message message, EMessageType result, string value)
		{
			Assert.AreEqual(result, message.Type);
			Assert.AreEqual(value, message.Value);
		}

		public static void CheckException(QueryParser parser, string query, EQueryError errorType)
		{
			try
			{
				parser.Parse(query);
				parser.Flush();
				ExpectException(errorType.ToString());
			}
			catch(QueryParserException e)
			{
				Assert.AreEqual(errorType, e.ErrorCode);
			}
		}

		public static void ExpectException<T>()
		{
			ExpectException(typeof(T).Name);
		}

		public static void ExpectException(string name)
		{
			Assert.Fail(string.Format("Exception of type {0} wasn't caught.", name));
		}
}
}
