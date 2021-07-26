using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazonian_Mars
{
    class Program
    {
        public enum Status
        {
            NoEffect,
            Healthy,
            Stunned,
            Poisoned = 15 //THIS WILL BE THE STANDARD POISON DAMAGE
            //DoubleHeal?
        }
        public enum DefendState
        {
            Physical,
            Magical,
            Ultimate,
            Healing,
        }
        public struct BattleAction
        {
            private int m_MoveValue;
            private int m_ManaValue;
            private string m_MoveName;
            private Status m_Effect;
            private DefendState m_ActionType;
            
            //get set properties for all important variables
            public int M_MoveValue { get { return m_MoveValue; } }
            public int M_ManaValue { get { return m_ManaValue; } }
            public string M_MoveName { get { return m_MoveName; } }
            public Status M_Effect { get { return m_Effect; } }
            public DefendState M_ActionType { get { return m_ActionType; } }
            //get set properties for all important variables

            public BattleAction(string movename, int movevalue, int manavalue,  Status effect, DefendState actiontype)
            {
                m_MoveValue = movevalue;
                m_ManaValue = manavalue;
                m_MoveName = movename;
                m_Effect = effect;
                m_ActionType = actiontype;
            }
        }
        static void Main(string[] args)
        {
            Living.Player player = new Living.Player(200, 200, Status.NoEffect, DefendState.Physical);
            Living.Enemy enemy = new Living.Enemy(400, 200, Status.NoEffect, DefendState.Physical, "Judas Tree");

            player.SetName();

            ManageGame.Screen.DisplayAllStats(player, enemy);
            ManageGame.Screen.DisplayAttacks(player.M_Support);
            Console.ReadLine();
            Console.Clear();

            ManageGame.Screen.DisplayDefensive();
            Console.ReadLine();
            Console.Clear();

            ManageGame.Screen.NarrateDefense(player, enemy, true, DefendState.Magical);
            Console.ReadLine();

        }
    }
}
