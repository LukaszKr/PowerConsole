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

			int hitIndex = hint.IndexOf(iterator.Argument.Value, StringComparison.OrdinalIgnoreCase);
			hitIndex = Math.Max(0, hitIndex);

			int argumentOffset = Math.Min(iterator.Argument.Offset, userInput.Length);

			Prefix = userInput.Substring(0, argumentOffset);
			HitPrefix = hint.Substring(0, hitIndex);
			Value = iterator.Argument.Value;

			int hitSufixOffset = Math.Min(hint.Length, hitIndex+Value.Length);
			HitSufix = hint.Substring(hitSufixOffset);

			SufixOffset = Prefix.Length+Value.Length;
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
