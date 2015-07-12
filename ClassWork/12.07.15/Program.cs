using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication6
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = new List<Car>
            {
            	new Car { Manufacturer = "Ferrari", Model = "La Ferrari", Speed = 340.0, Price = 5e5m },
                new Car { Manufacturer = "Lamborgini", Model = "Countach", Speed = 333d, Price = 21e4m },
            	new Car { Manufacturer = "Ferrari", Model = "F50", Speed = 329.0, Price = 235e3m },
                new Car { Manufacturer = "Lamborgini", Model = "Diablo", Speed = 350d, Price = 244e3m },
            };

            var comparer = new CarComparer(false);

            //Array.Sort(cars, comparer);

            var query = from car in cars
                        where car.Speed > 330.0
                        select new
                        {
                            Name = car.Manufacturer + "\"" + car.Model + "\"",
                            Speed = car.Speed,
                            IsModified = true
                        };
            var sameQuery = cars.Where(c => c.Speed > 330.0).Select(c => new
            {
                Name = c.Manufacturer + "\"" + c.Model + "\"",
                Speed = c.Speed,
                IsModified = true
            });

            var groupQuery = from car in cars
                             group car by car.Manufacturer into carsByManufacturer
                             select carsByManufacturer;

            cars.Add(new Car { Manufacturer = "Alpha Romeo", Model = "A350", Speed = 331.1, Price = 325e3m });

            //foreach (var item in query)
            //{
            //    Console.WriteLine("{0} {1}", item.Name, item.Speed);
            //}

            var groupQueryResult = groupQuery.ToList();
//            foreach (var carGroup in groupQueryResult)
//            {
//                Console.Write(carGroup.Key);
//                Console.Write(" Total = ");
////                Console.WriteLine(carGroup.Sum(c => c.Price));
//                Console.WriteLine(carGroup.Aggregate(0m, (acc, car) => acc += car.Price));
//                foreach (var car in carGroup)
//                {
//                    Console.Write("\t");
//                    Console.WriteLine(car);
//                }
//            }
            foreach (var car in groupQueryResult.SelectMany(group => group))
            {
                Console.WriteLine(car);
            }


            //foreach (var value in IteratorBuilder(10))
            //{
            //    Debug.WriteLine("Foreach " + value.ToString());
            //    Console.WriteLine(value);
            //}

            Console.ReadKey();
        }

        private static IEnumerable<int> IteratorBuilder(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Debug.WriteLine("Yield " + i.ToString());
                yield return i;
            }
        }
    }

    internal class CarComparer : IComparer<Car>
    {
        public bool Forward { get; private set; }

        public CarComparer(bool forward = true)
        {
            Forward = forward;
        }

        public int Compare(Car x, Car y)
        {
            int result = x.Speed.CompareTo(y.Speed);
            return Forward ? result : -result;
        }

        //public int Compare(Car x, Car y)
        //{
        //    uint sign = Convert.ToUInt32(Forward);
        //    sign <<= 32;

        //    int result = x.Speed.CompareTo(y.Speed);
        //    result &= (int)sign;
        //    return Forward ? result : -result;
        //}
    }
}
