/*----------------------------------------------------------
Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
----------------------------------------------------------
// Codebase: https://github.com/ArKuznetsov/oscript-ssh/
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

		private HostedScriptEngine engine;

		public EngineHelpWrapper()
		{
		}

		public HostedScriptEngine Engine
		{
			get
			{
				return engine;
			}
		}

		public IValue TestRunner { get; private set; }

		public HostedScriptEngine StartEngine()
		{
			engine = new HostedScriptEngine();
			engine.Initialize();

			// Тут можно указать любой класс из компоненты
			engine.AttachAssembly(System.Reflection.Assembly.GetAssembly(typeof(oscriptcomponent.ClientSsh)));

			// Если проектов компонент несколько, то надо взять по классу из каждой из них
			// engine.AttachAssembly(System.Reflection.Assembly.GetAssembly(typeof(oscriptcomponent_2.MyClass_2)));
			// engine.AttachAssembly(System.Reflection.Assembly.GetAssembly(typeof(oscriptcomponent_3.MyClass_3)));

			// Подключаем тестовую оболочку
			engine.AttachAssembly(System.Reflection.Assembly.GetAssembly(typeof(EngineHelpWrapper)));

			var testrunnerSource = LoadFromAssemblyResource("NUnitTests.Tests.testrunner.os");
			var testrunnerModule = engine.GetCompilerService().Compile(testrunnerSource);
			
			{
				var mi = engine.GetType().GetMethod("SetGlobalEnvironment",
					BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance);
				mi.Invoke(engine, new object[] {this, testrunnerSource});
			}

			engine.LoadUserScript(new ScriptEngine.UserAddedScript()
			{
				Type = ScriptEngine.UserAddedScriptType.Class,
				Image = testrunnerModule,
				Symbol = "TestRunner"
			});

			var testRunner = AttachedScriptsFactory.ScriptFactory("TestRunner", new IValue[] { });
			TestRunner = ValueFactory.Create(testRunner);

			return engine;
		}

        [Obsolete]
        public void RunTestScript(string resourceName)
		{
			var source = LoadFromAssemblyResource(resourceName);
			var module = engine.GetCompilerService().Compile(source);

			engine.LoadUserScript(new ScriptEngine.UserAddedScript()
			{
				Type = ScriptEngine.UserAddedScriptType.Class,
				Image = module,
				Symbol = resourceName
			});

			var test = AttachedScriptsFactory.ScriptFactory(resourceName, new IValue[] { });
			ArrayImpl testArray;
			{
				int methodIndex = test.FindMethod("ПолучитьСписокТестов");

				{
					IValue ivTests;
					test.CallAsFunction(methodIndex, new IValue[] { TestRunner }, out ivTests);
					testArray = ivTests as ArrayImpl;
				}
			}

			foreach (var ivTestName in testArray)
			{
				string testName = ivTestName.AsString();
				int methodIndex = test.FindMethod(testName);
				if (methodIndex == -1)
				{
					// Тест указан, но процедуры нет или она не экспортирована
					continue;
				}

				test.CallAsProcedure(methodIndex, new IValue[] { });
			}
		}

		public ICodeSource LoadFromAssemblyResource(string resourceName)
		{
			var asm = System.Reflection.Assembly.GetExecutingAssembly();
			string codeSource;

			using (Stream s = asm.GetManifestResourceStream(resourceName))
			{
				using (StreamReader r = new StreamReader(s))
				{
					codeSource = r.ReadToEnd();
				}
			}

			return engine.Loader.FromString(codeSource);
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
