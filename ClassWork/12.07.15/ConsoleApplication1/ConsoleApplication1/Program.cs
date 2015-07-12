using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            XDocument doc = XDocument.Load(@"Cars.xml");

            OutputNode(doc.Root);

            // Удалить первый элемент Cars
            XElement root = doc.Root;
            root.Elements().First().Remove(); //            root.RemoveChild(root.FirstChild);
            // Создать узлы элементов.
            var element = new XElement("Motorcycle",
                new XElement("Manufactured", "Harley-Davidson Motor Co. Inc."),
                new XElement("Model", "Harley 20J"),
                new XElement("Year", "1920"),
                new XElement("Color", "Olive"),
                new XElement("Engine", "37 HP"));
            // Присоединить узел bike к корневому узлу
            root.Add(element);
            // Сохранить измененный документ
            doc.Save(@"Motorcycle.xml");

            Console.ReadKey();
        }

        static void OutputNode(XElement node)
        {
            Console.WriteLine("Type={0}\tName={1}\tValue={2}", node.NodeType, node.Name, node.Value);
            if (node.HasAttributes)
            {
                foreach (var attr in node.Attributes())
                    Console.WriteLine("@Type= {0}\tName={1}\tValue={2}", attr.NodeType, attr.Name, attr.Value);
            }
            if (node.HasElements)
            {
                foreach (var child in node.Elements())
                    OutputNode(child);
            }
        }
    }
}
