/*
 *                            Menu.cs
 * 
 *  Here you can find the 2 classes responsible for creating menus
 *  
 */

namespace Csharp_console_app
{
    /// <summary>
    /// A simple Menu creator
    /// </summary>
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

        /// <summary>
        /// Creates a new Menu
        /// </summary>
        /// <param name="withExit">Whether an "Exit" option should be included in the menu</param>
        /// <param name="prompt">The prompt to display when waiting for user input</param>
        /// <param name="options">The menu options</param>
        public Menu(bool withExit, string prompt, params MenuOption[] options)
        {
            Options = new Dictionary<int, MenuOption>();

            for (int i = 0; i < options.Length; i++)
                Options.Add(i + 1, options[i]);


            WithExit = withExit;

            if (withExit)
                Options.Add(options.Length + 1, new MenuOption("Exit", () => { }));


            Prompt = prompt;
        }


        /// <summary>
        /// Displays the menu options to the console.
        /// </summary>
        public void DisplayOptions()
        {
            Console.WriteLine("");

            foreach (var (num, option) in Options)
                Console.WriteLine($"{num}. {option.Name}");

            Console.WriteLine("");
        }

        /// <summary>
        /// Runs the menu, repeatedly displaying the options and prompting the user for input until the user selects the "Exit" option (if enabled).
        /// </summary>
        public void Run()
        {
            string choice;
            int choiceNum;

            do
            {
                choice = "";
                DisplayOptions();

                while (!(int.TryParse(choice, out choiceNum) && Options.ContainsKey(choiceNum)))
                {
                    Console.Write($"{Prompt} ");
                    choice = Console.ReadLine();
                }

                Options[choiceNum].Action();
            }
            while (WithExit && choiceNum != Options.Count);
        }
    }

    /// <summary>
    /// Menu options just for the sake of the Menu class
    /// </summary>
    public class MenuOption
    {
        public string Name { get; set; }
        public Action Action { get; set; }

        public MenuOption(string name, Action action)
        {
            Name = name;
            Action = action;
        }
    }
}
