using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiniteStateMachine;
using System;
using System.IO;

namespace FSMTests
{
    [TestClass]
    public class ButterflyEasyTests
    {
        [TestMethod]
        public void InitialState_ShouldBe_Resting()
        {
            // Arrange
            var fsm = new ButterflyFSM();

            // Act
            var initialState = fsm.CurrentState;

            // Assert
            Assert.AreEqual(ButterflyFSM.State.Resting, initialState, "Изначальное состояние должно быть Resting.");
        }

        [TestMethod]
        public void Transition_FromResting_ToHibernating()
        {
            // Arrange
            var fsm = new ButterflyFSM();

            // Act
            var output = fsm.Transition(ButterflyFSM.Input.WinterColdsnap);

            // Assert
            Assert.AreEqual(ButterflyFSM.State.Hibernating, fsm.CurrentState, "Переход в состояние Hibernating не произошел.");
            Assert.AreEqual(ButterflyFSM.Output.HibernationPreparing, output, "Неверный выходной символ при переходе в Hibernating.");
        }

        [TestMethod]
        public void Transition_FromHibernating_ToNutriting()
        {
            // Arrange
            var fsm = new ButterflyFSM();
            fsm.Transition(ButterflyFSM.Input.WinterColdsnap); // Переводим автомат в состояние Hibernating

            // Act
            var output = fsm.Transition(ButterflyFSM.Input.FoodDetection);

            // Assert
            Assert.AreEqual(ButterflyFSM.State.Nutriting, fsm.CurrentState, "Переход в состояние Nutriting не произошел.");
            Assert.AreEqual(ButterflyFSM.Output.PreparingForNutrition, output, "Неверный выходной символ при переходе в Nutriting.");
        }

        [TestMethod]
        public void Transition_FromNutriting_ToPairing()
        {
            // Arrange
            var fsm = new ButterflyFSM();
            fsm.Transition(ButterflyFSM.Input.FoodDetection); // Переводим автомат в состояние Nutriting

            // Act
            var output = fsm.Transition(ButterflyFSM.Input.PartnerDetection);

            // Assert
            Assert.AreEqual(ButterflyFSM.State.Pairing, fsm.CurrentState, "Переход в состояние Pairing не произошел.");
            Assert.AreEqual(ButterflyFSM.Output.PartnerAttracting, output, "Неверный выходной символ при переходе в Pairing.");
        }

        [TestMethod]
        public void Transition_FromPairing_ToResting()
        {
            // Arrange
            var fsm = new ButterflyFSM();
            fsm.Transition(ButterflyFSM.Input.PartnerDetection); // Переводим автомат в состояние Pairing

            // Act
            var output = fsm.Transition(ButterflyFSM.Input.DailyCooling);

            // Assert
            Assert.AreEqual(ButterflyFSM.State.Resting, fsm.CurrentState, "Переход в состояние Resting не произошел.");
            Assert.AreEqual(ButterflyFSM.Output.RestplaceSearching, output, "Неверный выходной символ при переходе в Resting.");
        }

