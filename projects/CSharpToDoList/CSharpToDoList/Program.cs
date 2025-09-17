using System;
using System.Linq;
using System.Threading;
using System.IO;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Collections.Generic;

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
            bool saved = true;
            Screen activeScreen = Screen.Title;
            List<Screen> titleOptions = new List<Screen> { Screen.View, Screen.Add, Screen.Mark, Screen.Delete, Screen.Clear, Screen.End };
            Console.WriteLine("Welcome to the To-Do List!");
            while (activeScreen != Screen.End)
            {
                while (activeScreen == Screen.Title)
                {
                    Console.WriteLine("======= TO-DO LIST =======");
                    Console.WriteLine("1. View Tasks");
                    Console.WriteLine("2. Add Tasks");
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
                        string filler;
                        for (int i = 0; i < tasks.Count; i++)
                        {
                            if (tasks[i].completed)
                            {
                                filler = "X";
                            }
                            else
                            {
                                filler = " ";
                            }
                            Console.WriteLine((i + 1) + ". [" + filler + "] " + tasks[i].description);
                        }
                    }
                    activeScreen = Screen.Title;
                }
                while (activeScreen == Screen.Add)
                {
                    Console.WriteLine("==========================");
                    Console.WriteLine("Enter the new task: ");
                    PromptUser(ref input);
                    Task newTask = new Task(input, false);
                    tasks.Add(newTask);
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
                    Console.WriteLine("============================");
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
                        activeScreen = Screen.Title;
                    }
                    else if (input == "n")
                    {
                        Console.WriteLine("Clear cancelled.");
                        activeScreen = Screen.Title;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid option.");
                        activeScreen = Screen.Clear;
                    }
                }
            }
            SaveTasksToFile(tasks);
            Console.WriteLine("Thank you for using the To-Do List. Goodbye!");

            List<Task> LoadTasksFromFile()
            {
                if (!File.Exists(fileName))
                {
                    File.Create(fileName);
                }
                string[] readFile = File.ReadAllLines(fileName);
                List<Task> tasks = new List<Task>();

                for (int i = 0; i < readFile.Count(); i++)
                {
                    string description = readFile[i].Substring(0, readFile[i].IndexOf('|'));
                    string completedStr = readFile[i].Substring(readFile[i].IndexOf('|') + 1);
                    bool completed = (completedStr == "1");
                    Task loadedTask = new Task(description, completed);
                    tasks.Add(loadedTask);
                }

                return tasks;
            }
            void SaveTasksToFile(List<Task> tasks)
            {
                StreamWriter writer = new StreamWriter(fileName);
                foreach (Task task in tasks)
                {
                    writer.WriteLine(task.GetTaskString());
                }
                writer.Close();
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
                    else
                    {
                        Console.WriteLine("Please enter a valid option.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid option.");
                }
                return currentScreen;
            }

            Screen MarkComplete(string input, List<Task> tasks)
            {
                if (tasks.Count != 0)
                {
                    int completedTasks = 0;
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        if (tasks[i].completed)
                        {
                            completedTasks++;
                        }
                        if (completedTasks == tasks.Count)
                        {
                            Console.WriteLine("No tasks to mark as complete.");
                            return Screen.Title;
                        }
                        else
                        {
                            try
                            {
                                int choice = Convert.ToInt32(input);
                                if (choice >= 1 && choice <= tasks.Count)
                                {
                                    if (tasks[choice - 1].completed)
                                    {
                                        Console.WriteLine("That task is already marked complete.");
                                        return Screen.Title;
                                    }
                                    List<Task> nTasks = tasks;
                                    nTasks[choice - 1].MarkComplete();
                                    tasks = nTasks;
                                    Console.WriteLine("Task " + choice + " marked as complete!");
                                    return Screen.Title;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid task number.");
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Invalid task number.");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No tasks to mark as complete.");
                }
                return Screen.Title;
            }

            Screen DeleteTask(string input, List<Task> tasks)
            {
                if (tasks.Count != 0)
                {
                    try
                    {
                        int choice = Convert.ToInt32(input);
                        if (choice >= 1 && choice <= tasks.Count)
                        {
                            for (int i = 0; i < tasks.Count; ++i)
                            {
                                if (i == choice - 1)
                                {
                                    tasks.Remove(tasks[i]);
                                }
                            }
                            Console.WriteLine("Task " + choice + " deleted.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid task number.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Invalid task number.");
                    }
                }

                else
                {
                    Console.WriteLine("No tasks to delete.");
                }
                return Screen.Title;
            }

            void PromptUser(ref string input)
            {
                Console.Write($"<{username}>: ");
                input = Console.ReadLine();
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
            public string GetDescription()
            {
                return description;
            }
            public void MarkComplete()
            {
                completed = true;
            }
            public int GetCompletedAsInt()
            {
                return (completed ? 1 : 0);
            }
            public string GetTaskString()
            {
                return description + "|" + GetCompletedAsInt();
            }
        };
    }
}
