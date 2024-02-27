using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ToDoListApp
{
    class Program
    {
        static List<Task> tasks = new List<Task>();
        const string filePath = "tasks.json";

        static void Main(string[] args)
        {
            LoadTasks(); // Load tasks from file if available

            while (true)
            {
                Console.WriteLine("\nToDo List Application");
                Console.WriteLine("1. View Tasks");
                Console.WriteLine("2. Add Task");
                Console.WriteLine("3. Mark Task as Completed");
                Console.WriteLine("4. Delete Task");
                Console.WriteLine("5. Exit");
                Console.Write("\nEnter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewTasks();
                        break;
                    case "2":
                        AddTask();
                        break;
                    case "3":
                        MarkTaskAsCompleted();
                        break;
                    case "4":
                        DeleteTask();
                        break;
                    case "5":
                        SaveTasks(); // Save tasks to file before exiting
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void LoadTasks()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                tasks = JsonConvert.DeserializeObject<List<Task>>(json);
            }
        }

        static void SaveTasks()
        {
            string json = JsonConvert.SerializeObject(tasks);
            File.WriteAllText(filePath, json);
        }

        static void ViewTasks()
        {
            if (tasks != null && tasks.Count > 0)
            {
                Console.WriteLine("\nTasks:");
                foreach (var task in tasks)
                {
                    Console.WriteLine($"{task.Title} - {task.Description} - Due: {task.DueDate.ToShortDateString()} - Status: {(task.Completed ? "Completed" : "Pending")}");
                }
            }
            else
            {
                Console.WriteLine("No tasks found.");
            }
            Console.WriteLine();
        }

        static void AddTask()
        {
            Console.Write("Enter task name: ");
            string title = Console.ReadLine();
            Console.Write("Enter task description: ");
            string description = Console.ReadLine();
            Console.Write("Enter due date (MM/DD/YYYY): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
            {
                if (tasks == null)
                {
                    tasks = new List<Task>(); // Initialize tasks if it's null
                }

                tasks.Add(new Task { Title = title, Description = description, DueDate = dueDate, Completed = false });
                Console.WriteLine("Task added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid due date format. Task not added.");
            }
        }

        static void MarkTaskAsCompleted()
        {
            Console.Write("Enter task name to mark as completed: ");
            string title = Console.ReadLine();
            Task task = tasks.Find(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (task != null)
            {
                task.Completed = true;
                Console.WriteLine("Task marked as completed.");
            }
            else
            {
                Console.WriteLine("Task not found.");
            }
        }

        static void DeleteTask()
        {
            Console.Write("Enter task name to delete: ");
            string title = Console.ReadLine();
            Task task = tasks.Find(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (task != null)
            {
                tasks.Remove(task);
                Console.WriteLine("Task deleted successfully.");
            }
            else
            {
                Console.WriteLine("Task not found.");
            }
        }
    }

    class Task
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
    }
}

