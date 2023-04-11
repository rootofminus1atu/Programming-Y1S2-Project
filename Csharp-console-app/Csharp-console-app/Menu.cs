using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Csharp_console_app
{
    internal class Menu
    {
        public Dictionary<int, MenuOption> Options { get; set; }

        public Menu(params MenuOption[] options)
        {
            Options = new Dictionary<int, MenuOption>();

            for (int i = 0; i < options.Length; i++)
                Options.Add(i + 1, options[i]);

            Options.Add(options.Length + 1, new MenuOption("Exit", () => { }));
        }

        

        public void Display()
        {
            foreach (var (num, option) in Options)
                Console.WriteLine($"{num}. {option.Name}");
        }

        public void Run()
        {
            string choice = "";

            while (choice != $"{Options.Count}")
            {
                this.Display();

                Console.Write("Pick a menu optiooooon: ");
                choice = Console.ReadLine();

                try
                {
                    Options[int.Parse(choice)].Action();
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Invalid choice");
                }
                
            }

 
        }
    }

    public class MenuOption
    {
        public string Name { get; set; }
        public Action Action { get; set; }

        // the menu options could have additional parameters for a "success" and "fail" message


        public MenuOption(string name, Action action)
        {
            Name = name;
            Action = action;
        }


    }
}
