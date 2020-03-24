using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Renci.SshNet;

namespace SshDataProcessorCom
{
    [Guid("3F030104-744E-4ADD-8635-F39875A2E74B")]
    public interface SshClientComInterface
    {
        [DispId(1)]
        SshConnectionCom Create();
        [DispId(2)]
        void SetSshKey(string keyfile, string pass = "");
        [DispId(3)]
        StreamCom CreateStream();
        [DispId(4)]
        ScpCom CreateScp();
    }

    [Guid("38592B36-5F71-425F-A77D-E14F6A3CF16B"),
        InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface SshClientComEvents
    {
    }

    [Guid("5D39AE4A-1464-456B-A3BC-4934466BED02"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(SshClientComEvents))]
    public class SshClientCom : SshClientComInterface
    {
        private string _host;
        private int _port;
        private string _user;
        private readonly string _pass;
        private PrivateKeyFile _keyfile;
        private bool _keyFileIsset;

        public SshClientCom()
        {
        }

        public SshClientCom(string host, int port, string user, string pass)
        {
            _host = host;
            _port = port;
            _user = user;
            _pass = pass;
        }

        /// <summary>
        /// Получить Поток
        /// </summary>
        //[ContextMethod("ПолучитьПоток")]
        public StreamCom CreateStream()
        {
            var sclient = getSshClient();
            return new StreamCom(sclient);
        }

        /// <summary>
        /// Получить Соединение
        /// </summary>
        //[ContextMethod("ПолучитьСоединение")]
        public SshConnectionCom Create()
        {

            var sclient = getSshClient();
            return new SshConnectionCom(sclient);


        }

        /// <summary>
        /// Получить SCP
        /// </summary>
        //[ContextMethod("ПолучитьSCP")]
        public ScpCom CreateScp()
        {
            if (_keyFileIsset)
            {
                var scplient = new SftpClient(_host, _port, _user, _keyfile);
                return new ScpCom(scplient);
            }
            else
            {
                var scplient = new SftpClient(_host, _port, _user, _pass);
                return new ScpCom(scplient);
            }
        }

        /// <summary>
        /// Установить ключ
        /// </summary>
        //[ContextMethod("УстановитьКлюч")]
        public void SetSshKey(string keyfile, string pass = "")
        {
            _keyfile = new PrivateKeyFile(keyfile, pass);
            _keyFileIsset = true;
        }

        private SshClient getSshClient()
        {
            if (_keyFileIsset)
            {
                var sclient = new SshClient(_host, _port, _user, _keyfile);
                return sclient;
            }
            else
            {
                var sclient = new SshClient(_host, _port, _user, _pass);
                return sclient;
            }
        }
    }
}
