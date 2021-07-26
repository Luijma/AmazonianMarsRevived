using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazonian_Mars
{
     class Living
     {
        public abstract class Character
        {
            protected int m_HP;
            protected int m_MaxHP;
            protected int m_MP;
            protected int m_MaxMP;
            protected string m_Name;

            protected Program.Status m_CurrentStatus;
            protected Program.DefendState m_BlockType;

            protected Program.BattleAction[] m_Physical = new Program.BattleAction[4];
            protected Program.BattleAction[] m_Magical = new Program.BattleAction[4];
            protected Program.BattleAction[] m_Support = new Program.BattleAction[4];
            protected Program.BattleAction[] m_Ultimate = new Program.BattleAction[1];

            public Character(int maxHp, int maxMp, Program.Status status, Program.DefendState defend)
            {
                m_MaxHP = maxHp;
                m_HP = maxHp;

                m_MaxMP = maxMp;
                m_MP = maxMp;

                m_CurrentStatus = status;
                m_BlockType = defend;
            }

            // Get Properties of the most used variables
            public string M_Name { get { return m_Name; } }
            public int M_HP { get { return m_HP; } }
            public int M_MP { get { return m_MP; } }
            public int M_MaxHP { get { return m_MaxHP; } }
            public int M_MaxMP { get { return m_MaxHP; } }

            public Program.Status M_CurrentStatus { get { return m_CurrentStatus; } }
            public Program.DefendState M_BlockType { get { return m_BlockType; } }

            public Program.BattleAction[] M_Physical { get { return m_Physical; } }
            public Program.BattleAction[] M_Magical { get { return m_Magical; } }
            public Program.BattleAction[] M_Support { get { return m_Support; } }
            public Program.BattleAction[] M_Ultimate { get { return m_Ultimate; } }
            // Get Properties of the most used variables

            public void ChangeHP(int movevalue)
            {
                m_HP += movevalue;

                //make sure hp doesnt go below zero or above MaxHP
                if (m_HP < 0)
                { m_HP = 0;}

                else if (m_HP > M_MaxHP)
                { m_HP = m_MaxHP;}
            }

            public void ChangeMP(int manavalue)
            {
                m_MP += manavalue;
                
                if(m_MP < 0)
                { m_MP = 0; }
                else if(m_MP > M_MaxMP)
                { m_MP = M_MaxMP; }                
            }
            
            public void ChangeStatus(Program.Status effect)
            {
                if(effect != Program.Status.NoEffect)
                    m_CurrentStatus = effect;
            }

            public abstract Program.BattleAction ChoseBattleAction(string battletype);
            public abstract Program.DefendState ChoseBlock();

            

        }
        public class Player : Character
        {
            //First calls the base Character constructor, then fills in the arrays with the BattleActions.
            public Player(int maxHp, int maxMp, Program.Status status, Program.DefendState defend) : base(maxHp, maxMp, status, defend)
            {
                //negative number on movevalue means move deals damage.
                //positive number on movevalue means move heals damage.
                //Same concept for manavalue.
                m_Physical[0] = new Program.BattleAction("Cross Slash", -35, 40, Program.Status.NoEffect, Program.DefendState.Physical);
                m_Physical[1] = new Program.BattleAction("Spirit Drain", -25, 50, Program.Status.NoEffect, Program.DefendState.Physical);
                m_Physical[2] = new Program.BattleAction("Grass Roots", -30, 38, Program.Status.NoEffect, Program.DefendState.Physical);
                m_Physical[3] = new Program.BattleAction("Drain", -35, 25, Program.Status.NoEffect, Program.DefendState.Physical);

                m_Magical[0] = new Program.BattleAction("Explosive poison bomb", -15, -45, Program.Status.Poisoned, Program.DefendState.Magical);
                m_Magical[1] = new Program.BattleAction("Icy blizzard", -25, 50, Program.Status.Stunned, Program.DefendState.Magical);
                m_Magical[2] = new Program.BattleAction("Windy tornado", -90, -115, Program.Status.NoEffect, Program.DefendState.Magical);
                m_Magical[3] = new Program.BattleAction("Watery tearfest", -60, -50, Program.Status.NoEffect, Program.DefendState.Magical);

                m_Support[0] = new Program.BattleAction("Grandma's Recipe", 125, -75, Program.Status.NoEffect, Program.DefendState.Healing);
                m_Support[1] = new Program.BattleAction("Basil Buffet", 45, -85, Program.Status.Healthy, Program.DefendState.Healing);
                m_Support[2] = new Program.BattleAction("Aura Cleansing", 75, -40, Program.Status.NoEffect, Program.DefendState.Healing);
                m_Support[3] = new Program.BattleAction("Solar Winds", 45, -30, Program.Status.NoEffect, Program.DefendState.Healing);

                m_Ultimate[0] = new Program.BattleAction("Ultimate", 200, -150, Program.Status.NoEffect, Program.DefendState.Ultimate);
            }


            //Allows the user to pick their character's name. 
            public void SetName()
            {
                string yesno;
                do
                {
                    Console.WriteLine("What's your name little fella?");
                    m_Name = Console.ReadLine();
                    Console.Clear();

                    do
                    {
                        Console.WriteLine("are you sure your name is " + m_Name, "?");
                        yesno = Console.ReadLine();
                        Console.Clear();
                    } while (yesno != "yes" && yesno != "no");


                } while (yesno == "no");
                Console.Clear();
            }

            public string ChoseBattleType()
            {
                //Placeholder Code
                string choice;
                choice = Console.ReadLine();
                return choice;
            }
            public override Program.BattleAction ChoseBattleAction(string battletype)
            {
                string choice = Console.ReadLine();
                int moveIndex = int.Parse(choice);
                Program.BattleAction chosenmove = new Program.BattleAction();
                switch (battletype)
                {
                    case "physical":
                        chosenmove = this.m_Physical[moveIndex];
                        break;
                    case "magical":
                        chosenmove = this.m_Magical[moveIndex];
                        break;
                    case "support":
                        chosenmove = this.m_Support[moveIndex];
                        break;
                }
                

                return chosenmove;
            }
            public override Program.DefendState ChoseBlock()
            {
                //PLACEHOLDER
                return Program.DefendState.Physical;
            }
        }
        public class Enemy : Character
        {
            //First calls the Character's base constructor, then the Enemy constructor
            public Enemy(int maxHP, int maxMP, Program.Status status, Program.DefendState defend, string name):base(maxHP, maxMP, status, defend)
            {
                m_Name = name;

                m_Physical[0] = new Program.BattleAction("Absorb", -35, 40, Program.Status.NoEffect, Program.DefendState.Physical);
                m_Physical[1] = new Program.BattleAction("Leachify", -25, 50, Program.Status.NoEffect, Program.DefendState.Physical);
                m_Physical[2] = new Program.BattleAction("Dig deep", -30, 38, Program.Status.NoEffect, Program.DefendState.Physical);
                m_Physical[3] = new Program.BattleAction("Timber", -35, 25, Program.Status.NoEffect, Program.DefendState.Physical);

                m_Magical[0] = new Program.BattleAction("Foggy Mist", -45, -35, Program.Status.NoEffect, Program.DefendState.Magical);
                m_Magical[1] = new Program.BattleAction("Ivy Tornado", -15, -75, Program.Status.Poisoned, Program.DefendState.Magical);
                m_Magical[2] = new Program.BattleAction("Malicious vines", -55, -90, Program.Status.Stunned, Program.DefendState.Magical);
                m_Magical[3] = new Program.BattleAction("Sappy surprise", -75, -125, Program.Status.NoEffect, Program.DefendState.Magical);

                m_Support[0] = new Program.BattleAction("Synthesis", 55, -90, Program.Status.NoEffect, Program.DefendState.Magical);
                m_Support[1] = new Program.BattleAction("Himarra's remedy", 35, -100, Program.Status.Healthy, Program.DefendState.Healing);
                m_Support[2] = new Program.BattleAction("HotSpring", 100, -130, Program.Status.NoEffect, Program.DefendState.Healing);
                m_Support[3] = new Program.BattleAction("Stanley Steamer", 30, -35, Program.Status.NoEffect, Program.DefendState.Healing);

                m_Ultimate[0] = new Program.BattleAction("Earth shaking quake", -100, -175, Program.Status.Poisoned, Program.DefendState.Ultimate);
            }

            public override Program.BattleAction ChoseBattleAction(string battletype)
            {
                //random
                Random random = new Random();

                bool NeedHealing = false;
                bool NeedMana = false;

                if (m_HP <= 100)
                    NeedHealing = true;
                if (M_MP <= 50)
                    NeedMana = true;

                //choosing a heal when necessary
                if (NeedHealing && m_MP >= 35 && m_MP < 90)
                    return m_Support[3];

                if (NeedHealing && m_MP >= 90 && m_MP < 130)
                    return m_Support[0];

                if (NeedHealing && m_MP >= 130)
                    return m_Support[2];

                //if enemy status
                if (m_CurrentStatus == Program.Status.Poisoned)
                    return m_Support[1];
                if (m_CurrentStatus == Program.Status.Stunned)
                    return m_Support[1];

                if (!NeedHealing && !NeedMana)
                {
                    //random max mp & hp move choices
                    if (m_HP == m_MaxHP && m_MP == m_MaxMP)
                    {
                        int EarthShakingQuake = 1, SappySurprise = 2, EndRandom = 3;

                        int MaxHP_MaxMP = random.Next(EarthShakingQuake, EndRandom);

                        if (MaxHP_MaxMP == EarthShakingQuake)
                            return m_Ultimate[0];
                        if (MaxHP_MaxMP == SappySurprise)
                            return m_Magical[3];
                    }

                    //random high mp move choices
                    if (m_MP >= 125 && m_MP != m_MaxMP)
                    {
                        int SappySurprise = 1, MaliciousVines = 2, EndRandom = 3;

                        int highMP = random.Next(SappySurprise, EndRandom);

                        if (highMP == SappySurprise)
                            return m_Magical[3];
                        if (highMP == MaliciousVines)
                            return m_Magical[2];
                    }

                    //random mid mp move choices
                    if (m_MP >= 50 && m_MP <= 125)
                    {
                        int FoggyMist = 1, Timber = 2, EndRandom = 3;

                        int midMP = random.Next(FoggyMist, EndRandom);

                        if (midMP == FoggyMist)
                            return m_Magical[0];
                        if (midMP == Timber)
                            return m_Physical[3];
                    }

                    //if player status

                    if (/*player.M_CurrentStatus == Program.Status.NoEffect && */ m_MP >= 125)
                    {
                        return m_Magical[1];
                    }
                }

                //random low mp move choices.
                if (NeedMana)
                {
                    int Absorb = 1, Leachify = 2, DigDeep = 3, EndRandom = 4;

                    int NeedManaChoice = random.Next(Absorb, EndRandom);

                    if (NeedManaChoice == Absorb)
                        return m_Physical[0];
                    if (NeedManaChoice == Leachify)
                        return m_Physical[1];
                    if (NeedManaChoice == DigDeep)
                        return m_Physical[3];
                }

                //placeholder
                return m_Physical[0];

            }
            public override Program.DefendState ChoseBlock()
            {
                //PLACEHOLDER
                return Program.DefendState.Physical;
            }
        }

     }
}