        [TestMethod]
        public void Transition_WithoutStateChange_ShouldReturn_ProcessContinuation()
        {
            // Arrange
            var fsm = new ButterflyFSM();

            // Act
            var output = fsm.Transition(ButterflyFSM.Input.DailyCooling); // Переход из Resting в Resting

            // Assert
            Assert.AreEqual(ButterflyFSM.State.Resting, fsm.CurrentState, "Состояние должно оставаться Resting.");
            Assert.AreEqual(ButterflyFSM.Output.ProcessContinuation, output, "Выходной символ должен быть ProcessContinuation при отсутствии изменения состояния.");
        }
    }
    [TestClass]
    public class ButterflyComplexTests
    {
        [TestMethod]
        public void TransitionTest_FromTranslationFile()
        {
            // Относительный путь к файлу (в папке bin/Debug или bin/Release)
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.txt");

            // Проверка, что файл существует
            Assert.IsTrue(File.Exists(filePath), "Файл с входными данными не найден.");

            // Чтение всех строк из файла
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                // Создаем новый экземпляр конечного автомата для каждой строки
                var fsm = new ButterflyFSM();

                // Разделяем строку по пробелам на пары "входной_символ/выходной_символ"
                var transitions = line.Split(' ');

                foreach (var transition in transitions)
                {
                    if (string.IsNullOrWhiteSpace(transition))
                        continue;

                    // Разделяем на входной и выходной символы
                    var parts = transition.Split('/');
                    int inputSymbol = int.Parse(parts[0]);
                    int expectedOutputSymbol = int.Parse(parts[1]);

                    // Преобразование inputSymbol в соответствующий Input enum
                    ButterflyFSM.Input input;
                    switch (inputSymbol)
                    {
                        case 0:
                            input = ButterflyFSM.Input.DailyCooling;
                            break;
                        case 1:
                            input = ButterflyFSM.Input.WinterColdsnap;
                            break;
                        case 2:
                            input = ButterflyFSM.Input.FoodDetection;
                            break;
                        case 3:
                            input = ButterflyFSM.Input.PartnerDetection;
                            break;
                        default:
                            throw new ArgumentException("Недопустимый входной символ");
                    }

                    // Преобразование expectedOutputSymbol в соответствующий Output enum
                    ButterflyFSM.Output expectedOutput;
                    switch (expectedOutputSymbol)
                    {
                        case 0:
                            expectedOutput = ButterflyFSM.Output.RestplaceSearching;
                            break;
                        case 1:
                            expectedOutput = ButterflyFSM.Output.HibernationPreparing;
                            break;
                        case 2:
                            expectedOutput = ButterflyFSM.Output.PreparingForNutrition;
                            break;
                        case 3:
                            expectedOutput = ButterflyFSM.Output.PartnerAttracting;
                            break;
                        case 4:
                            expectedOutput = ButterflyFSM.Output.ProcessContinuation;
                            break;
                        default:
                            throw new ArgumentException("Недопустимый выходной символ");
                    }

                    // Вызов метода Transition и получение фактического выходного символа
                    var actualOutput = fsm.Transition(input);

                    // Сравнение фактического и ожидаемого выходного символа
                    Assert.AreEqual(expectedOutput, actualOutput,
                        $"Ошибка в переходе {transition}: ожидаемый {expectedOutput}, фактический {actualOutput}");
                }

                // Сбрасываем состояние автомата
                fsm = new ButterflyFSM();
            }
        }

        [TestMethod]
        public void TransitionTestFrom_ConfigAndTranslationFiles()
        {
            // Путь к файлам конфигурации и переходов
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
            string transitionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.txt");

            // Проверка, что файлы существуют
            Assert.IsTrue(File.Exists(configFilePath), "Файл конфигурации не найден.");
            Assert.IsTrue(File.Exists(transitionFilePath), "Файл с переходами не найден.");

            // Словари для сопоставления inputSymbol и expectedOutputSymbol
            var inputMap = new Dictionary<int, ButterflyFSM.Input>();
            var outputMap = new Dictionary<int, ButterflyFSM.Output>();

            // Чтение конфигурационного файла
            var configLines = File.ReadAllLines(configFilePath);
            foreach (var line in configLines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split("==", StringSplitOptions.TrimEntries);
                int symbol = int.Parse(parts[0].Trim());

                // Определение, является ли это сопоставлением для Input или Output
                if (parts[1].Contains("Input"))
                {
                    var input = Enum.Parse<ButterflyFSM.Input>(parts[1].Trim().Replace("ButterflyFSM.Input.", ""));
                    inputMap[symbol] = input;
                }
                else if (parts[1].Contains("Output"))
                {
                    var output = Enum.Parse<ButterflyFSM.Output>(parts[1].Trim().Replace("ButterflyFSM.Output.", ""));
                    outputMap[symbol] = output;
                }
            }

            // Чтение всех строк из файла с переходами
            var lines = File.ReadAllLines(transitionFilePath);

            foreach (var line in lines)
            {
                // Создаем новый экземпляр конечного автомата для каждой строки
                var fsm = new ButterflyFSM();

                // Разделяем строку по пробелам на пары "входной_символ/выходной_символ"
                var transitions = line.Split(' ');

                foreach (var transition in transitions)
                {
                    if (string.IsNullOrWhiteSpace(transition)) continue;

                    // Разделяем на входной и выходной символы
                    var parts = transition.Split('/');
                    int inputSymbol = int.Parse(parts[0]);
                    int expectedOutputSymbol = int.Parse(parts[1]);

                    // Преобразование inputSymbol и expectedOutputSymbol с помощью сопоставлений из файла конфигурации
                    ButterflyFSM.Input input = inputMap[inputSymbol];
                    ButterflyFSM.Output expectedOutput = outputMap[expectedOutputSymbol];

                    // Вызов метода Transition и получение фактического выходного символа
                    var actualOutput = fsm.Transition(input);

                    // Сравнение фактического и ожидаемого выходного символа
                    Assert.AreEqual(expectedOutput, actualOutput,
                        $"Ошибка в переходе {transition}: ожидаемый {expectedOutput}, фактический {actualOutput}");
                }

                // Сбрасываем состояние автомата
                fsm = new ButterflyFSM();
            }
        }
    }
}
