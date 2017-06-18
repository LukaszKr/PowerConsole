using NUnit.Framework;
using ProceduralLevel.GameConsole.Logic;
using System.Collections.Generic;

namespace ProceduralLevel.GameConsole.Editor.Test
{
	public class QueryParserTest
	{
		private QueryParser m_Parser = new QueryParser();

		[Test]
		public void NoParameter()
		{
			m_Parser.Parse("test");
			List<Query> queries = m_Parser.Flush();

			Assert.AreEqual(1, queries.Count);
			Query query = queries[0];
			Assert.AreEqual(1, query.Params.Count);
			TestHelper.CheckParam(query.Params[0], null, "test");
		}

		[Test]
		public void MultipleSpaces()
		{
			m_Parser.Parse("test   param");
			List<Query> queries = m_Parser.Flush();

			Assert.AreEqual(1, queries.Count);
			Query query = queries[0];
			Assert.AreEqual(2, query.Params.Count);
			TestHelper.CheckParam(query.Params[0], null, "test");
			TestHelper.CheckParam(query.Params[1], null, "param");
		}

		[Test]
		public void MultipleCommands()
		{
			m_Parser.Parse("test; test1 ; test3 param;");
			List<Query> queries = m_Parser.Flush();

			Assert.AreEqual(3, queries.Count);
			Query query;

			query = queries[0];
			Assert.AreEqual(1, query.Params.Count);
			TestHelper.CheckParam(query.Params[0], null, "test");

			query = queries[1];
			Assert.AreEqual(1, query.Params.Count);
			TestHelper.CheckParam(query.Params[0], null, "test1");

			query = queries[2];
			Assert.AreEqual(2, query.Params.Count);
			TestHelper.CheckParam(query.Params[0], null, "test3");
			TestHelper.CheckParam(query.Params[1], null, "param");

		}

		[Test]
		public void NamedParameter()
		{
			m_Parser.Parse("test arg=value");
			List<Query> queries = m_Parser.Flush();

			Assert.AreEqual(1, queries.Count);
			Query query = queries[0];
			Assert.AreEqual(2, query.Params.Count);
			TestHelper.CheckParam(query.Params[0], null, "test");
			TestHelper.CheckParam(query.Params[1], "arg", "value");
		}
	}
}

