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
        public bool WithExit;
        public string Prompt;
        // this could have additional properties such as startText, endText, etc.
        // and additional methods for adding special messages

        // for optional arguments 
        // probably not a good implementation considering how named and params arguments work
        public Menu(params MenuOption[] options) : this(withExit: true, prompt: ">", options) { }
        public Menu(bool withExit, params MenuOption[] options) : this(withExit: withExit, prompt: ">", options) { }
        public Menu(string prompt, params MenuOption[] options) : this(withExit: true, prompt: prompt, options) { }

        public Menu(bool withExit = true, string prompt = ">", params MenuOption[] options)
        {
            Options = new Dictionary<int, MenuOption>();

            for (int i = 0; i < options.Length; i++)
                Options.Add(i + 1, options[i]);


            WithExit = withExit;

            if (withExit)
                Options.Add(options.Length + 1, new MenuOption("Exit", () => { }));


            Prompt = prompt;
        }



        public void DisplayOptions()
        {
            Console.WriteLine("");

            foreach (var (num, option) in Options)
                Console.WriteLine($"{num}. {option.Name}");

            Console.WriteLine("");
        }


        public void Run()
        {
            string choice = "";

            while (choice != $"{Options.Count}")
            {
                choice = "";
                int choiceNum;

                this.DisplayOptions();

                while(!(int.TryParse(choice, out choiceNum) && Options.ContainsKey(choiceNum)))
                {
                    Console.Write($"{Prompt} ");
                    choice = Console.ReadLine();
                }

                Options[choiceNum].Action();


                if (!WithExit)
                    break;
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
