using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic.Queries
{
    [Category("PowerConsole")]
    public class QueryParserTest
	{
		private QueryParser m_Parser = new QueryParser();

		[Test]
		public void WhiteSpaceOnly()
		{
			m_Parser.Parse(" ");
			List<Query> queries = m_Parser.Flush();

			Assert.AreEqual(1, queries.Count);
			TestHelper.CheckQuery(queries[0], "", 0);
		}

		[Test]
		public void NoParameter()
		{
			m_Parser.Parse("test");
			List<Query> queries = m_Parser.Flush();

			Assert.AreEqual(1, queries.Count);
			Query query = queries[0];
			TestHelper.CheckQuery(query, "test", 0);
		}

		[Test]
		public void SpaceAtTheEnd()
		{
			m_Parser.Parse("test ");
			Query query = m_Parser.Flush()[0];

			Assert.AreEqual(1, query.Arguments.Count);
		}

		[Test]
		public void RawQuery()
		{
			string expectedQuery = "test 123 ";
			string rawQuery = expectedQuery+"; yup";
			m_Parser.Parse(rawQuery);
			List<Query> queries = m_Parser.Flush();
			Assert.AreEqual(expectedQuery, queries[0].RawQuery);
		}

		[Test]
		public void MultipleSpaces()
		{
			m_Parser.Parse("test   param");
			List<Query> queries = m_Parser.Flush();

			Assert.AreEqual(1, queries.Count);
			Query query = queries[0];
			TestHelper.CheckQuery(query, "test", 3);
			TestHelper.CheckArgument(query.Arguments[2], null, "param");
		}

		[Test]
		public void MultipleCommands()
		{
			m_Parser.Parse("test; test1 ; test3 param;");
			List<Query> queries = m_Parser.Flush();

			Assert.AreEqual(3, queries.Count);
			Query query;

			query = queries[0];
			TestHelper.CheckQuery(query, "test", 0);

			query = queries[1];
			TestHelper.CheckQuery(query, "test1", 0);

			query = queries[2];
			TestHelper.CheckQuery(query, "test3", 1);
			TestHelper.CheckArgument(query.Arguments[0], null, "param");
			Assert.AreEqual(20, query.Arguments[0].Offset);
		}

		[Test]
		public void NamedParameter()
		{
			m_Parser.Parse("test arg=value");
			List<Query> queries = m_Parser.Flush();

			Assert.AreEqual(1, queries.Count);
			Query query = queries[0];
			TestHelper.CheckQuery(query, "test", 1);
			TestHelper.CheckArgument(query.Arguments[0], "arg", "value");
		}

		[Test]
		public void RepeatOption()
		{
			m_Parser.Parse("test -repeat 2");
			List<Query> queries = m_Parser.Flush();

			Assert.AreEqual(2, queries.Count);
			Query query = queries[0];
			TestHelper.CheckQuery(query, "test", 0);
			query = queries[1];
			TestHelper.CheckQuery(query, "repeat", 1, true);
			TestHelper.CheckArgument(query.Arguments[0], null, "2");
		}
	}
}
