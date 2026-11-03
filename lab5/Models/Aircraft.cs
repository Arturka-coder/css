using System;

namespace AirlineProject.Models
{
    public abstract class Aircraft : IComparable<Aircraft>
    {
        public string Model { get; set; }
        public double FlightRange { get; set; }       
        public int PassengerCapacity { get; set; }    
        public double CargoCapacity { get; set; }      
        public double FuelConsumption { get; set; }    
        protected Aircraft(string model, double flightRange, int passengerCapacity, double cargoCapacity, double fuelConsumption)
        {
            Model = model;
            FlightRange = flightRange;
            PassengerCapacity = passengerCapacity;
            CargoCapacity = cargoCapacity;
            FuelConsumption = fuelConsumption;
        }

        public int CompareTo(Aircraft? other)
        {
            if (other == null) return 1;
            return FlightRange.CompareTo(other.FlightRange);
        }

        public override string ToString() => 
            $"{Model,-25} | Дальность: {FlightRange,5} км | Пассажиры: {PassengerCapacity,3} | Груз: {CargoCapacity,5} т | Расход: {FuelConsumption,5} кг/ч";
    }
}
