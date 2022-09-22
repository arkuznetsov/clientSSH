/*----------------------------------------------------------
Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
----------------------------------------------------------
// Codebase: https://github.com/ArKuznetsov/clientSSH/
----------------------------------------------------------*/

using System.Text;
using ScriptEngine.Machine.Contexts;
using Renci.SshNet;
 
namespace oscriptcomponent
{
    /// <summary>
    /// Класс Поток
    /// </summary>
    [ContextClass("ПотокSSH", "StreamSSH")]
    public class Stream : AutoContext<Stream>
    {
  
        private readonly SshClient _sshClient;
        private readonly ShellStream _sshStream;

        public Stream(SshClient ssh)
        {
            _sshClient = ssh;
            _sshClient.Connect();
            
            _sshStream = _sshClient.CreateShellStream("xterm", 80, 50, 1024, 1024, 1024);

            while (!_sshStream.DataAvailable)
                System.Threading.Thread.Sleep(200);
        
            var line = _sshStream.Read();
            _sshStream.Flush();
  
        }

        
        
        /// <summary>
        /// Записать в поток
        /// </summary>
        /// <returns>РезультатВыполнения</returns>
        [ContextMethod("ЗаписатьВПоток")]
         public string WriteLine(string command)
        {
    
            _sshStream.Flush();
            _sshStream.WriteLine(command);

            
            StringBuilder output = new StringBuilder();
 
            string line;

            while (!_sshStream.DataAvailable)
                System.Threading.Thread.Sleep(200);

//            System.Threading.Thread.Sleep(200);
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
        [ContextMethod("Отключиться")]
        public void DisconnectStream()
        {
    
            _sshClient.Disconnect();
      
        }
    }
}

