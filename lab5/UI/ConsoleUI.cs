using System;
using AirlineProject.Services;

namespace AirlineProject.UI
{
    public class ConsoleUI
    {
        private readonly IAirlinesService _service;

        public ConsoleUI(IAirlinesService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== АВИАКОМПАНИЯ (АВИАПАРК) ===");
                Console.WriteLine("1. Показать весь список самолетов");
                Console.WriteLine("2. Показать общую вместимость и грузоподъемность");
                Console.WriteLine("3. Сортировать самолеты по дальности полета");
                Console.WriteLine("4. Найти самолет по диапазону потребления топлива");
                Console.WriteLine("0. Выход");
                Console.Write("\nВыберите пункт меню: ");

                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        PrintAircrafts(_service.GetAllAircrafts());
                        break;
                    case "2":
                        Console.WriteLine($"Общая пассажировместимость: {_service.GetTotalPassengerCapacity()} чел.");
                        Console.WriteLine($"Общая грузоподъемность: {_service.GetTotalCargoCapacity()} т");
                        break;
                    case "3":
                        _service.SortByFlightRange();
                        Console.WriteLine("Самолеты успешно отсортированы!");
                        PrintAircrafts(_service.GetAllAircrafts());
                        break;
                    case "4":
                        Console.Write("Минимальный расход (кг/ч): ");
                        double min = double.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Максимальный расход (кг/ч): ");
                        double max = double.Parse(Console.ReadLine() ?? "0");
                        PrintAircrafts(_service.FindByFuelConsumption(min, max));
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private void PrintAircrafts(System.Collections.Generic.List<AirlineProject.Models.Aircraft> aircrafts)
        {
            if (aircrafts == null || aircrafts.Count == 0)
            {
                Console.WriteLine("Список пуст.");
                return;
            }

            foreach (var a in aircrafts)
            {
                Console.WriteLine(a);
            }
        }
    }
}