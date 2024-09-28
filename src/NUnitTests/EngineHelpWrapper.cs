/*----------------------------------------------------------
Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
----------------------------------------------------------
// Codebase: https://github.com/ArKuznetsov/clientSSH/
----------------------------------------------------------*/

using System;
using System.IO;
using System.Reflection;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.HostedScript.Library;
using ScriptEngine.Machine;
using ScriptEngine.Environment;
using ScriptEngine.HostedScript;

namespace NUnitTests
{
	public class EngineHelpWrapper : IHostApplication
	{

		private HostedScriptEngine _engine;
		private string _resourceName;
		private ScriptEngine.ModuleImage _module;

		private UserScriptContextInstance _testModule;
		private ArrayImpl _testMethods;

		public EngineHelpWrapper(string resourceName)
		{
			_engine = new HostedScriptEngine();
			_engine.Initialize();

			// Тут можно указать любой класс из компоненты
			// Если проектов компонент несколько, то надо взять по классу из каждой из них
			_engine.AttachAssembly(System.Reflection.Assembly.GetAssembly(typeof(oscriptcomponent.ClientSSH)));

			// Подключаем тестовую оболочку
			_engine.AttachAssembly(System.Reflection.Assembly.GetAssembly(typeof(EngineHelpWrapper)));

			var testrunnerSource = LoadFromAssemblyResource("NUnitTests.Tests.testrunner.os");
			var testrunnerModule = _engine.GetCompilerService().Compile(testrunnerSource);
			
			{
				var mi = _engine.GetType().GetMethod("SetGlobalEnvironment",
					BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance);
				mi.Invoke(_engine, new object[] {this, testrunnerSource});
			}

			_engine.LoadUserScript(new ScriptEngine.UserAddedScript()
			{
				Type = ScriptEngine.UserAddedScriptType.Class,
				Image = testrunnerModule,
				Symbol = "TestRunner"
			});

			var testRunner = AttachedScriptsFactory.ScriptFactory("TestRunner", new IValue[] { });
			TestRunner = ValueFactory.Create(testRunner);

			_resourceName = resourceName;
			var source = LoadFromAssemblyResource(_resourceName);
			_module = _engine.GetCompilerService().Compile(source);

			_engine.LoadUserScript(new ScriptEngine.UserAddedScript()
			{
				Type = ScriptEngine.UserAddedScriptType.Class,
				Image = _module,
				Symbol = _resourceName
			});

			_testModule = AttachedScriptsFactory.ScriptFactory(_resourceName, new IValue[] { });

			int methodIndex = _testModule.FindMethod("ПолучитьСписокТестов");

			{
				IValue ivTests;
				_testModule.CallAsFunction(methodIndex, new IValue[] { TestRunner }, out ivTests);
				_testMethods = ivTests as ArrayImpl;
			}

		}

		public HostedScriptEngine Engine
		{
			get
			{
				return _engine;
			}
		}

		public ArrayImpl TestMethods
		{
			get
			{
				return _testMethods;
			}
		}

		public IValue TestRunner { get; private set; }

		public int RunTestMethod(string methodName, out string testException)
		{
			testException = "";

			int methodIndex;

			if (ValueFactory.Create() == _testMethods.Find(ValueFactory.Create(methodName)))
			{
				return -1;
			}

			try
			{
				methodIndex = _testModule.FindMethod(methodName);
			}
			catch
			{
				return -1;
			}

			try
			{
				_testModule.CallAsProcedure(methodIndex, new IValue[] { });
			}
			catch (Exception e)
			{
				testException = e.ToString();
				return 1;
			}

			return 0;
		}

		public ICodeSource LoadFromAssemblyResource(string resourceName)
		{
			var asm = Assembly.GetExecutingAssembly();
			string codeSource;

			using (Stream s = asm.GetManifestResourceStream(resourceName))
			{
				using (StreamReader r = new StreamReader(s))
				{
					codeSource = r.ReadToEnd();
				}
			}

			return _engine.Loader.FromString(codeSource);
		}

		public void Echo(string str, MessageStatusEnum status = MessageStatusEnum.Ordinary)
		{
			Console.WriteLine(str);
		}

		public string[] GetCommandLineArguments()
		{
			return new string[] { };
		}

		public bool InputString(out string result, string prompt, int maxLen, bool multiline)
		{
			result = "";
			return false;
		}

		public void ShowExceptionInfo(Exception exc)
		{
		}
	}
}
