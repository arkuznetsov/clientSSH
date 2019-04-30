using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SshDataProcessorCom;

namespace TestSshCom
{
    class Program
    {
        static void Main(string[] args)
        {
            var dp = new SshDataProcessorCom.SshDataProcessorCom();
            var cl = dp.NewSSHClient("192.168.1.199", 22, "onescript", "123456");
            var cn = cl.Create();
            var cm = cn.NewSshCommand("echo \"123456\" | sudo -S service apache2 restart", "utf-8");
            var res = cm.Execute();
            var err = cm.GetError();
            var mm = cm.ReadExtendedOutputStreamAsString();
            var os = cm.ReadOutputStreamAsString();
           
            var ec = cm.GetExitStatus();
            cn.Disconnect();

        }
    }
}
