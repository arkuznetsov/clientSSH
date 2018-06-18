using System.Text;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using Renci.SshNet;
 
namespace oscriptcomponent
{
    /// <summary>
    /// Класс Соединение
    /// </summary>
    [ContextClass("СоединениеSSH", "ConnectionSSH")]
    public class Connection : AutoContext<Connection>
    {
  
        private readonly SshClient _sshClient;
        private  ShellStream _sshStream;

        public Connection(SshClient ssh)
        {
            _sshClient = ssh;
            _sshClient.Connect();
        }

        
        /// <summary>
        /// Выполнить комманду
        /// </summary>
        /// <returns>Результат выполнения</returns>
       [ContextMethod("ВыполнитьКоманду")]
        public string Execute(string command)
        {

          var result = "";
            using(var cmd = _sshClient.CreateCommand(command, Encoding.UTF8))
            {
                
                cmd.Execute();
                result = cmd.Result;
            }
            
           return result;
            
        }
        
        /// <summary>
        /// Закрыть соединение
        /// </summary>
        [ContextMethod("Разорвать")]
        public void Disconnect()
        {
    
            _sshClient.Disconnect();
      
        }
    }
}

