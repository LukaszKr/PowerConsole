using UnityEngine.UI;

namespace ProceduralLevel.PowerConsole.View
{
	public class DefaultMessageView: AConsoleMessageView
	{
		public Text Text;

		public override float Height { get { return Text.preferredHeight; } }

		protected override void RefreshView()
		{
			Text.text = m_Message.ToString();
		}
	}
}
