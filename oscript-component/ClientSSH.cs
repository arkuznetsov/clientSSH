using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace oscriptcomponent
{
	/// <summary>
	/// Некоторый класс
	/// </summary>
	[ContextClass("КлиентSSH", "ClientSSH")]
	public class ClientSSH : AutoContext<ClientSSH>
	{
		public ClientSSH()
		{
		}

		/// <summary>
		/// Некоторое свойство только для чтения.
		/// </summary>
		[ContextProperty("СвойствоДляЧтения", "ReadonlyProperty")]
		public string ReadonlyProperty
		{
			get
			{
				return "MyValue";
			}
		}

		/// <summary>
		/// Некоторый конструктор
		/// </summary>
		/// <returns></returns>
		[ScriptConstructor]
		public static IRuntimeContextInstance Constructor()
		{
			return new ClientSSH();
		}
	}
}

