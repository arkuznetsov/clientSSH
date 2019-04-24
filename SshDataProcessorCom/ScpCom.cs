using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Renci.SshNet;
using System.IO;

namespace SshDataProcessorCom
{
    [Guid("5437D1F8-E035-4A21-8549-4E1AC6B1DD75")]
    public interface ScpComInterface
    {
        [DispId(1)]
        void UploadFile(string fileName, string dest);
        [DispId(2)]
        void DownloadFile(string src, string dest);
        [DispId(1)]
        void Disconnect();
    }

    [Guid("01A32653-D52E-4FCE-A709-37B8E7C620AB"),
        InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ScpComEvents
    {
    }

    [Guid("92FD4F93-2AA6-451B-9284-F5AEA61452A7"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(ScpComEvents))]
    public class ScpCom : ScpComInterface
    {
        private readonly SftpClient _sftpClient;

        public ScpCom()
        {
        }

        public ScpCom(SftpClient scp)
        {
            _sftpClient = scp;
            _sftpClient.Connect();
        }

        /// <summary>
        /// Отправить Файл
        /// </summary>
        /// <returns>Результат выполнения</returns>
        //[ContextMethod("ОтправитьФайл")]
        public void UploadFile(string fileName, string dest)
        {
            var file = new FileStream(@fileName, FileMode.Open, FileAccess.Read);
            _sftpClient.UploadFile(file, dest);
        }

        /// <summary>
        /// Получить Файл
        /// </summary>
        /// <returns>Результат выполнения</returns>
        //[ContextMethod("ПолучитьФайл")]
        public void DownloadFile(string src, string dest)
        {
            var file = new FileStream(@dest, FileMode.OpenOrCreate, FileAccess.Write);
            _sftpClient.DownloadFile(src, file);
        }

        /// <summary>
        /// Закрыть соединение
        /// </summary>
        //[ContextMethod("Разорвать")]
        public void Disconnect()
        {
            _sftpClient.Disconnect();
        }
    }
}
