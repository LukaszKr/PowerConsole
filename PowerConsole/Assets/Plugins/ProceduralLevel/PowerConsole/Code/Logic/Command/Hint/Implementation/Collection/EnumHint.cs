using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class EnumHint: ACollectionHint<Enum>
	{
		private Type m_EnumType;

		public EnumHint(Type enumType)
		{
			m_EnumType = enumType;
		}

		protected override string[] GetAllOptions()
		{
			return Enum.GetNames(m_EnumType);
		}
	}
}
