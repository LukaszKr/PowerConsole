using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AGenericHint<HintType>: AHint
	{
		public override Type HintedType { get { return typeof(HintType); } }
	}
}
