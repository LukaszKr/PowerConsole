using System.Collections.Generic;
using System.Reflection;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class CommandMethod
	{
		private List<CommandParameter> m_Parameters;

		public MethodInfo Command;
		public int ParameterCount { get { return m_Parameters.Count; } }

		private int m_NonOptionalCount = 0;

		public CommandMethod(MethodInfo methodInfo)
		{
			Command = methodInfo;
			m_Parameters = new List<CommandParameter>();
		}

		public void MapArguments(Query query)
		{
			int requiredArguments = m_NonOptionalCount;
			int realIndex = 0;
			HashSet<string> mappedValues = new HashSet<string>();
			for(int x = 0; x < query.Arguments.Count; x++)
			{
				Argument argument = query.Arguments[x];
				if(argument.Name == null)
				{
					while(m_Parameters.Count > realIndex)
					{
						CommandParameter cmdParameter = m_Parameters[realIndex];
						realIndex++;
						if(mappedValues.Contains(cmdParameter.Name))
						{
							continue;
						}
						mappedValues.Add(cmdParameter.Name);
						argument.Name = cmdParameter.Name;
						argument.Parameter = cmdParameter;
						break;
					}
					if(argument.Name == null)
					{
						throw new TooManyArgumentsException(query.Arguments.Count, m_Parameters.Count);
					}
				}
				else
				{
					argument.Parameter = FindParameter(argument.Name);
					if(argument.IsMapped)
					{
						mappedValues.Add(argument.Name);
					}
					else
					{
						throw new NamedArgumentNotFoundException(argument.Name, argument.Value);
					}
				}

				if(!argument.Parameter.HasDefault)
				{
					requiredArguments --;
				}
			}

			//add default optional values
			for(int x = 0; x < m_Parameters.Count; x++)
			{
				CommandParameter parameter = m_Parameters[x];
				if(!mappedValues.Contains(parameter.Name))
				{
					query.Arguments.Add(new Argument() 
					{ 
						Name = parameter.Name,
						Parameter = parameter
					});
				}
			}

			if(requiredArguments > 0)
			{
				List<CommandParameter> missing = new List<CommandParameter>();
				for(int x = 0; x < m_Parameters.Count; x++)
				{
					CommandParameter parameter = m_Parameters[x];
					if(!parameter.HasDefault && !mappedValues.Contains(parameter.Name))
					{
						missing.Add(parameter);
					}
				}
				throw new NotEnoughtArgumentsException(query, m_NonOptionalCount, missing);
			}
		}

		#region Parameters
		public void ClearParameters()
		{
			m_NonOptionalCount = 0;
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
				m_NonOptionalCount ++;
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

		public CommandParameter GetParameter(int index)
		{
			return m_Parameters[index];
		}
		#endregion
	}
}
