/*----------------------------------------------------------
Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
----------------------------------------------------------
// Codebase: https://github.com/ArKuznetsov/oscript-ssh/
----------------------------------------------------------*/

using NUnit.Framework;
using oscriptcomponent;

// Используется NUnit 3.6

namespace NUnitTests
{
	[TestFixture]
	public class MainTestClass
	{

		private EngineHelpWrapper _host;

		[OneTimeSetUp]
		public void Initialize()
		{
			_host = new EngineHelpWrapper();
			_host.StartEngine();
		}

		[Test]
		public void TestAsInternalObjects()
		{
			var item1 = new ClientSsh("", 22, "", "");
       
//			Assert.AreEqual(item1.ReadonlyProperty, "MyValue"); 
		}

		[Test]
		public void TestAsExternalObjects()
		{
			_host.RunTestScript("NUnitTests.Tests.external.os");
		}
	}
}
