using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication6
{
    class Car : IComparable//, IComparable<Car>
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double Speed { get; set; }
        public decimal Price { get; set; }

        int IComparable.CompareTo(object obj)
        {
            Car otherother = obj as Car;
            if (otherother == null)
                throw new ArgumentException("obj");
            return CompareTo(other: otherother);
        }

        int CompareTo(Car other)
        {
            return Speed.CompareTo(other.Speed);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} goes {2} km/h, price {3}$", Manufacturer, Model, Speed, Price);
        }
    }
}
