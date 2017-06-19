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
			int realIndex = 0;
			HashSet<string> mappedValues = new HashSet<string>();
			for(int x = 0; x < query.Arguments.Count; x++)
			{
				Argument argument = query.Arguments[x];
				if(argument.Name == null)
				{
					while(m_Parameters.Count < realIndex)
					{
						CommandParameter cmdParameter = m_Parameters[realIndex];
						realIndex++;
						if(mappedValues.Contains(cmdParameter.Name))
						{
							continue;
						}
						mappedValues.Add(cmdParameter.Name);
						argument.Name = cmdParameter.Name;
						break;
					}
					if(argument.Name == null)
					{

					}
				}
			}
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

		private CommandParameter FindParameter(string name)
		{
			for(int x = 0; x < m_Parameters.Count; x++)
			{
				CommandParameter parameter = m_Parameters[x];
				if(name == parameter.Name)
				{
					return parameter;
				}
			}
			return null;
		}
	}
}
