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
            Assert.AreEqual(ButterflyFSM.State.Resting, initialState, "����������� ��������� ������ ���� Resting.");
        }

        [TestMethod]
        public void Transition_FromResting_ToHibernating()
        {
            // Arrange
            var fsm = new ButterflyFSM();

            // Act
            var output = fsm.Transition(ButterflyFSM.Input.WinterColdsnap);

            // Assert
            Assert.AreEqual(ButterflyFSM.State.Hibernating, fsm.CurrentState, "������� � ��������� Hibernating �� ���������.");
            Assert.AreEqual(ButterflyFSM.Output.HibernationPreparing, output, "�������� �������� ������ ��� �������� � Hibernating.");
        }

        [TestMethod]
        public void Transition_FromHibernating_ToNutriting()
        {
            // Arrange
            var fsm = new ButterflyFSM();
            fsm.Transition(ButterflyFSM.Input.WinterColdsnap); // ��������� ������� � ��������� Hibernating

            // Act
            var output = fsm.Transition(ButterflyFSM.Input.FoodDetection);

            // Assert
            Assert.AreEqual(ButterflyFSM.State.Nutriting, fsm.CurrentState, "������� � ��������� Nutriting �� ���������.");
            Assert.AreEqual(ButterflyFSM.Output.PreparingForNutrition, output, "�������� �������� ������ ��� �������� � Nutriting.");
        }

        [TestMethod]
        public void Transition_FromNutriting_ToPairing()
        {
            // Arrange
            var fsm = new ButterflyFSM();
            fsm.Transition(ButterflyFSM.Input.FoodDetection); // ��������� ������� � ��������� Nutriting

            // Act
            var output = fsm.Transition(ButterflyFSM.Input.PartnerDetection);

            // Assert
            Assert.AreEqual(ButterflyFSM.State.Pairing, fsm.CurrentState, "������� � ��������� Pairing �� ���������.");
            Assert.AreEqual(ButterflyFSM.Output.PartnerAttracting, output, "�������� �������� ������ ��� �������� � Pairing.");
        }

        [TestMethod]
        public void Transition_FromPairing_ToResting()
        {
            // Arrange
            var fsm = new ButterflyFSM();
            fsm.Transition(ButterflyFSM.Input.PartnerDetection); // ��������� ������� � ��������� Pairing

            // Act
            var output = fsm.Transition(ButterflyFSM.Input.DailyCooling);

            // Assert
            Assert.AreEqual(ButterflyFSM.State.Resting, fsm.CurrentState, "������� � ��������� Resting �� ���������.");
            Assert.AreEqual(ButterflyFSM.Output.RestplaceSearching, output, "�������� �������� ������ ��� �������� � Resting.");
        }

        [TestMethod]
        public void Transition_WithoutStateChange_ShouldReturn_ProcessContinuation()
        {
            // Arrange
            var fsm = new ButterflyFSM();

            // Act
            var output = fsm.Transition(ButterflyFSM.Input.DailyCooling); // ������� �� Resting � Resting

            // Assert
            Assert.AreEqual(ButterflyFSM.State.Resting, fsm.CurrentState, "��������� ������ ���������� Resting.");
            Assert.AreEqual(ButterflyFSM.Output.ProcessContinuation, output, "�������� ������ ������ ���� ProcessContinuation ��� ���������� ��������� ���������.");
        }
    }
    [TestClass]
    public class ButterflyComplexTests
    {
        [TestMethod]
        public void TransitionTest_FromTranslationFile()
        {
            // ������������� ���� � ����� (� ����� bin/Debug ��� bin/Release)
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.txt");

            // ��������, ��� ���� ����������
            Assert.IsTrue(File.Exists(filePath), "���� � �������� ������� �� ������.");

            // ������ ���� ����� �� �����
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                // ������� ����� ��������� ��������� �������� ��� ������ ������
                var fsm = new ButterflyFSM();

                // ��������� ������ �� �������� �� ���� "�������_������/��������_������"
                var transitions = line.Split(' ');

                foreach (var transition in transitions)
                {
                    if (string.IsNullOrWhiteSpace(transition))
                        continue;

                    // ��������� �� ������� � �������� �������
                    var parts = transition.Split('/');
                    int inputSymbol = int.Parse(parts[0]);
                    int expectedOutputSymbol = int.Parse(parts[1]);

                    // �������������� inputSymbol � ��������������� Input enum
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
                            throw new ArgumentException("������������ ������� ������");
                    }

                    // �������������� expectedOutputSymbol � ��������������� Output enum
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
                            throw new ArgumentException("������������ �������� ������");
                    }

                    // ����� ������ Transition � ��������� ������������ ��������� �������
                    var actualOutput = fsm.Transition(input);

                    // ��������� ������������ � ���������� ��������� �������
                    Assert.AreEqual(expectedOutput, actualOutput,
                        $"������ � �������� {transition}: ��������� {expectedOutput}, ����������� {actualOutput}");
                }

                // ���������� ��������� ��������
                fsm = new ButterflyFSM();
            }
        }

        [TestMethod]
        public void TransitionTestFrom_ConfigAndTranslationFiles()
        {
            // ���� � ������ ������������ � ���������
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
            string transitionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.txt");

            // ��������, ��� ����� ����������
            Assert.IsTrue(File.Exists(configFilePath), "���� ������������ �� ������.");
            Assert.IsTrue(File.Exists(transitionFilePath), "���� � ���������� �� ������.");

            // ������� ��� ������������� inputSymbol � expectedOutputSymbol
            var inputMap = new Dictionary<int, ButterflyFSM.Input>();
            var outputMap = new Dictionary<int, ButterflyFSM.Output>();

            // ������ ����������������� �����
            var configLines = File.ReadAllLines(configFilePath);
            foreach (var line in configLines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split("==", StringSplitOptions.TrimEntries);
                int symbol = int.Parse(parts[0].Trim());

                // �����������, �������� �� ��� �������������� ��� Input ��� Output
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

            // ������ ���� ����� �� ����� � ����������
            var lines = File.ReadAllLines(transitionFilePath);

            foreach (var line in lines)
            {
                // ������� ����� ��������� ��������� �������� ��� ������ ������
                var fsm = new ButterflyFSM();

                // ��������� ������ �� �������� �� ���� "�������_������/��������_������"
                var transitions = line.Split(' ');

                foreach (var transition in transitions)
                {
                    if (string.IsNullOrWhiteSpace(transition)) continue;

                    // ��������� �� ������� � �������� �������
                    var parts = transition.Split('/');
                    int inputSymbol = int.Parse(parts[0]);
                    int expectedOutputSymbol = int.Parse(parts[1]);

                    // �������������� inputSymbol � expectedOutputSymbol � ������� ������������� �� ����� ������������
                    ButterflyFSM.Input input = inputMap[inputSymbol];
                    ButterflyFSM.Output expectedOutput = outputMap[expectedOutputSymbol];

                    // ����� ������ Transition � ��������� ������������ ��������� �������
                    var actualOutput = fsm.Transition(input);

                    // ��������� ������������ � ���������� ��������� �������
                    Assert.AreEqual(expectedOutput, actualOutput,
                        $"������ � �������� {transition}: ��������� {expectedOutput}, ����������� {actualOutput}");
                }

                // ���������� ��������� ��������
                fsm = new ButterflyFSM();
            }
        }
    }
}
