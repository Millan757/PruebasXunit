using Chat.Client.Library.Helpers;
using Chat.Client.Library.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Chat.Client
{
    public class Program
    {
        private static IChatClient _chatClient;
        private static string _username;
        private static string _password;

        private static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5000/api/");
            _chatClient = new ChatClient(new ChatApiClient(httpClient), new UserApiClient(httpClient));

            WelcomeMessage();

            var key = Console.ReadKey();

            CheckLoginSelection(key);

            IntroduceLoginData();

            await LoginOrCreateUserAsync(key);


            _chatClient.OverwriteLastLine += (sender, _) => { Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1); };
            _chatClient.NewMessageRecived += (sender, s) => { Console.WriteLine(s); };
            _chatClient.Connect();

            var message = "";

            Console.Clear();
            while (true)
            {
                message = Console.ReadLine();
                if (message == "exit")
                {
                    break;
                }

                await _chatClient.SendMessageAsync(message);
            }

            _chatClient.Dispose();
        }

        private static void IntroduceLoginData()
        {
            Console.WriteLine("");
            Console.WriteLine("Introduce your name:");
            _username = Console.ReadLine();
            Console.WriteLine("Introduce your password:");
            _password = CryptographyHelper.CalculateHash(Console.ReadLine());
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to this chat");
            Console.WriteLine("Do you have user?");
            Console.WriteLine("1-Yes");
            Console.WriteLine("2-No");
        }

        private static void CheckLoginSelection(ConsoleKeyInfo key)
        {
            if (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2)
            {
                Console.WriteLine("Invalid option");
                Console.WriteLine("Restart the program.");
                Console.Read();
                Environment.Exit(0);
            }
        }

        private static async Task LoginOrCreateUserAsync(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.D1)
            {
                if (await _chatClient.LoginAsync(_username, _password))
                {
                    Console.WriteLine("Logged in!");
                }
                else
                {
                    Console.WriteLine("Username or password are incorrect.");
                    Console.WriteLine("Restart the program.");
                    Console.Read();
                    Environment.Exit(0);
                }
            }
            else
            {
                if (await _chatClient.CreateUserAsync(_username, _password))
                {
                    Console.WriteLine("User created!");
                }
                else
                {
                    Console.WriteLine("Problem creating the user.");
                    Console.WriteLine("Restart the program.");
                    Console.Read();
                    Environment.Exit(0);
                }
            }
        }
    }
}
