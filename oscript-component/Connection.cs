using System;
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
        }

        
        /// <summary>
        /// Выполнить комманду
        /// </summary>
        /// <returns>Результат выполнения</returns>
       [ContextMethod("ВыполнитьКоманду")]
        public string Execute(string command)
        {

          var result = "";
            using(var cmd = _sshClient.CreateCommand(command, Encoding.UTF8)){
                
                cmd.Execute();
                result = cmd.Result;
            }
            
           return result;
            
        }
    
        
        /// <summary>
        /// Установить соединение
        /// </summary>
        [ContextMethod("Установить")]
        public void Connect()
        {
    
            _sshClient.Connect();
      
        }

        /// <summary>
        /// Получить поток
        /// </summary>
        [ContextMethod("ПолучитьПоток")]
        public void getSteream()
        {
    
           _sshStream = _sshClient.CreateShellStream("xterm", 80, 50, 1024, 1024, 1024);

            while (!_sshStream.DataAvailable)
                System.Threading.Thread.Sleep(200);
        
            var line = _sshStream.Read();
            _sshStream.Flush();
//            return stream;
      
        }

        /// <summary>
        /// Записать в поток
        /// </summary>
        [ContextMethod("ЗаписатьВПоток")]
        public string WriteLine(string command)
        {
    
            _sshStream.Flush();
            _sshStream.WriteLine(command);

            
            StringBuilder output = new StringBuilder();
 
            string line;

            while (!_sshStream.DataAvailable)
                System.Threading.Thread.Sleep(200);


            var num = 0;
            while (_sshStream.DataAvailable)
            {
                if (num > 1)
                {
                    output.Append('\n');
                }
                line = _sshStream.ReadLine();
                output.Append(line);
                num++;
                
            }
         

            return output.ToString();

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

