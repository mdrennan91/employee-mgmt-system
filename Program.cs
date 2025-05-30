using System;
using System.Collections.Generic;

class Employee
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Role { get; set; }
}

class Program
{
    static List<Employee> employees = new List<Employee>();
    static int nextId = 1;

    static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- Employee Management System ---");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View Employees");
            Console.WriteLine("3. Edit Employee");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1": AddEmployee(); break;
                case "2": ViewEmployees(); break;
                case "3": EditEmployee(); break;
                case "4": DeleteEmployee(); break;
                case "5": running = false; break;
                case null: Console.WriteLine("No input received."); break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }

        Console.WriteLine("Goodbye!");
    }

    static void AddEmployee()
    {
        Console.Write("Enter employee name: ");
        string? name = Console.ReadLine();

        Console.Write("Enter employee role: ");
        string? role = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(role))
        {
            Console.WriteLine("Name and role cannot be empty.");
            return;
        }

        employees.Add(new Employee { Id = nextId++, Name = name, Role = role });
        Console.WriteLine("Employee added.");
    }

    static void ViewEmployees()
    {
        Console.WriteLine("\n--- Employee List ---");
        foreach (var emp in employees)
        {
            Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Role: {emp.Role}");
        }
    }

    static void EditEmployee()
    {
        Console.Write("Enter employee ID to edit: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var emp = employees.Find(e => e.Id == id);
            if (emp != null)
            {
                Console.Write("Enter new name: ");
                emp.Name = Console.ReadLine();

                Console.Write("Enter new role: ");
                emp.Role = Console.ReadLine();

                Console.WriteLine("Employee updated.");
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }
    
    static void DeleteEmployee()
    {
        Console.Write("Enter employee ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var emp = employees.Find(e => e.Id == id);
            if (emp != null)
            {
                employees.Remove(emp);
                Console.WriteLine("Employee deleted.");
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }
}