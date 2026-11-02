using System;
using System.Collections.Generic;
using System.IO;
using AirlineProject.Models;

namespace AirlineProject.Data
{
    public class FileAirlineRepository : IAirlineRepository
    {
        private readonly string _filePath;

        public FileAirlineRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<Aircraft> LoadAircrafts()
        {
            var aircrafts = new List<Aircraft>();

            if (!File.Exists(_filePath))
            {
                Console.WriteLine($"Файл {_filePath} не найден!");
                return aircrafts;
            }

            foreach (var line in File.ReadAllLines(_filePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(';');
                if (parts.Length < 6) continue;

                string type = parts[0].Trim();
                string model = parts[1].Trim();
                double range = double.Parse(parts[2]);
                int passengers = int.Parse(parts[3]);
                double cargo = double.Parse(parts[4]);
                double fuel = double.Parse(parts[5]);

                Aircraft aircraft = type switch
                {
                    "Passenger" => new PassengerAircraft(model, range, passengers, cargo, fuel),
                    "Cargo" => new CargoAircraft(model, range, passengers, cargo, fuel),
                    "Business" => new BusinessJet(model, range, passengers, cargo, fuel),
                    _ => throw new ArgumentException($"Неизвестный тип: {type}")
                };

                aircrafts.Add(aircraft);
            }

            return aircrafts;
        }
    }
}