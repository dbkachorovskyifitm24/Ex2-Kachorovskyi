# BankAccount + Singleton Logger (C#)

## Структура
- `Logger` — клас-сінглтон (thread-safe, через `lock`), що логує всі транзакції у консоль і зберігає історію в пам'яті.
- `InsufficientFundsException` — кастомний виняток, що кидається при спробі знятии суму, яка перевищує баланс.
- `BankAccount` — поля `AccountNumber`, `Balance`; методи `Deposit` та `Withdraw`, кожен з яких логує операцію через `Logger.Instance`.
- `Program.cs` — демонстрація роботи: депозит, успішне та невдале зняття коштів.
