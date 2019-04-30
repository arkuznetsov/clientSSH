using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using Renci.SshNet;
using ScriptEngine.HostedScript.Library.Binary;

namespace oscriptcomponent
{
    [ContextClass("КомандаSSH", "CommandSSH")]
    public class CommandSsh : AutoContext<CommandSsh>
    {
        private readonly SshCommand _command;
        private readonly System.Text.Encoding _encoding;

        public CommandSsh(SshCommand cmd, System.Text.Encoding encoding)
        {
            _command = cmd;
            _encoding = encoding;
        }

        [ContextMethod("ПолучитьСтатусЗавершения", "GetExitStatus")]
        public int GetExitStatus()
        {
            return _command.ExitStatus;
        }

        [ContextMethod("ПолучитьОшибку", "GetError")]
        public string GetError()
        {
            return _command.Error;
        }

        [ContextMethod("УстановитьТаймаут", "SetTimeout")]
        public void SetTimeout(int hours, int mins, int secs)
        {
            _command.CommandTimeout = new TimeSpan(hours, mins, secs);
        }



        [ContextMethod("ПрочитатьПотокВыводаКакСтроку", "ReadOutputStreamAsString")]
        public string ReadOutputStreamAsString()
        {

            return ConvertToString(ReadStream(_command.OutputStream));
        }

        [ContextMethod("ПрочитатьПотокВыводаКакДвоичныеДанные", "ReadOutputStreamAsBinaryData")]
        public BinaryDataContext ReadOutputStreamAsBinaryData()
        {
            return new BinaryDataContext(ReadStream(_command.OutputStream));
        }

        [ContextMethod("ПрочитатьРасширенныйПотокВыводаКакСтроку", "ReadExtendedOutputStreamAsString")]
        public string ReadExtendedOutputStreamAsString()
        {

            return ConvertToString(ReadStream(_command.ExtendedOutputStream));
        }

        [ContextMethod("ПрочитатьРасширеныйПотокВыводаКакДвоичныеДанные", "ReadExtendedOutputStreamAsBinaryData")]
        public BinaryDataContext ReadExtendedOutputStreamAsBinaryData()
        {
            return new BinaryDataContext(ReadStream(_command.ExtendedOutputStream));
        }

        private string ConvertToString(byte[] buffer)
        {
            if (buffer.Length > 0)
                return _encoding.GetString(buffer);
            else
                return "";
        }

        private byte [] ReadStream(System.IO.Stream s)
        {
            long bytesCount = s.Length - s.Position;

            byte[] buffer = new byte[bytesCount];
            int bytesReaded = s.Read(buffer, (int)s.Position, (int)bytesCount);
            System.Array.Resize(ref buffer, bytesReaded);
            return buffer;
        }

        /// <summary>
        /// Выполнить комманду
        /// </summary>
        /// <returns>Результат выполнения</returns>
        [ContextMethod("ВыполнитьКоманду")]
        public string Execute()
        {
            _command.Execute();
            return _command.Result;
            
        }

    }
}
