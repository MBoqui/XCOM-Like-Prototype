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

        public GameObject tankPrefab;
        public Grid grid;
        public GridAgent tank;
        int numberPlayers;

        public StateMachine(Grid grid, int numberPlayers, GameObject tankPrefab)
        {
            this.tankPrefab = tankPrefab;
            this.grid = grid;
            this.numberPlayers = numberPlayers;
        }

        public void Execute()
        {
            currentState.Execute();
        }


        public void SetState(State newState)
        {
            currentState = newState;
            currentState.Enter();
        }

        public void GoToNextTurn(int currentTurnIndex)
        {
            int nextTurnIndex = currentTurnIndex + 1;

            if (nextTurnIndex > numberPlayers)
            {
                nextTurnIndex = 1;
            }

            SetState(new PlayerTurn(nextTurnIndex, this));
        }
    }
}
