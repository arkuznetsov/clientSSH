using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace SshDataProcessorCom
{
    [Guid("47D3C751-C4C7-4049-BB7F-06ED74462C25")]
    public interface SshDataProcessorComInterface
    {
        [DispId(1)]
        SshClientCom NewSSHClient(string host, int port, string user, string pass);
    }

    [Guid("CBD68176-21CD-4796-AAD2-7A2D01B9919C"),
        InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface SshDataProcessorComEvents
    {
    }

    [Guid("1CFFEE15-ED82-4CF8-A460-8B00FFF69246"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(SshDataProcessorComEvents))]
    public class SshDataProcessorCom : SshDataProcessorComInterface
    {
        private SshDataProcessorCom _pipeline;

        public SshDataProcessorCom()
        {
        }

        //[ContextMethod("НовыйКлиентSSH", "NewSSHClient")]
        public SshClientCom NewSSHClient(string host, int port, string user, string pass)
        {
            return new SshClientCom(host, port, user, pass);
        }
    }
}
