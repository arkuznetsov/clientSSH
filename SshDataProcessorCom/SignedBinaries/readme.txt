В этой папке должны содержаться подписанные dll
CoreTechs.Sftp.Client.dll
Renci.SshNet.dll

Исходные dll можно подписать, использовав:
> ildasm /all /out=MYASSEMBLY.il MYASSEMBLY.dll
> ilasm /dll /key=key.snk MYASSEMBLY.il

https://stackoverflow.com/questions/1220519/how-to-sign-a-net-assembly-dll-file-with-a-strong-name