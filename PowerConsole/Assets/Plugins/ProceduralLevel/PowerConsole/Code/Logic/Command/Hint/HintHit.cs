using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class HintHit
	{
		public readonly string Prefix;
		public readonly string HitPrefix;
		public readonly string Value;
		public readonly string HitSufix;
		public readonly string Sufix;
		public readonly string Merged;
		public readonly string Hint;

		public readonly int SufixOffset;

		public HintHit(string userInput, AHintIterator iterator, string hint)
		{
			Hint = hint;
			Argument argument = iterator.Argument;

			int hitIndex = hint.IndexOf(argument.Value, StringComparison.OrdinalIgnoreCase);
			if(hitIndex >= 0)
			{
				Value = argument.Value;
				SufixOffset = 0;
			}
			else
			{
				Value = string.Empty;
				SufixOffset = argument.Value.Length;
			}
			hitIndex = Math.Max(0, hitIndex);

			int argumentOffset = Math.Min(argument.Offset, userInput.Length);

			Prefix = userInput.Substring(0, argumentOffset);
			HitPrefix = hint.Substring(0, hitIndex);

			int hitSufixOffset = Math.Min(hint.Length, hitIndex+Value.Length);
			HitSufix = hint.Substring(hitSufixOffset);

			SufixOffset += Prefix.Length+Value.Length;
			Sufix = userInput.Substring(Math.Min(userInput.Length, SufixOffset));
			SufixOffset += HitPrefix.Length+HitSufix.Length;


			Merged = Prefix+HitPrefix+Value+HitSufix+Sufix;
		}

		public override string ToString()
		{
			return Merged;
		}
	}
}
