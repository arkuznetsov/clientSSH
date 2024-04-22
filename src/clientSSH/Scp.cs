/*----------------------------------------------------------
Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
----------------------------------------------------------
// Codebase: https://github.com/ArKuznetsov/clientSSH/
----------------------------------------------------------*/

using System.IO;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library;
using Renci.SshNet;
using Renci.SshNet.Sftp;

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
        /// Существует
        /// </summary>
        /// <param name="path">Путь к файлу или каталогу</param>
        /// <returns>
        /// <c>true</c> если файл или каталог существует; в противном случае <c>false</c>.
        /// </returns>
        [ContextMethod("Существует")]
        public IValue Exists(string path)
        {

            return ValueFactory.Create(_sftpClient.Exists(path));

        }

        /// <summary>
        /// Содержимое каталога
        /// </summary>
        /// <param name="path">Путь к каталогу</param>
        /// <returns>
        /// <c>Соответствие</c> - Список файлов и каталогов в указанном каталоге.</returns>
        /// <c>Ключ</c> - Имя файла / каталога
        /// <c>Значение</c> - <c>Структура</c> - описание файла / каталога
        /// <c>Имя</c> - имя файла
        /// <c>ПолноеИмя</c> - полный путь к файлу
        /// <c>ЭтоФайл</c> - это файл
        /// <c>ЭтоКаталог</c> - это каталог
        /// </returns>
        [ContextMethod("СодержимоеКаталога")]
        public IValue ListDirectory(string path)
        {

            var SrcList = _sftpClient.ListDirectory(path);

            MapImpl ResultList = new MapImpl();

            IValue ResultItemKey;
            StructureImpl ResultItemValue;

            foreach (SftpFile item in SrcList)
            {
                ResultItemKey = ValueFactory.Create(item.Name);

                ResultItemValue = new StructureImpl();
                ResultItemValue.Insert("Имя", ValueFactory.Create(item.Name));
                ResultItemValue.Insert("ПолноеИмя", ValueFactory.Create(item.FullName));
                ResultItemValue.Insert("ЭтоФайл", ValueFactory.Create(item.IsRegularFile));
                ResultItemValue.Insert("ЭтоКаталог", ValueFactory.Create(item.IsDirectory));
                ResultItemValue.Insert("Размер", ValueFactory.Create(item.Length));

                ResultList.Insert(ResultItemKey, ResultItemValue);
            }
            return ValueFactory.Create(ResultList);

        }

        /// <summary>
        /// Создать каталог
        /// </summary>
        /// <param name="path">Путь к новому каталогу.</param>
        [ContextMethod("СоздатьКаталог")]
        public void CreateDirectory(string path)
        {

            _sftpClient.CreateDirectory(path);

        }

        /// <summary>
        /// Удалить каталог
        /// </summary>
        /// <param name="path">Путь к удаляемому каталогу.</param>
        [ContextMethod("УдалитьКаталог")]
        public void DeleteDirectory(string path)
        {

            _sftpClient.DeleteDirectory(path);

        }

        /// <summary>
        /// Отправить Файл
        /// </summary>
        /// <param name="fileName">Путь к отправляемому файлу.</param>
        /// <param name="dest">Путь к файлу на сервере.</param>
        /// <param name="canOwerwrite">если указано <c>true</c>, то существующий файл на сервере будет перезаписан.</param>
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
        /// <param name="src">Путь к файлу на сервере.</param>
        /// <param name="dest">Путь для загрузки файла.</param>
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
        /// <param name="path">Путь к файлу для удаления.</param>
        [ContextMethod("УдалитьФайл")]
        public void DeleteFile(string path)
        {

            _sftpClient.DeleteFile(path);

        }

        /// <summary>
        /// Закрыть соединение
        /// </summary>
        [ContextMethod("Отключиться")]
        public void Disconnect()
        {
    
            _sftpClient.Disconnect();
      
        }
    }
}

