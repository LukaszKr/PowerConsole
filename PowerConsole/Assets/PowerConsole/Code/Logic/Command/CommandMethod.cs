using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class CommandMethod
	{
		private List<CommandParameter> m_Parameters;

		public int ParameterCount { get { return m_Parameters.Count; } }

		public CommandMethod()
		{
			m_Parameters = new List<CommandParameter>();
		}

		public void Parse(ValueParser parser, Query query)
		{

		}

		public void MapParameters(Query query)
		{
			
		}

		public void ClearParameters()
		{
			m_Parameters.Clear();
		}

		public void AddParameter(CommandParameter parameter)
		{
			if(!parameter.HasDefault)
			{
				for(int x = 0; x < m_Parameters.Count; x++)
				{
					CommandParameter existingParameter = m_Parameters[x];
					if(existingParameter.HasDefault)
					{
						throw new OptionalParameterOrderException(this, existingParameter, parameter);
					}
				}
			}
			m_Parameters.Add(parameter);
		}

		public List<CommandParameter> CopyParameters()
		{
			return new List<CommandParameter>(m_Parameters);
		}

		public CommandParameter FindParameter(string name)
		{
			for(int x = 0; x < m_Parameters.Count; x++)
			{
				CommandParameter parameter = m_Parameters[x];
				if(parameter.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return parameter;
				}
			}
			return null;
		}
	}
}
