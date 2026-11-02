using System.IO;
using AirlineProject.Data;
using AirlineProject.Services;
using AirlineProject.UI;

namespace AirlineProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Подключаем поддержку кодировки для корректного вывода кириллицы в консоли
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            string filePath = "aircrafts.txt";

            // Инициализация компонентов архитектуры
            IAirlineRepository repository = new FileAirlineRepository(filePath);
            var aircrafts = repository.LoadAircrafts();

            IAirlinesService service = new AirlineService(aircrafts);
            ConsoleUI ui = new ConsoleUI(service);

            // Запуск консольного приложения
            ui.Run();
        }
    }
}