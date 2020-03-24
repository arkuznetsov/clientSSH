using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Renci.SshNet;

namespace SshDataProcessorCom
{
    [Guid("26CB8AD5-08EC-459E-9DCE-DB14EAE6FADF")]
    public interface SshCommandComInterface
    {
        int GetExitStatus();
        string GetError();
        void SetTimeout(int hours, int mins, int secs);
        string ReadOutputStreamAsString();
        string ReadOutputStreamAsBinaryData();
        string ReadExtendedOutputStreamAsString();
        string ReadExtendedOutputStreamAsBinaryData();
        string Execute();
    }

    [Guid("9EC04CCC-B75B-457F-9090-1CFC3A59CEAD"),
        InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface SshCommandComEvents
    {
    }

    [Guid("BF5CA1EC-2F92-4651-9A7B-50245FE5B850"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(SshCommandComEvents))]
    public class SshCommandCom : SshCommandComInterface
    {
        private readonly SshCommand _command;
        private readonly System.Text.Encoding _encoding;

        public SshCommandCom()
        {
        }

        public SshCommandCom(SshCommand cmd, System.Text.Encoding encoding)
        {
            _command = cmd;
            _encoding = encoding;
        }

        //[ContextMethod("ПолучитьСтатусЗавершения", "GetExitStatus")]
        public int GetExitStatus()
        {
            return _command.ExitStatus;
        }

        //[ContextMethod("ПолучитьОшибку", "GetError")]
        public string GetError()
        {
            return _command.Error;
        }

        //[ContextMethod("УстановитьТаймаут", "SetTimeout")]
        public void SetTimeout(int hours, int mins, int secs)
        {
            _command.CommandTimeout = new TimeSpan(hours, mins, secs);
        }

        //[ContextMethod("ПрочитатьПотокВыводаКакСтроку", "ReadOutputStreamAsString")]
        public string ReadOutputStreamAsString()
        {

            return ConvertToString(ReadStream(_command.OutputStream));
        }

        //[ContextMethod("ПрочитатьПотокВыводаКакДвоичныеДанные", "ReadOutputStreamAsBinaryData")]
        public string ReadOutputStreamAsBinaryData()
        {
            return System.Convert.ToBase64String(ReadStream(_command.OutputStream));
        }

        //[ContextMethod("ПрочитатьРасширенныйПотокВыводаКакСтроку", "ReadExtendedOutputStreamAsString")]
        public string ReadExtendedOutputStreamAsString()
        {

            return ConvertToString(ReadStream(_command.ExtendedOutputStream));
        }

        //[ContextMethod("ПрочитатьРасширеныйПотокВыводаКакДвоичныеДанные", "ReadExtendedOutputStreamAsBinaryData")]
        public string ReadExtendedOutputStreamAsBinaryData()
        {
            return System.Convert.ToBase64String(ReadStream(_command.ExtendedOutputStream));
        }

        
        private string ConvertToString(byte[] buffer)
        {
            if (buffer.Length > 0)
                return _encoding.GetString(buffer);
            else
                return "";
        }

        private byte[] ReadStream(System.IO.Stream s)
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
        public string Execute()
        {
            _command.Execute();
            return _command.Result;
        }

    }
}
