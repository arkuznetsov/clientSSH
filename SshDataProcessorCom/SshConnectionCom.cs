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
        [DispId(3)]
        SshCommandCom NewSshCommand(string command, string encoding);
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

        //[ContextMethod("НовыйКомандаSSH", "NewSSHCommand")]
        public SshCommandCom NewSshCommand(string command, string encoding)
        {
            System.Text.Encoding enc = GetEncodingByName(encoding);
            return new SshCommandCom(_sshClient.CreateCommand(command, enc), enc);
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

        private Encoding GetEncodingByName(string encoding, bool addBOM = true)
        {
            Encoding enc;
            if (encoding == null)
                enc = new UTF8Encoding(addBOM);
            else
            {
                switch (encoding.ToUpper())
                {
                    case "UTF-8":
                        enc = new UTF8Encoding(addBOM);
                        break;
                    case "UTF-16":
                    case "UTF-16LE":
                    // предположительно, варианты UTF16_PlatformEndian\UTF16_OppositeEndian
                    // зависят от платформы x86\m68k\SPARC. Пока нет понимания как корректно это обработать.
                    // Сейчас сделано исходя из предположения что PlatformEndian должен быть LE поскольку 
                    // платформа x86 более широко распространена
                    case "UTF16_PLATFORMENDIAN":
                        enc = new UnicodeEncoding(false, addBOM);
                        break;
                    case "UTF-16BE":
                    case "UTF16_OPPOSITEENDIAN":
                        enc = new UnicodeEncoding(true, addBOM);
                        break;
                    case "UTF-32":
                    case "UTF-32LE":
                    case "UTF32_PLATFORMENDIAN":
                        enc = new UTF32Encoding(false, addBOM);
                        break;
                    case "UTF-32BE":
                    case "UTF32_OPPOSITEENDIAN":
                        enc = new UTF32Encoding(true, addBOM);
                        break;
                    default:
                        enc = Encoding.GetEncoding(encoding);
                        break;

                }
            }

            return enc;
        }

    }
}
