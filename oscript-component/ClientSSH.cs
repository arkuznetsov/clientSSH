using Renci.SshNet;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace oscriptcomponent
{
    /// <summary>
    /// КлиентSSH
    /// </summary>
    [ContextClass("КлиентSSH", "ClientSSH")]
    public class ClientSsh : AutoContext<ClientSsh>
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _user;
        private readonly string _pass;
        private PrivateKeyFile _keyfile;
        private bool _keyFileIsset;

        public ClientSsh(string host, int port, string user, string pass)
        {
            
            _host = host;
            _port = port;
            _user = user;
            _pass = pass;
            
        }

        
        /// <summary>
        /// Получить Поток
        /// </summary>
        [ContextMethod("ПолучитьПоток")]
        public Stream CreateStream()
        {

            var sclient = getSshClient();
            
            return new Stream(sclient);
        }

        
        /// <summary>
        /// Получить Соединение
        /// </summary>
        [ContextMethod("ПолучитьСоединение")]
        public Connection Create()
        {

            var sclient  = getSshClient();
            return new Connection(sclient);
            
            
        }

        /// <summary>
        /// Получить SCP
        /// </summary>
        [ContextMethod("ПолучитьSCP")]
        public Scp CreateScp()
        {

            var scpclient  = new SftpClient(_host, _port, _user, _pass);
            return new Scp(scpclient);
            
        }

        
        
        /// <summary>
        /// Установить ключ
        /// </summary>
        [ContextMethod("УстановитьКлюч")]
        public void SetSshKey(string keyfile, string pass = "")
        {

            _keyfile = new PrivateKeyFile(keyfile, pass);
            _keyFileIsset = true;

        }
        
        /// <summary>
        /// Создает КлиентSSH
        /// </summary>
        /// <returns>КлиентSSH</returns>
        /// <param name="host">Хост</param>>
        [ScriptConstructor]
        public static IRuntimeContextInstance Constructor(IValue host, IValue port, IValue user, IValue pass)
        {
            return new ClientSsh(host.AsString(), (int) port.AsNumber() , user.AsString(), pass.AsString());
        }

        private SshClient getSshClient()
        {
            
            if (_keyFileIsset)
            {
            
                var sclient  = new SshClient(_host, _port, _user, _keyfile);
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