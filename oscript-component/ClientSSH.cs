using Renci.SshNet;
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

        private  SshClient _sshClient;
        
        public ClientSSH()
        {
        }

        
        /// <summary>
        /// Получить соединение
        /// </summary>
        [ContextMethod("ПолучитьСоединение")]
        public Connection Create(string Host, int Port, string User, string Pass)
        {

          var sclient = new SshClient(Host, Port, User, Pass);
            
            return new Connection(sclient);
        }
  
        
        /// <summary>
        /// Разорвать соединение
        /// </summary>
        [ContextMethod("РазорватьСоединение")]
        public void Close()
        {

                _sshClient.Disconnect();
            
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