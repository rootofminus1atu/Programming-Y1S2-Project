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
        public List<MenuOption> Options { get; set; }

        public Menu(params MenuOption[] options)
        {
            Options = new List<MenuOption>();

            foreach (MenuOption option in options)
                Options.Add(option);

            Options.Add(new MenuOption("Exit", () => { }));
        }

        public void Display()
        {
            for (int i = 0; i < Options.Count; i++)
                Console.WriteLine($"{i + 1}. {Options[i].Name}");
        }

        public void Run()
        {
            string choice = "";

            while (choice != $"{Options.Count}")
            {
                this.Display();

                Console.Write("Pick a menu optiooooon: ");
                choice = Console.ReadLine();

                Options[int.Parse(choice) - 1].Action();
            }

 
        }
    }

    public class MenuOption
    {
        public string Name { get; set; }
        public Action Action { get; set; }

        // the menu optios could have additional parameters for a "success" and "fail" message


        public MenuOption(string name, Action action)
        {
            Name = name;
            Action = action;
        }


    }
}
