using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleHintPanel: AConsolePanel
	{
		public ConsoleHintPanel(ConsoleView consoleView) : base(consoleView)
		{
		}

		public override float PreferedHeight(float availableHeight)
		{
			return Styles.InputHeight*2;
		}

		protected override void OnRender(Vector2 size)
		{
		}

		protected override void OnSizeChanged(Vector2 size)
		{
		}
	}
}
