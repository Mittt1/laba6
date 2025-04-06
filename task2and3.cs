class Program
{
    static void Main()
    {
        Console.WriteLine("Программа для работы с денежными суммами");

        Money amount1 = Money.ReadFromConsole();
        Money amount2 = Money.ReadFromConsole();

        // Примеры использования операторов
        Console.WriteLine("\nРезультаты операций:");
        Console.WriteLine($"Сумма 1: {amount1}");
        Console.WriteLine($"Сумма 2: {amount2}");
        Console.WriteLine($"Сумма 1 + Сумма 2: {amount1 + amount2}");
        Console.WriteLine($"Сумма 1 - Сумма 2: {amount1 - amount2}");
        Console.WriteLine($"Сумма 1 + 50 руб: {amount1 + 50}");
        Console.WriteLine($"100 руб - Сумма 1: {100 - amount1}");

        // Примеры унарных операторов
        Money incremented = ++amount1;
        Console.WriteLine($"\nУнарный ++: {incremented}");
        Money decremented = --amount2;
        Console.WriteLine($"Унарный --: {decremented}");

        // Примеры приведения типов
        uint rubles = (uint)amount1;
        double kopeksAsDouble = (double)amount2;
        Console.WriteLine($"\nПриведение к uint: {rubles} руб.");
        Console.WriteLine($"Приведение к double: {kopeksAsDouble:F2} руб.");

        Console.ReadKey();
    }
}







// Класс для представления денежной величины
using Input_Validation;

public class Money
{
    public uint rubles;
    public byte kopeks;

    public Money() { rubles = 0; kopeks = 0; }
    public Money(uint rubles, byte kopeks)
    {
        if (kopeks > 99)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Копеек {kopeks} превышает 99. Установлено 99.");
            Console.ResetColor();
            this.kopeks = 99;
        }
        else this.kopeks = kopeks;
        this.rubles = rubles;
    }

    // Унарные операторы
    public static Money operator ++(Money m)
    {
        byte newKopeks = (byte)(m.kopeks + 1);
        if (newKopeks == 100)
            return new Money(m.rubles + 1, 0);
        else
            return new Money(m.rubles, newKopeks);
    }

    public static Money operator --(Money m)
    {
        if (m.kopeks == 0)
        {
            if (m.rubles > 0)
                return new Money(m.rubles - 1, 99);
            else
                return new Money(0, 0);
        }
        else
            return new Money(m.rubles, (byte)(m.kopeks - 1));
    }

    // Операторы приведения типов
    public static implicit operator uint(Money m) => m.rubles;
    public static explicit operator double(Money m) => m.kopeks / 100.0;

    // Бинарные операторы
    public static Money operator +(Money m1, Money m2)
    {
        long totalKopeks = (long)m1.rubles * 100 + m1.kopeks + (long)m2.rubles * 100 + m2.kopeks;
        uint rub = (uint)(totalKopeks / 100);
        byte kop = (byte)(totalKopeks % 100);
        return new Money(rub, kop);
    }

    public static Money operator -(Money m1, Money m2)
    {
        long difference = (long)m1.rubles * 100 + m1.kopeks - (long)m2.rubles * 100 - m2.kopeks;
        if (difference < 0) return new Money(0, 0);
        else return new Money((uint)(difference / 100), (byte)(difference % 100));
    }

    public static Money operator +(Money m, uint rub)
        => new Money(m.rubles + rub, m.kopeks);
    public static Money operator +(uint rub, Money m)
        => new Money(rub + m.rubles, m.kopeks);

    public static Money operator -(Money m, uint rub)
    {
        long totalKopeks = (long)m.rubles * 100 + m.kopeks - (long)rub * 100;
        if (totalKopeks < 0) return new Money(0, 0);
        else return new Money((uint)(totalKopeks / 100), (byte)(totalKopeks % 100));
    }

    public static Money operator -(uint rub, Money m)
    {
        long totalKopeks = (long)rub * 100 - (long)m.rubles * 100 - m.kopeks;
        if (totalKopeks < 0) return new Money(0, 0);
        else return new Money((uint)(totalKopeks / 100), (byte)(totalKopeks % 100));
    }

    // Метод ввода
    public static Money ReadFromConsole()
    {
        Console.WriteLine("Введите сумму (рубли и копейки):");
        uint rubles = InputValidation.InputUintWithValidation("Рубли: ");
        byte kopeks = InputValidation.InputByteWithValidation("Копейки (0-99): ", 0, 99);
        return new Money(rubles, kopeks);
    }

    // Переопределение ToString()
    public override string ToString() => $"{rubles} руб. {kopeks:D2} коп.";
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

        public static uint InputUintWithValidation(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Пустой ввод. Повторите.");
                    continue;
                }
                if (uint.TryParse(input, out uint result))
                    return result;
                else
                    Console.WriteLine("Неверный формат. Введите целое неотрицательное число.");
            }
        }

        public static byte InputByteWithValidation(string prompt, byte min, byte max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Пустой ввод. Повторите.");
                    continue;
                }
                if (byte.TryParse(input, out byte result) && result >= min && result <= max)
                    return result;
                else
                    Console.WriteLine($"Введите число от {min} до {max}.");
            }
        }
    }
}
