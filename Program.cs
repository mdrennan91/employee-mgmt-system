using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

// this holds info about one employee
class Employee
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Role { get; set; }
    public Department Department { get; set; }
}

// simple struct for the department name
struct Department
{
    public string Name { get; set; }

    [JsonConstructor]
    public Department(string name)
    {
        Name = name;
    }
}

class Program
{
    // list to hold all employees
    static List<Employee> employees = new List<Employee>();
    
    // keeps track of the next ID we should assign
    static int nextId = 1;

    // this is the name of the file we’ll load/save to
    const string FilePath = "employees.json";

    static void Main()
    {
        LoadEmployees(); // try to load existing employee data

        bool running = true;

        while (running)
        {
            // main menu
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

        SaveEmployees(); // write everything to the file before quitting
        Console.WriteLine("Goodbye!");
    }

    // add a new employee to the list
    static void AddEmployee()
    {
        Console.Write("Enter employee name: ");
        string? name = Console.ReadLine();

        Console.Write("Enter employee role: ");
        string? role = Console.ReadLine();

        Console.Write("Enter department name: ");
        string? deptName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(deptName))
        {
            Console.WriteLine("Name, role, and department cannot be empty.");
            return;
        }

        var department = new Department(deptName);
        employees.Add(new Employee { Id = nextId++, Name = name, Role = role, Department = department });
        Console.WriteLine("Employee added.");
    }

    // show all employees in the list
    static void ViewEmployees()
    {
        Console.WriteLine("\n--- Employee List ---");
        foreach (var emp in employees)
        {
            Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Role: {emp.Role}, Department: {emp.Department.Name}");
        }
    }

    // update an existing employee's info
    static void EditEmployee()
    {
        Console.Write("Enter employee ID to edit: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var emp = employees.Find(e => e.Id == id);
            if (emp != null)
            {
                Console.Write("Enter new name (leave blank to keep current): ");
                string? newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName)) emp.Name = newName;

                Console.Write("Enter new role (leave blank to keep current): ");
                string? newRole = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newRole)) emp.Role = newRole;

                Console.Write("Enter new department (leave blank to keep current): ");
                string? newDept = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newDept)) emp.Department = new Department(newDept);

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

    // remove an employee from the list
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

    // try to read data from the json file
    static void LoadEmployees()
    {
        if (!File.Exists(FilePath))
            return;

        try
        {
            string json = File.ReadAllText(FilePath);

            if (string.IsNullOrWhiteSpace(json))
                return;

            var loadedEmployees = JsonSerializer.Deserialize<List<Employee>>(json);
            if (loadedEmployees != null)
            {
                employees = loadedEmployees;
                nextId = employees.Count > 0 ? employees[^1].Id + 1 : 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Failed to load employee data. {ex.Message}");
        }
    }

    // save data to the json file
    static void SaveEmployees()
    {
        string json = JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}