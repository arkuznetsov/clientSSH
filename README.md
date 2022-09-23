# Oscript SSH client 

[![GitHub release](https://img.shields.io/github/release/ArKuznetsov/clientSSH.svg?style=flat-square)](https://github.com/ArKuznetsov/clientSSH/releases)
[![GitHub license](https://img.shields.io/github/license/ArKuznetsov/clientSSH.svg?style=flat-square)](https://github.com/ArKuznetsov/clientSSH/blob/develop/LICENSE)
[![GitHub Releases](https://img.shields.io/github/downloads/ArKuznetsov/clientSSH/latest/total?style=flat-square)](https://github.com/ArKuznetsov/clientSSH/releases)
[![GitHub All Releases](https://img.shields.io/github/downloads/ArKuznetsov/clientSSH/total?style=flat-square)](https://github.com/ArKuznetsov/clientSSH/releases)

[![Build Status](https://img.shields.io/github/workflow/status/ArKuznetsov/clientSSH/%D0%9A%D0%BE%D0%BD%D1%82%D1%80%D0%BE%D0%BB%D1%8C%20%D0%BA%D0%B0%D1%87%D0%B5%D1%81%D1%82%D0%B2%D0%B0)](https://github.com/arkuznetsov/clientSSH/actions/)
[![Quality Gate](https://open.checkbsl.org/api/project_badges/measure?project=clientSSH&metric=alert_status)](https://open.checkbsl.org/dashboard/index/clientSSH)
[![Coverage](https://open.checkbsl.org/api/project_badges/measure?project=clientSSH&metric=coverage)](https://open.checkbsl.org/dashboard/index/clientSSH)
[![Tech debt](https://open.checkbsl.org/api/project_badges/measure?project=clientSSH&metric=sqale_index)](https://open.checkbsl.org/dashboard/index/clientSSH)

## SSH клиент для oscript

## Примеры использования
### SSH клиент

```bsl
#Использовать ClientSSH
    
КлиентSSH = Новый КлиентSSH("127.0.0.1", 22, "user", "password");
Соединение = КлиентSSH.ПолучитьСоединение();
Результат = Соединение.ВыполнитьКоманду("echo 123");   
    
Соединение.Отключиться();

```

### Клиент для конфигуратора в режиме Агента 

Запустить конфигуратор в режиме агента:  
`
1cv8.exe DESIGNER /F"<ПутьКБазе>" /AgentMode /Visible /AgentSSHHostKeyAuto /AgentBaseDir "<ПутьКПапкеВыгрузки>"
`


```bsl
#Использовать ClientSSH

КлиентSSH = Новый КлиентSSH("127.0.0.1", 1543, "admin", "");
Поток = КлиентSSH.ПолучитьПоток();

// Следующие строки обязательны, иначе скрипт зависает
// вариант для 8.3.16 и выше
Результат = Поток.ЗаписатьВПоток("options set --show-prompt=no");
// вариант для 8.3.15 и ниже
Результат = Поток.ЗаписатьВПоток("options set --show-prompt=no --output-format=json");

Результат = Поток.ЗаписатьВПоток("common connect-ib");
Результат = Поток.ЗаписатьВПоток("config dump-config-to-files --dir .");
Результат = Поток.ЗаписатьВПоток("common disconnect-ib");

Поток.Отключиться();

```


### Авторизация ssh с ключом

```bsl
#Использовать ClientSSH
    
КлиентSSH = Новый КлиентSSH("127.0.0.1", 22, "user", "");
КлиентSSH.УстановитьКлюч("ПутьКСекретномуКлючу", "СекретнаяФраза");
Соединение = КлиентSSH.ПолучитьСоединение();
Результат = Соединение.ВыполнитьКоманду("echo 123");   
    
Соединение.Отключиться();

```

### Передача файлов

```bsl
#Использовать ClientSSH
    
КлиентSSH = Новый КлиентSSH("127.0.0.1", 1543, "admin", "");
Scp = КлиентSSH.ПолучитьScp();
Scp.ОтправитьФайл("C:\cf\1Cv8.cf", "/1Cv8.cf");

Scp.ПолучитьФайл("/1Cv8.cf", "C:\cf\1Cv8_2.cf");
Scp.Отключиться();

```

## Известные проблемы:
* Вешается поток, если не передать следующие настройки:  
  - для 8.3.16 и выше
    - `Поток.ЗаписатьВПоток("options set --show-prompt=no");`  
  - для 8.3.15 и ниже
    - `Поток.ЗаписатьВПоток("options set --show-prompt=no --output-format=json");`  
 * В папке выгрузки создается файл `agentbasedir.json` и подпапка с именем пользователя (Особенность режима Агента)

Пример json-файла

```json
{
"usersInfo": [
{
"name": "Администратор",
"dir": "0"
}
]
}
```
