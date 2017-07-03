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

		public int SufixOffset { get; private set; }

		public HintHit(string userInput, Argument argument, string hint)
		{
			Hint = hint;

			int hitIndex = hint.IndexOf(argument.Value, StringComparison.OrdinalIgnoreCase);
			hitIndex = Math.Max(0, hitIndex);

			int argumentOffset = Math.Min(argument.Offset, userInput.Length);

			Prefix = userInput.Substring(0, argumentOffset);
			HitPrefix = hint.Substring(0, hitIndex);
			Value = argument.Value;

			int hitSufixOffset = Math.Min(hint.Length, hitIndex+Value.Length);
			HitSufix = hint.Substring(hitSufixOffset);

			SufixOffset = Math.Min(userInput.Length, Prefix.Length+HitPrefix.Length+Value.Length);
			Sufix = userInput.Substring(SufixOffset);

			Merged = Prefix+HitPrefix+Value+HitSufix+Sufix;
		}

		public override string ToString()
		{
			return Merged;
		}
	}
}
