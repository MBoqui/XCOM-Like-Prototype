using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;

namespace GameStates
{
    public class StateMachine
    {
        State currentState;

        public Grid grid;

        public void Execute()
        {
            currentState.Execute();
        }

        public void Exit()
        {
            currentState.Exit();
        }


        public void SetState(State newState)
        {
            currentState = newState;
            currentState.Enter();
        }

        public void GoToNextTurn(int currentTurnIndex)
        {
            int nextTurnIndex = currentTurnIndex;
            bool isntValidIndex = true;

            while (isntValidIndex)
            {
                nextTurnIndex += 1;

                if (nextTurnIndex > GameSettings.Instance.numberPlayers)
                {
                    nextTurnIndex = 1;
                }

                isntValidIndex = UnitManager.Instance.GetArmyEliminated(nextTurnIndex);
            }

            SetState(new PlayerTurn(nextTurnIndex, this));
        }
    }
}
