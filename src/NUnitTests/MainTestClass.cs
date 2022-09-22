/*----------------------------------------------------------
Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
----------------------------------------------------------
// Codebase: https://github.com/ArKuznetsov/oscript-ssh/
----------------------------------------------------------*/

using NUnit.Framework;
using oscriptcomponent;
using System.Collections.Generic;
using ScriptEngine.HostedScript.Library;

// Используется NUnit 3.6

namespace NUnitTests
{
	[TestFixture]
	public class MainTestClass
	{

		private EngineHelpWrapper _host;

		public static List<TestCaseData> TestCases
		{
			get
			{
				var testCases = new List<TestCaseData>();
				EngineHelpWrapper _host = new EngineHelpWrapper();
				_host.StartEngine();

				ArrayImpl testMethods =_host.GetTestMethods("NUnitTests.Tests.external.os");

				foreach (var ivTestMethod in testMethods)
				{
					testCases.Add(new TestCaseData(ivTestMethod.ToString()));
				}

				return testCases;
			}
		}
		
		[OneTimeSetUp]
		public void Initialize()
		{
			_host = new EngineHelpWrapper();
			_host.StartEngine();
		}

		[Test]
		[Category("Create SSH client")]
		public void TestAsInternalObjects()
		{
			var item1 = new ClientSsh("", 22, "", "");

			Assert.IsNotNull(item1, "Ошибка создания экземпляра класса {0}", typeof(ClientSsh)); 
		}

		[TestCaseSource(nameof(TestCases))]
		[Category("Test SSH client")]
		public void TestAsExternalObjects(string testCase)
		{
			string testException;

			int result = _host.RunTestMethod("NUnitTests.Tests.external.os", testCase, out testException);
			
			switch (result)
			{
				case -1:
					Assert.Ignore("Тест: {0} не реализован!", testCase);
					break;
				case 0:
					Assert.Pass();
					break;
				case 1:
					Assert.Fail("Тест: {0} провален с сообщением: {1}", testCase, testException);
					break;
				default:
					Assert.Warn("Тест: {0} вернул неожиданный результат: {1}", testCase, result);
					break;
			}
		}
	}
}
