using System.IO;
using ScriptEngine.Machine.Contexts;
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
        /// Отправить Файл
        /// </summary>
        /// <returns>Результат выполнения</returns>
       [ContextMethod("ОтправитьФайл")]
        public void UploadFile(string fileName, string dest)
        {
            
            using (var file = new FileStream(@fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _sftpClient.UploadFile(file, dest);
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
        /// Закрыть соединение
        /// </summary>
        [ContextMethod("Разорвать")]
        public void Disconnect()
        {
    
            _sftpClient.Disconnect();
      
        }
    }
}

