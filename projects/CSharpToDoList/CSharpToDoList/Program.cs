using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpToDoList
{
    internal class Program
    {
        enum Screen
        {
            Title,
            View,
            Add,
            Mark,
            Delete,
            Clear,
            End
        };

        static void Main(string[] args)
        {
            string username = Environment.UserName;
            string fileName = "myTasks.txt";
            List<Task> tasks = LoadTasksFromFile();
            string input = "";
            Screen activeScreen = Screen.Title;

            List<Screen> titleOptions = new List<Screen>
            {
                Screen.View,
                Screen.Add,
                Screen.Mark,
                Screen.Delete,
                Screen.Clear,
                Screen.End
            };

            Console.WriteLine("Welcome to the To-Do List!");

            while (activeScreen != Screen.End)
            {
                while (activeScreen == Screen.Title)
                {
                    Console.WriteLine("======= TO-DO LIST =======");
                    Console.WriteLine("1. View Tasks");
                    Console.WriteLine("2. Add Task");
                    Console.WriteLine("3. Mark Task Completed");
                    Console.WriteLine("4. Delete Task");
                    Console.WriteLine("5. Clear All Tasks");
                    Console.WriteLine("6. Save & Exit");
                    Console.WriteLine("==========================");
                    Console.WriteLine("Choose an option.");
                    PromptUser(ref input);
                    activeScreen = HandleMenuInput(input, titleOptions, activeScreen);
                }

                while (activeScreen == Screen.View)
                {
                    Console.WriteLine("======= YOUR TASKS =======");
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine("No tasks yet!");
                    }
                    else
                    {
                        for (int i = 0; i < tasks.Count; i++)
                        {
                            string filler = tasks[i].completed ? "X" : " ";
                            Console.WriteLine($"{i + 1}. [{filler}] {tasks[i].description}");
                        }
                    }
                    activeScreen = Screen.Title;
                }

                while (activeScreen == Screen.Add)
                {
                    Console.WriteLine("==========================");
                    Console.WriteLine("Enter the new task: ");
                    PromptUser(ref input);
                    tasks.Add(new Task(input, false));
                    Console.WriteLine("Task added!");
                    activeScreen = Screen.Title;
                }

                while (activeScreen == Screen.Mark)
                {
                    Console.WriteLine("==========================");
                    Console.WriteLine("Enter the task number you wish to mark as completed.");
                    PromptUser(ref input);
                    activeScreen = MarkComplete(input, tasks);
                }

                while (activeScreen == Screen.Delete)
                {
                    Console.WriteLine("==========================");
                    Console.WriteLine("Enter the task number you wish to delete.");
                    PromptUser(ref input);
                    activeScreen = DeleteTask(input, tasks);
                }

                while (activeScreen == Screen.Clear)
                {
                    Console.WriteLine("Are you sure you want to clear all tasks? (y/n):");
                    PromptUser(ref input);
                    if (input == "y")
                    {
                        tasks.Clear();
                        Console.WriteLine("Tasks cleared!");
                    }
                    else if (input == "n")
                    {
                        Console.WriteLine("Clear cancelled.");
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid option.");
                        activeScreen = Screen.Clear;
                        continue;
                    }
                    activeScreen = Screen.Title;
                }
            }

            SaveTasksToFile(tasks);
            Console.WriteLine("Thank you for using the To-Do List. Goodbye!");

            // ====================
            // LOCAL FUNCTIONS
            // ====================

            List<Task> LoadTasksFromFile()
            {
                if (!File.Exists(fileName))
                {
                    using (File.Create(fileName)) { }
                    return new List<Task>();
                }

                string[] readFile = File.ReadAllLines(fileName);
                List<Task> loaded = new List<Task>();

                foreach (var line in readFile)
                {
                    string decoded = Decode(line);
                    if (string.IsNullOrWhiteSpace(decoded) || !decoded.Contains("|")) continue;

                    string description = decoded.Substring(0, decoded.IndexOf('|'));
                    string completedStr = decoded.Substring(decoded.IndexOf('|') + 1);
                    bool completed = (completedStr == "1");

                    loaded.Add(new Task(description, completed));
                }

                return loaded;
            }

            void SaveTasksToFile(List<Task> tasks)
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    foreach (Task task in tasks)
                    {
                        writer.WriteLine(Encode(task.GetTaskString()));
                    }
                }
            }

            Screen HandleMenuInput(string input, List<Screen> options, Screen currentScreen)
            {
                try
                {
                    int choice = Convert.ToInt32(input);
                    if (choice >= 1 && choice <= options.Count)
                    {
                        return options[choice - 1];
                    }
                }
                catch { }
                Console.WriteLine("Please enter a valid option.");
                return currentScreen;
            }

            Screen MarkComplete(string input, List<Task> tasks)
            {
                if (tasks.Count == 0)
                {
                    Console.WriteLine("No tasks to mark as complete.");
                    return Screen.Title;
                }

                if (tasks.All(t => t.completed))
                {
                    Console.WriteLine("No incomplete tasks to mark.");
                    return Screen.Title;
                }

                try
                {
                    int choice = Convert.ToInt32(input);
                    if (choice >= 1 && choice <= tasks.Count)
                    {
                        if (tasks[choice - 1].completed)
                        {
                            Console.WriteLine("That task is already marked complete.");
                        }
                        else
                        {
                            tasks[choice - 1].MarkComplete();
                            Console.WriteLine($"Task {choice} marked as complete!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid task number.");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid task number.");
                }

                return Screen.Title;
            }

            Screen DeleteTask(string input, List<Task> tasks)
            {
                if (tasks.Count == 0)
                {
                    Console.WriteLine("No tasks to delete.");
                    return Screen.Title;
                }

                try
                {
                    int choice = Convert.ToInt32(input);
                    if (choice >= 1 && choice <= tasks.Count)
                    {
                        tasks.RemoveAt(choice - 1);
                        Console.WriteLine($"Task {choice} deleted.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid task number.");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid task number.");
                }

                return Screen.Title;
            }

            void PromptUser(ref string input)
            {
                Console.Write($"<{username}>: ");
                input = Console.ReadLine();
            }

            string Encode(object input)
            {
                string stringedInput = input.ToString();
                int n = 8;
                StringBuilder sb = new StringBuilder();
                foreach (char c in stringedInput.ToCharArray())
                {
                    sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
                }
                string r1 = sb.ToString();
                string r2 = string.Join(string.Empty, r1.Select((x, i) => i > 0 && i % n == 0 ? $" {x}" : x.ToString()));
                var plainTextBytes = Encoding.UTF8.GetBytes(r2);
                string r3 = Convert.ToBase64String(plainTextBytes);
                StringBuilder sb2 = new StringBuilder();
                foreach (char c in r3.ToCharArray())
                {
                    sb2.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
                }
                string r4 = sb2.ToString();
                string r5 = string.Join(string.Empty, r4.Select((x, i) => i > 0 && i % n == 0 ? $" {x}" : x.ToString()));
                return r5;
            }

            string Decode(string input)
            {
                string r1 = input.Replace(" ", string.Empty);
                List<byte> byteList = new List<byte>();
                for (int i = 0; i < r1.Length; i += 8)
                {
                    byteList.Add(Convert.ToByte(r1.Substring(i, 8), 2));
                }
                string r2 = Encoding.ASCII.GetString(byteList.ToArray());
                var base64EncodedBytes = Convert.FromBase64String(r2);
                string r3 = Encoding.UTF8.GetString(base64EncodedBytes);
                string r4 = r3.Replace(" ", string.Empty);
                List<byte> byteList2 = new List<byte>();
                for (int i = 0; i < r4.Length; i += 8)
                {
                    byteList2.Add(Convert.ToByte(r4.Substring(i, 8), 2));
                }
                return Encoding.ASCII.GetString(byteList2.ToArray());
            }
        }

        class Task
        {
            public string description;
            public bool completed;

            public Task(string _description, bool _completed)
            {
                description = _description;
                completed = _completed;
            }

            public void MarkComplete() => completed = true;

            public int GetCompletedAsInt() => (completed ? 1 : 0);

            public string GetTaskString() => description + "|" + GetCompletedAsInt();
        };
    }
}
