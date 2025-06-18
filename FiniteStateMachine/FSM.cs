using System;

namespace FiniteStateMachine
{
    public class ButterflyFSM
    {
        // Перечисление состояний бабочки
        public enum State
        {
            Resting = 0,    // Отдых
            Hibernating,    // Спячка
            Nutriting,      // Питание
            Pairing         // Спаривание
        }

        // Поле для хранения текущего состояния
        private State СurrentState;

        // Перечисление входного алфавита 
        public enum Input
        {
            DailyCooling = 0,   // Переход к отдыху
            WinterColdsnap,     // Переход к спячке
            FoodDetection,      // Переход к питанию
            PartnerDetection    // Переход к спариванию
            
        }

        // Перечисление выходного алфавита
        public enum Output
        {
            RestplaceSearching = 0,     // Переход к отдыху
            HibernationPreparing,       // Переход к спячке
            PreparingForNutrition,      // Переход к питанию
            PartnerAttracting,          // Переход к спариванию
            ProcessContinuation         // Продолжение процесса
        }

        // Конструктор, который устанавливает начальное состояние
        public ButterflyFSM()
        {
            СurrentState = State.Resting;
        }

        // Свойство для доступа к текущему состоянию
        public State CurrentState
        {
            get { return СurrentState; }
            private set { СurrentState = value; }
        }

        // Метод для изменения текущего состояния автомата
        public Output Transition(Input input)
        {
            Output output = Output.ProcessContinuation; // По умолчанию процесс продолжается
            State previousState = CurrentState; // Сохраняем предыдущее состояние

            switch (CurrentState)
            {
                case State.Resting:
                    switch (input)
                    {
                        case Input.WinterColdsnap:
                            CurrentState = State.Hibernating;
                            output = Output.HibernationPreparing;
                            break;
                        case Input.FoodDetection:
                            CurrentState = State.Nutriting;
                            output = Output.PreparingForNutrition;
                            break;
                        case Input.PartnerDetection:
                            CurrentState = State.Pairing;
                            output = Output.PartnerAttracting;
                            break;
                    }
                    break;

                case State.Hibernating:
                    switch (input)
                    {
                        case Input.DailyCooling:
                            CurrentState = State.Resting;
                            output = Output.RestplaceSearching;
                            break;
                        case Input.FoodDetection:
                            CurrentState = State.Nutriting;
                            output = Output.PreparingForNutrition;
                            break;
                        case Input.PartnerDetection:
                            CurrentState = State.Pairing;
                            output = Output.PartnerAttracting;
                            break;
                    }
                    break;

                case State.Nutriting:
                    switch (input)
                    {
                        case Input.DailyCooling:
                            CurrentState = State.Resting;
                            output = Output.RestplaceSearching;
                            break;
                        case Input.PartnerDetection:
                            CurrentState = State.Pairing;
                            output = Output.PartnerAttracting;
                            break;
                        case Input.WinterColdsnap:
                            CurrentState = State.Hibernating;
                            output = Output.HibernationPreparing;
                            break;
                    }
                    break;

                case State.Pairing:
                    switch (input)
                    {
                        case Input.DailyCooling:
                            CurrentState = State.Resting;
                            output = Output.RestplaceSearching;
                            break;
                        case Input.WinterColdsnap:
                            CurrentState = State.Hibernating;
                            output = Output.HibernationPreparing;
                            break;
                        case Input.FoodDetection:
                            CurrentState = State.Nutriting;
                            output = Output.PreparingForNutrition;
                            break;
                    }
                    break;
            }

            // Проверяем, изменилось ли текущее состояние
            if (CurrentState == previousState)
            {
                output = Output.ProcessContinuation;
            }

            return output; // Возвращаем выходной символ после перехода
        }
    }

}
