using System;
using System.Collections.Generic;

namespace task4
{
    
    public abstract class Employee
    {
        
        public int Id { get; private set; }
        public string Name { get; private set; }
        protected decimal BaseSalary { get; set; }

        protected Employee(int id, string name, decimal baseSalary)
        {
            Id = id;
            Name = name;
            BaseSalary = baseSalary;
        }

        public abstract decimal CalculateSalary();

        protected decimal ApplyTax(decimal amount)
        {
            return amount * 0.90m;
        }

        internal string GetDepartmentCode()
        {
            return "DEPT-OOP-2026";
        }
    }

    #region full time
    public class FullTimeEmployee : Employee
    {
        public decimal Bonus { get; set; }

        public FullTimeEmployee(int id, string name, decimal baseSalary, decimal bonus)
            : base(id, name, baseSalary)
        {
            Bonus = bonus;
        }

        public override decimal CalculateSalary()
        {
            decimal rawSalary = BaseSalary + Bonus;
            return ApplyTax(rawSalary);
        }
    }
    #endregion

    #region part time
    public class PartTimeEmployee : Employee
    {
        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }

        public PartTimeEmployee(int id, string name, decimal hoursWorked, decimal hourlyRate)
            : base(id, name, 0)
        {
            HoursWorked = hoursWorked;
            HourlyRate = hourlyRate;
        }

        public override decimal CalculateSalary()
        {
            decimal rawSalary = HoursWorked * HourlyRate;
            return ApplyTax(rawSalary);
        }
    }

    #endregion
    #region Freelancer
    public class Freelancer : Employee
    {
        public decimal TotalProjectFees { get; set; }

        public Freelancer(int id, string name, decimal totalProjectFees)
            : base(id, name, 0)
        {
            TotalProjectFees = totalProjectFees;
        }

        public override decimal CalculateSalary()
        {
            return ApplyTax(TotalProjectFees);
        }
    } 
    #endregion

    // 5. Payroll Class
    public class Payroll
    {
        private List<Employee> employees = new List<Employee>();

        public void AddEmployee(Employee emp)
        {
            employees.Add(emp);
            Console.WriteLine(" Employee added successfully.");
        }

        public void RemoveEmployee(int id)
        {
            Employee empToRemove = employees.Find(e => e.Id == id);
            if (empToRemove != null)
            {
                employees.Remove(empToRemove);
                Console.WriteLine(" Employee removed successfully.");
            }
            else
            {
                Console.WriteLine(" Employee ID not found.");
            }
        }

        public void ShowPayrollReport()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine(" No employees in the payroll yet.");
                return;
            }

            Employee highestPaid = null;
            decimal maxSalary = decimal.MinValue;

            foreach (var emp in employees)
            {
                decimal netSalary = emp.CalculateSalary();
                string typeStr = "";

                if (emp is FullTimeEmployee) typeStr = "Full-Time";
                else if (emp is PartTimeEmployee) typeStr = "Part-Time";
                else if (emp is Freelancer) typeStr = "Freelancer";

                Console.WriteLine($"{emp.Id,-4} {emp.Name,-9} {typeStr,-11} {netSalary,10:F2}");

                if (netSalary > maxSalary)
                {
                    maxSalary = netSalary;
                    highestPaid = emp;
                }
            }

            if (highestPaid != null)
            {
                Console.WriteLine($"Highest paid: {highestPaid.Name} ({maxSalary:F2})");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Payroll payroll = new Payroll();
            payroll.AddEmployee(new FullTimeEmployee(1, "Ahmed", 9000m, 0m)); // 9000 - 10% = 8100
            payroll.AddEmployee(new PartTimeEmployee(2, "Mona", 50m, 70m));     // 3500 - 10% = 3150 (50*70=3500)
            payroll.AddEmployee(new Freelancer(3, "Karim", 6000m));             // 6000 - 10% = 5400

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("    CONSOL PAINEL MENU");
                Console.WriteLine("1. Add employee");
                Console.WriteLine("2. Show payroll report");
                Console.WriteLine("3. Show highest paid");
                Console.WriteLine("4. Remove employee");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option (1-5): ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddNewEmployeePrompt(payroll);
                        break;
                    case "2":
                        payroll.ShowPayrollReport();
                        break;
                    case "3":
                        payroll.ShowPayrollReport(); // Built into report logic as requested
                        break;
                    case "4":
                        Console.Write("Enter Employee ID to remove: ");
                        if (int.TryParse(Console.ReadLine(), out int removeId))
                        {
                            payroll.RemoveEmployee(removeId);
                        }
                        else
                        {
                            Console.WriteLine(" Invalid ID format.");
                        }
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Exiting program. Goodbye!");
                        break;
                    default:
                        Console.WriteLine(" Invalid option. Please choose between 1 and 5.");
                        break;
                }
            }
        }

        private static void AddNewEmployeePrompt(Payroll payroll)
        {
            Console.WriteLine("Select Employee Type:");
            Console.WriteLine("1. Full-Time Employee");
            Console.WriteLine("2. Part-Time Employee");
            Console.WriteLine("3. Freelancer");
            Console.Write("Choice: ");
            string typeChoice = Console.ReadLine();

            Console.Write("Enter ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine(" ID must be a positive integer.");
                return;
            }

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            if (typeChoice == "1")
            {
                Console.Write("Enter Base Salary: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal baseSalary) || baseSalary <= 0)
                {
                    Console.WriteLine(" Salary must be positive, otherwise show a clear message.");
                    return;
                }

                Console.Write("Enter Bonus: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal bonus) || bonus < 0)
                {
                    Console.WriteLine(" Bonus must be valid (>= 0).");
                    return;
                }

                payroll.AddEmployee(new FullTimeEmployee(id, name, baseSalary, bonus));
            }
            else if (typeChoice == "2")
            {
                Console.Write("Enter Hours Worked: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal hours) || hours <= 0)
                {
                    Console.WriteLine(" Hours must be positive.");
                    return;
                }

                Console.Write("Enter Hourly Rate: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal rate) || rate <= 0)
                {
                    Console.WriteLine(" Hourly Rate must be positive.");
                    return;
                }

                payroll.AddEmployee(new PartTimeEmployee(id, name, hours, rate));
            }
            else if (typeChoice == "3")
            {
                Console.Write("Enter Total Project Fees: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal fees) || fees <= 0)
                {
                    Console.WriteLine(" Project fees must be positive.");
                    return;
                }

                payroll.AddEmployee(new Freelancer(id, name, fees));
            }
            else
            {
                Console.WriteLine(" Invalid employee type selection.");
            }
        }
    }
}