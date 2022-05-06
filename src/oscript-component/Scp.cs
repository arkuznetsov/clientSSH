/*----------------------------------------------------------
Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
----------------------------------------------------------
// Codebase: https://github.com/ArKuznetsov/oscript-ssh/
----------------------------------------------------------*/

using System.IO;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using Renci.SshNet;
 
namespace oscriptcomponent
{
    /// <summary>
    /// Класс Соединение
    /// </summary>
    [ContextClass("СоединениеSCP", "ConnectionSCP")]
    public class Scp : AutoContext<Scp>
    {
  
        private readonly SftpClient _sftpClient;

        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="scp"></param>
        public Scp(SftpClient scp)
        {
            _sftpClient = scp;
            _sftpClient.Connect();
        }

        /// <summary>
        /// Создать каталог
        /// </summary>
        /// <param name="path">Path to file or directory.</param>
        /// <returns>
        /// <c>true</c> if directory or file exists; otherwise <c>false</c>.
        /// </returns>
        [ContextMethod("Существует")]
        public IValue Exists(string path)
        {

            return ValueFactory.Create(_sftpClient.Exists(path));

        }

        /// <summary>
        /// Создать каталог
        /// </summary>
        /// <param name="path">Path to new directory.</param>
        [ContextMethod("СоздатьКаталог")]
        public void CreateDirectory(string path)
        {

            _sftpClient.CreateDirectory(path);

        }

        /// <summary>
        /// Удалить каталог
        /// </summary>
        /// <param name="path">Path to directory to delete.</param>
        [ContextMethod("УдалитьКаталог")]
        public void DeleteDirectory(string path)
        {

            _sftpClient.DeleteDirectory(path);

        }

        /// <summary>
        /// Отправить Файл
        /// </summary>
        /// <param name="fileName">File path.</param>
        /// <param name="dest">Remote file path.</param>
        /// <param name="canOwerwrite">if set to <c>true</c> then existing file will be overwritten.</param>
        /// <returns>Результат выполнения</returns>
        [ContextMethod("ОтправитьФайл")]
        public void UploadFile(string fileName, string dest, IValue canOwerwrite = null)
        {
            using (var file = new FileStream(@fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (canOwerwrite == null)
                {
                    _sftpClient.UploadFile(file, dest);
                }
                else
                {
                    _sftpClient.UploadFile(file, dest, canOwerwrite.AsBoolean());
                }
            }
            
        }
     
        /// <summary>
        /// Получить Файл
        /// </summary>
        /// <returns>Результат выполнения</returns>
        [ContextMethod("ПолучитьФайл")]
        public void DownloadFile(string src, string dest)
        {

            using (var file = new FileStream(@dest, FileMode.OpenOrCreate, FileAccess.Write))
            {
                _sftpClient.DownloadFile(src, file);
            }
            
        }

        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="path">Path to file to delete.</param>
        [ContextMethod("УдалитьФайл")]
        public void DeleteFile(string path)
        {

            _sftpClient.DeleteFile(path);

        }

        /// <summary>
        /// Закрыть соединение
        /// </summary>
        [ContextMethod("Разорвать")]
        public void Disconnect()
        {
    
            _sftpClient.Disconnect();
      
        }
    }
}

