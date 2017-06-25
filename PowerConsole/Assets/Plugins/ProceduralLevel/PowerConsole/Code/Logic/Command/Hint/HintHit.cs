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

		public HintHit(string userInput, Argument argument, string hint)
		{
			int hitIndex = hint.IndexOf(argument.Value, StringComparison.OrdinalIgnoreCase);
			hitIndex = Math.Max(0, hitIndex);

			int argumentOffset = Math.Min(argument.Offset, userInput.Length);

			Prefix = userInput.Substring(0, argumentOffset);
			HitPrefix = hint.Substring(0, hitIndex);
			Value = argument.Value;

			int hitSufixOffset = Math.Min(hint.Length, hitIndex+Value.Length);
			HitSufix = hint.Substring(hitSufixOffset);

			int sufixOffset = Math.Min(userInput.Length, userInput.Length+HitPrefix.Length+Value.Length+HitSufix.Length);
			Sufix = userInput.Substring(sufixOffset);

			Merged = Prefix+HitPrefix+Value+HitSufix+Sufix;
		}
	}
}
