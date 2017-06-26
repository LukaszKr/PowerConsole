using ProceduralLevel.Common.Serialization;
using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class MacroState: AConsoleState
	{
		private List<Macro> m_Macros = new List<Macro>();

		private int m_RecordingFrom = 0;
		public Macro Recorded { get; private set; }

		public bool IsRecording { get { return Recorded != null; } }
		public readonly MacroNameHint NameHint;

		public int Count { get { return m_Macros.Count; } }

		public MacroState(ConsoleInstance console) : base(console)
		{
			NameHint = new MacroNameHint(this, m_Macros);
		}

		public override void BindEvents()
		{
		}

		public bool StartRecording(string name)
		{
			if(IsRecording)
			{
				return false;
			}
			if(!CanUseName(name))
			{
				return false;
			}
			Console.SetLocked(true);
			m_RecordingFrom = Console.HistoryState.Count;
			Recorded = new Macro(name);
			NameHint.InvalidateCache();
			return true;
		}

		public Macro Get(int index)
		{
			if(index >= 0 && index < m_Macros.Count)
			{
				return m_Macros[index];
			}
			return null;
		}

		public bool Save()
		{
			if(!IsRecording)
			{
				return false;
			}
			Console.SetLocked(false);
			for(int x = m_RecordingFrom; x < Console.HistoryState.Count-1; x++)
			{
				Recorded.Queries.Add(Console.HistoryState.Get(x));
			}
			if(!RegisterMacro(Recorded))
			{
				return false;
			}
			Recorded = null;
			Write(Console.Persistence);
			return true;
		}

		public bool RemoveMacro(string name)
		{
			int index = IndexOf(name);
			if(index >= 0)
			{
				Console.RemoveCommand(name);
				m_Macros.RemoveAt(index);
				Write(Console.Persistence);
				NameHint.InvalidateCache();
				return true;
			}
			return false;
		}

		private int IndexOf(string name)
		{
			for(int x = 0; x < m_Macros.Count; x++)
			{
				Macro macro = m_Macros[x];
				if(macro.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return x;
				}
			}
			return -1;
		}

		private bool RegisterMacro(Macro macro)
		{
			if(!CanUseName(macro.Name))
			{
				return false;
			}
			m_Macros.Add(macro);
			Console.RemoveCommand(macro.Name);
			Console.AddCommand(new MacroPlayerCommand(Console, macro));
			NameHint.InvalidateCache();
			return true;
		}

		private bool CanUseName(string name)
		{
			AConsoleCommand command = Console.FindCommand(name);
			return (command == null || (command is MacroPlayerCommand));
		}

		public void Clear()
		{
			for(int x = 0; x < m_Macros.Count; x++)
			{
				Macro macro = m_Macros[x];
				Console.RemoveCommand(macro.Name);
			}
			m_Macros.Clear();
		}

		#region Serialization
		private const string MACRO_FILE = "MacroState";

		protected override string SavePath { get { return MACRO_FILE; } }

		private const string KEY_MACROS = "macros";

		protected override void Serialize(IObjectSerializer serializer)
		{
			base.Serialize(serializer);
			
			IArraySerializer macros = serializer.WriteArray(KEY_MACROS);
			for(int x = 0; x < m_Macros.Count; x++)
			{
				macros.Write(m_Macros[x]);
			}
		}

		protected override void Deserialize(IObjectSerializer serializer)
		{
			base.Deserialize(serializer);

			Clear();

			IArraySerializer macros = serializer.TryReadArray(KEY_MACROS);
			if(macros != null)
			{
				for(int x = 0; x < macros.Count; x++)
				{
					try
					{
						IObjectSerializer rawMacro = macros.ReadObject();
						Macro macro = new Macro();
						macro.Deserialize(rawMacro);
						RegisterMacro(macro);
					}
					finally { } //skip corrupted macros
				}
			}
		}
		#endregion
	}
}
