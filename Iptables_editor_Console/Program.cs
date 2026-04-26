using IPTables.Net.Iptables;

namespace Renci.SshNet
{
    public class Program
    {
        static string host = "";
        static string username = "";
        static string portdef = "22";
        static string password = "";
        static void CheckString(string conn)
        {
            int count = 0, dog = 0, dvt = 0;
            int length = conn.Length;
            while(count < length-1)
            {
                if (conn[count] == '@')
                {
                    dog = count;
                    count++;
                }
                else if (conn[count] == ':')
                {
                    dvt = count;
                    count++;
                }
                else count++;
            }
            count = 0;
            while(count < dog)
            {
                username += conn[count];
                count++;
            }
            if (dvt != 0) {
                while (count < dvt - 1)
                {
                    count++;
                    host += conn[count];
                }
                portdef = "";
                while (dvt < length-1)
                {
                    dvt++;
                    portdef += conn[dvt];
                }
            }
            else
            {
                while (count < length-1)
                {
                    count++;
                    host += conn[count];
                }
            }
            Console.WriteLine(portdef);
            Console.WriteLine(username);
            Console.WriteLine(host);
        } //Парсим строку логин+хост+порт, сделать проверку на ошибки и некорректный ввод, пока не так важно
        static void CheckPwd()
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        if ((password.Length - 1) >= 0)
                        {
                            password = password.Remove(password.Length - 1);
                            Console.Write("\b \b");
                            continue;
                        }
                        else
                            continue;
                    case ConsoleKey.Enter:
                        break;
                    default:
                        password += key.KeyChar;
                        Console.Write("*");
                        continue;
                }
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
        }
        static void Main()
        {
            Console.WriteLine("Привет! Это мое приложение для более гибкой настройки iptables в Linux, тебе тоже скорее всего надоело заниматься дичью и возиться с командами для работы с iptables" +
                "\nЭта программа создается с целью ускорить настройку правил файервола для Linux, в первую очередь я думаю о замене местами самих правил, их редактирование и т.д. Планов много, но надо все обдумывать." +
                "\nВведите логин и хост в формате login@host:port");
            CheckString(Console.ReadLine()); //Передаем функции ввод из строки
            Console.WriteLine("Введите пароль: ");
            CheckPwd();
            int port = int.Parse(portdef); // 
            var client = new SshClient(host, port, username, password);
            client.Connect();
            var stream = client.CreateShellStream("xterm", 80, 24, 1024, 768, 1024);
            System.Threading.Thread.Sleep(1500);
            stream.WriteLine("sudo -p 'PASSWORD:' iptables-save > iptables.txt");
            stream.WriteLine(password);
            System.Threading.Thread.Sleep(1000);
            client.Disconnect();
        }
    }
}