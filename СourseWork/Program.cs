using System;
using System.Xml.Linq;
using FiniteStateMachine;

class Program
{
    static void Main(string[] args)
    {
        ButterflyFSM fsm = new ButterflyFSM(); // Создаем экземпляр автомата
        Console.WriteLine("Начальное состояние: " + fsm.CurrentState);
        Console.WriteLine("Введите номер состояния для переключения:");
        Console.WriteLine("1 - Отдых (Resting)");
        Console.WriteLine("2 - Спячка (Hibernating)");
        Console.WriteLine("3 - Питание (Nutriting)");
        Console.WriteLine("4 - Спаривание (Pairing)");
        Console.WriteLine("0 - Выход из программы");

        while (true)
        {
            Console.Write(" Введите номер состояния: ");
            string? userInput = Console.ReadLine();

            // Проверка на выход из программы
            if (userInput.Equals("0"))
            {
                break;
            }

            // Переключение состояния на основе ввода пользователя
            if (int.TryParse(userInput, out int inputNumber) && inputNumber >= 0 && inputNumber <= 4)
            {
                ButterflyFSM.Input input;

                // Преобразуем номер состояния в соответствующий входной символ
                switch (inputNumber)
                {
                    case 1:
                        input = ButterflyFSM.Input.DailyCooling; // Переход к отдыху
                        break;
                    case 2:
                        input = ButterflyFSM.Input.WinterColdsnap; // Переход к спячке
                        break;
                    case 3:
                        input = ButterflyFSM.Input.FoodDetection; // Переход к питанию
                        break;
                    case 4:
                        input = ButterflyFSM.Input.PartnerDetection; // Переход к спариванию
                        break;
                    default:
                        continue; // Игнорируем другие значения
                }

                // Переход к новому состоянию
                ButterflyFSM.Output output = fsm.Transition(input);
                Console.WriteLine("  Новое состояние: " + fsm.CurrentState);
                Console.WriteLine("  Выходной символ: " + output);
            }
            else
            {
                Console.WriteLine("  Некорректный ввод. Попробуйте еще раз.");
            }
        }

        Console.WriteLine("Программа завершена.");
    }
}

