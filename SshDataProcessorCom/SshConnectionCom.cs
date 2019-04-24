using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Renci.SshNet;

namespace SshDataProcessorCom
{
    [Guid("2FAFFB6C-1714-4276-9BB0-6ED8F4B6AF78")]
    public interface SshConnectionComInterface
    {
        [DispId(1)]
        string Execute(string command);
        [DispId(2)]
        void Disconnect();
    }

    [Guid("8580BFE0-F676-46CD-B6D3-BE9332E44F18"),
        InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface SshConnectionComEvents
    {
    }

    [Guid("54848AAD-D752-41A7-A890-7DC61B9D9056"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(SshConnectionComEvents))]
    public class SshConnectionCom : SshConnectionComInterface
    {
        private readonly SshClient _sshClient;

        public SshConnectionCom()
        {
        }

        public SshConnectionCom(SshClient ssh)
        {
            _sshClient = ssh;
            _sshClient.Connect();
        }

        public string Execute(string command)
        {

            var result = "";
            using (var cmd = _sshClient.CreateCommand(command, Encoding.UTF8))
            {

                cmd.Execute();
                result = cmd.Result;
            }

            return result;

        }

        public void Disconnect()
        {

            _sshClient.Disconnect();

        }
    }
}
