using System.Collections;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AHint: IEnumerable
	{
		public abstract string[] GetHints();

		public IEnumerator GetEnumerator()
		{
			return new HintEnumerator(GetHints());
		}
	}
}
