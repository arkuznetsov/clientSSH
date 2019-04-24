using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptEngine;
using ScriptEngine.HostedScript;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using oscriptcomponent;

namespace SshClientFabric
{
    // Это класс менеджера обработки
    [ContextClass("ФабрикаКлиентSSH", "SSHClientFabric")]
    public class SshClientFabricDataProcessorManager : AutoContext<SshClientFabricDataProcessorManager>
    {
        public SshClientFabricDataProcessorManager()
        {

        }

        // Метод платформы
        [ContextMethod("Создать", "Create")]
        public IValue Create()
        {
            return new SshClientFabricDataProcessorObject();
        }

        // Статический метод модуля менеджера
        [ContextMethod("НовыйКлиентSSH", "NewSSHClient")]
        public ClientSsh NewSSHClient(string host, int port, string user, string pass)
        {
            return new ClientSsh(host, port, user, pass);
        }


    }

    // Это класс модуля объекта обработки
    [ContextClass("ФабрикаКлиентSSHОбъект", "ФабрикаКлиентSSHObject")]
    public class SshClientFabricDataProcessorObject : AutoContext<SshClientFabricDataProcessorObject>
    {
        public SshClientFabricDataProcessorObject()
        {

        }
        /*
        [ContextProperty("Свойство", "Property")]
        public string Property
        {
            get;
            set;
        }

        [ContextProperty("Свойство", "Property")]
        public int Property
        {
            get;
            set;
        }

        [ContextProperty("Свойство", "Property")]
        public int Property
        {
            get;
            set;
        }

        [ContextProperty("Свойство", "Property")]
        public int Property
        {
            get;
            set;
        }

        [ContextMethod("Сложить", "Add")]
        public int Add(int number1, int number2)
        {
            return number1 + number2;
        }
        */
    }
}
