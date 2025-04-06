class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Работа с классами SymbolFields и Book");

        // Тестирование конструкторов и методов SymbolFields
        Console.WriteLine("\nТестирование SymbolFields:");
        SymbolFields sfDefault = new SymbolFields();
        Console.WriteLine("Конструктор по умолчанию: " + sfDefault);

        char field1 = Input_Validation.InputValidation.InputCharWithValidation("Введите первый символ: ");
        char field2 = Input_Validation.InputValidation.InputCharWithValidation("Введите второй символ: ");
        SymbolFields sfParams = new SymbolFields(field1, field2);
        Console.WriteLine("Конструктор с параметрами: " + sfParams);

        SymbolFields sfCopy = new SymbolFields(sfParams);
        Console.WriteLine("Конструктор копирования: " + sfCopy);

        // Тестирование конструкторов и методов Book
        Console.WriteLine("\nТестирование Book:");
        Book bookDefault = new Book();
        Console.WriteLine("Стандартный конструктор:\n" + bookDefault);

        string title = Input_Validation.InputValidation.InputStringWithValidation("Введите название книги: ");
        string author = Input_Validation.InputValidation.InputStringWithValidation("Введите автора книги (например, Иванов В.И.): ");
        Book bookParams = new Book('X', 'Y', title, author);
        Console.WriteLine("\nКонструктор с параметрами:\n" + bookParams);

        Book bookCopy = new Book(bookParams);
        Console.WriteLine("\nКонструктор копирования:\n" + bookCopy);

        // Изменение названия и добавление инициалов автора
        bookCopy.ChangeTitle("Новое название");
        Console.WriteLine("\nПосле смены названия:\n" + bookCopy);

        bookCopy.AddAuthorInitialsToFields();
        Console.WriteLine("\nПосле добавления инициалов автора к полям:\n" + bookCopy);

        Console.ReadLine();
    }
}




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Базовый класс с двумя символьными полями
public class SymbolFields
{
    public char Field1 { get; set; }
    public char Field2 { get; set; }

    // Стандартный конструктор
    public SymbolFields()
    {
        Field1 = 'A';
        Field2 = 'B';
    }

    // Конструктор с параметрами
    public SymbolFields(char field1, char field2)
    {
        Field1 = field1;
        Field2 = field2;
    }

    // Конструктор копирования
    public SymbolFields(SymbolFields other)
    {
        Field1 = other.Field1;
        Field2 = other.Field2;
    }

    // Метод, создающий строку из полей
    public string CreateStringFromFields()
    {
        return $"Поле 1: {Field1}, Поле 2: {Field2}";
    }

    // Перегрузка метода ToString()
    public override string ToString()
    {
        return CreateStringFromFields();
    }
}





using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Дочерний класс Book
public class Book : SymbolFields
{
    public string Title { get; set; }
    public string Author { get; set; }

    // Стандартный конструктор
    public Book() : base()
    {
        Title = "Без названия";
        Author = "Без автора";
    }

    // Конструктор с параметрами
    public Book(char field1, char field2, string title, string author) : base(field1, field2)
    {
        Title = title;
        Author = author;
    }

    // Конструктор копирования
    public Book(Book other) : base(other)
    {
        Title = other.Title;
        Author = other.Author;
    }

    // Метод для вывода информации о книге
    public void PrintBookInfo()
    {
        Console.WriteLine(this.ToString());
    }

    // Перегрузка ToString() для Book
    public override string ToString()
    {
        return $"Книга: {Title}, Автор: {Author}, Поля: {base.ToString()}";
    }

    // Метод для изменения названия книги
    public void ChangeTitle(string newTitle)
    {
        Title = newTitle;
    }


    // Метод для добавления инициалов автора к полям класса SymbolFields
    public void AddAuthorInitialsToFields()
    {
        // Разбор строки с автором (через пробел или точки)
        string[] authorParts = Author.Replace('.', ' ').Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (authorParts.Length < 2)
        {
            Console.WriteLine("Не удалось сформировать инициалы (слишком мало данных об авторе).");
            return;
        }

        // Универсальный способ получения инициалов (только со 2-го слова и далее)
        string initials = "";
        for (int i = 1; i < authorParts.Length; i++) // Начиная со второго слова (имени)
        {
            if (authorParts[i].Length > 0)
            {
                initials += authorParts[i][0] + ".";
            }
        }

        // Коррекция индексов для Field1 и Field2
        Field1 = initials.Length > 0 ? initials[0] : '?';
        Field2 = initials.Length > 2 ? initials[2] : (initials.Length > 1 ? initials[1] : '?');
    }
}







using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Класс для проверки ввода
namespace Input_Validation
{
    public static class InputValidation
    {
        public static double InputDoubleWithValidation(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пустой ввод. Повторите ввод.");
                    Console.ResetColor();
                    continue;
                }
                if (input == "0" || input == "0,0")
                {
                    return 0.0;
                }
                if (double.TryParse(input, out double inputNumber))
                {
                    if (input.StartsWith("0") && !input.StartsWith("0,"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Недопустимый ввод. Число не может начинаться с нуля, если оно не равно нулю (за исключением 0 и 0,x).");
                        Console.ResetColor();
                        continue;
                    }
                    if ((input.StartsWith("-0") && !input.StartsWith("-0,")) || input == "-0")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Недопустимый ввод. Число не может начинаться с нуля, если оно не равно нулю (за исключением -0,x).");
                        Console.ResetColor();
                        continue;
                    }
                    return inputNumber;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неверный формат числа. Повторите ввод.");
                Console.ResetColor();
            }
        }

        public static string InputStringWithValidation(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пустой ввод. Повторите ввод.");
                    Console.ResetColor();
                    continue;
                }
                return input.Trim();
            }
        }

        public static char InputCharWithValidation(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || input.Length != 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Необходимо ввести ровно один символ. Повторите ввод.");
                    Console.ResetColor();
                    continue;
                }
                return input[0];
            }
        }
    }
}
