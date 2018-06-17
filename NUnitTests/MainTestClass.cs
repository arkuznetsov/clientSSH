using System;
using System.IO;
using NUnit.Framework;
using ScriptEngine.HostedScript;
using ScriptEngine.Machine;
using ScriptEngine.Environment;
using oscriptcomponent;

// Используется NUnit 3.6

namespace NUnitTests
{
	[TestFixture]
	public class MainTestClass
	{

		private EngineHelpWrapper host;

		[OneTimeSetUp]
		public void Initialize()
		{
			host = new EngineHelpWrapper();
			host.StartEngine();
		}

		[Test]
		public void TestAsInternalObjects()
		{
			var item1 = new ClientSSH();
       
//			Assert.AreEqual(item1.ReadonlyProperty, "MyValue"); 
		}

		[Test]
		public void TestAsExternalObjects()
		{
			host.RunTestScript("NUnitTests.Tests.external.os");
		}
	}
}
