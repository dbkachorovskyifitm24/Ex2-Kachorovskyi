using System;
using System.Collections.Generic;

public sealed class Logger
{
    private static Logger instance;
    private static readonly object lockObj = new object();
    private List<string> logs;

    private Logger()
    {
        logs = new List<string>();
    }

    public static Logger Instance
    {
        get
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }
    }

    public void Log(string message)
    {
        string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        logs.Add(entry);
        Console.WriteLine(entry);
    }

    public List<string> GetLogs()
    {
        return logs;
    }
}

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string message) : base(message)
    {
    }
}

public class BankAccount
{
    public string AccountNumber { get; private set; }
    public decimal Balance { get; private set; }

    public BankAccount(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
        Logger.Instance.Log($"Рахунок {AccountNumber} створено з балансом {Balance}");
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сума депозиту повинна бути більше нуля");
        }

        Balance += amount;
        Logger.Instance.Log($"Рахунок {AccountNumber}: депозит {amount}, новий баланс {Balance}");
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сума зняття повинна бути більше нуля");
        }

        if (amount > Balance)
        {
            Logger.Instance.Log($"Рахунок {AccountNumber}: спроба зняти {amount} при балансі {Balance} - відмовлено");
            throw new InsufficientFundsException($"Недостатньо коштів на рахунку {AccountNumber}. Баланс: {Balance}, запитано: {amount}");
        }

        Balance -= amount;
        Logger.Instance.Log($"Рахунок {AccountNumber}: зняття {amount}, новий баланс {Balance}");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        BankAccount account = new BankAccount("UA123456789", 1000m);

        account.Deposit(500m);
        account.Withdraw(300m);

        try
        {
            account.Withdraw(5000m);
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        Console.WriteLine($"Фінальний баланс рахунку {account.AccountNumber}: {account.Balance}");

        Console.WriteLine("\nІсторія логів:");
        foreach (string log in Logger.Instance.GetLogs())
        {
            Console.WriteLine(log);
        }
    }
}