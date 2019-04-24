using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Renci.SshNet;

namespace SshDataProcessorCom
{
    [Guid("AB50019B-D4F8-41CA-B4B6-C15376A99DA3")]
    public interface StreamComInterface
    {
        [DispId(1)]
        string WriteLine(string command);
        [DispId(2)]
        void DisconnectStream();

    }

    [Guid("7846DF17-8C48-427A-B439-E945304A9EAE"),
        InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface StreamComEvents
    {
    }

    [Guid("455B1311-50F7-475F-B461-F1381E246FAA"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(StreamComEvents))]
    public class StreamCom : StreamComInterface
    {
        private SshClient _sshClient;
        private ShellStream _sshStream;

        public StreamCom()
        {
        }

        public StreamCom(SshClient ssh)
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
        //[ContextMethod("ЗаписатьВПоток")]
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
        //[ContextMethod("Разорвать")]
        public void DisconnectStream()
        {
            _sshClient.Disconnect();
        }
    }
}
