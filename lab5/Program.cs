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
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            string filePath = "aircrafts.txt";

            IAirlineRepository repository = new FileAirlineRepository(filePath);
            var aircrafts = repository.LoadAircrafts();

            IAirlinesService service = new AirlineService(aircrafts);
            ConsoleUI ui = new ConsoleUI(service);

            ui.Run();
        }
    }
}