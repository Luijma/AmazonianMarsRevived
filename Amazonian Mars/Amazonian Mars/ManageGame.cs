using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazonian_Mars
{
    class ManageGame
    {
        public class BattleSystem
        {
            Living.Player m_Player;
            Program.BattleAction m_PlayerMove;
            Program.DefendState m_PlayerDefend;

            Living.Enemy m_Enemy;
            Program.BattleAction m_EnemyMove;
            Program.DefendState m_EnemyDefend;
            
            public BattleSystem(Living.Player player, Living.Enemy enemy)
            {
                m_Player = player;
                m_Enemy = enemy;
            }

            public void StartBattle()
            {
                Console.WriteLine(m_Player.M_Name + " just ran into " + m_Enemy.M_Name + "!");

                Console.WriteLine("");
                Console.WriteLine("");


                Console.WriteLine(m_Enemy.M_Name + " doesnt look very friendly!");
                Console.ReadLine();

                Console.Clear();

                Console.WriteLine("\t " + m_Player.M_Name + " VS " + m_Enemy.M_Name);
                Console.WriteLine("");
                Console.ReadLine();

                Console.Clear();

                BattleLoop();
            }

            private void BattleLoop()
            {
                do
                {
                    Screen.DisplayAllStats(m_Player, m_Enemy);

                    /*
                     * How will you attack?
                     * 1) Physical
                     * 2) Magical
                     * 3) Support
                     */
                    Screen.DisplayOffensive();

                    //Player's Attack Phase
                    switch (m_Player.ChoseBattleType())
                    {
                        case "1":
                            HandlePlayerTurn("physical", m_Player.M_Physical);

                            break;

                        case "2":
                            HandlePlayerTurn("magical", m_Player.M_Magical);

                            break;

                        case "3":
                            HandlePlayerTurn("support", m_Player.M_Support);
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("\n\n\n\t\tChoose a valid option! 1-3");
                            Console.ReadLine();
                            Console.Clear();
                            continue;
                    }
                    // Check Enemy HP, end battle if 0
                    if (m_Enemy.M_HP == 0)
                    {
                        break;
                    }
                    Console.Clear();

                    //Enemy Attack phase
                    Screen.DisplayAllStats(m_Player, m_Enemy);
                    m_EnemyMove = m_Enemy.ChoseBattleAction("");
                    Screen.NarrateActions(m_Enemy.M_Name, m_Player.M_Name, m_PlayerMove);

                    if (m_EnemyMove.M_MoveValue < 0)
                    { 
                        HandleAttack(m_Enemy, m_Player, m_EnemyMove);
                    }
                    else
                    {
                        HandleAttack(m_Enemy, m_Enemy, m_EnemyMove);
                    }

                    //Enemy's Defense Phase
                    /* Commenting out to focus on attack phases
                    m_EnemyDefend = m_Enemy.ChoseBlock();

                    Screen.NarrateActions(m_Player.M_Name, m_Enemy.M_Name, m_PlayerMove);

                    Screen.NarrateDefense(m_Enemy, m_Player, DefenseSuccess(m_PlayerMove.M_ActionType, m_EnemyDefend), m_EnemyDefend);

                    m_Player.ChangeMP(m_PlayerMove.M_ManaValue);
                    m_Enemy.ChangeHP(m_PlayerMove.M_MoveValue);

                    //Enemy's Defense Phase
                    */
                } while (true);

            }

            private void HandlePlayerTurn(string attackType, Program.BattleAction[] actions)
            {
                Console.Clear();
                Screen.DisplayAllStats(m_Player, m_Enemy);

                //display possible actions
                Screen.DisplayAttacks(actions);
                //player Choses move
                m_PlayerMove = m_Player.ChoseBattleAction(attackType);
                Console.Clear();
                //Clear Screen after option chosen

                //Display HUD
                Screen.DisplayAllStats(m_Player, m_Enemy);
                Screen.NarrateActions(m_Player.M_Name, m_Enemy.M_Name, m_PlayerMove);
                HandleAttack(m_Player, m_Enemy, m_PlayerMove);
            }

            public void HandleAttack(Living.Character Attacker, Living.Character Target, Program.BattleAction move)
            {
                Target.ChangeHP(move.M_MoveValue);
                Attacker.ChangeMP(move.M_ManaValue);
            }

            private int ModifiedMana()
            {
                //placeholder
                return 10;
            }
            private bool DefenseSuccess(Program.DefendState AttackType, Program.DefendState DefendType)
            {
                return true; // placeholder
            }
            
            
        }

        public struct Screen
        {
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ LEARN THESE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //                                                                VVVVVVVVVVV
                
                //Use this to Display HPMP HUD
            public static void DisplayAllStats(Living.Player player, Living.Enemy enemy)
            {
                //This is the order of the HUD that will always be present on screen
                DisplayHP(enemy);
                DisplayMP(enemy);
                Console.WriteLine("");
                Console.WriteLine("");

                DisplayHP(player);
                DisplayMP(player);
                Console.WriteLine("");
                Console.WriteLine("");
            }

            //Use this to print the Physical/Magical/support attack options
            public static void DisplayOffensive()
            {
                Console.WriteLine("How will you attack?");
                Console.WriteLine("");
                DisplayOptions(m_AttackTypes);
            }

            //Use this to print the Physical/Magical/Ult defend options
            public static void DisplayDefensive()
            {
                Console.WriteLine("How will you defend?");
                Console.WriteLine("");
                DisplayOptions(m_DefenseTypes);
            }
            //Display the Actual BattleActions alongside it's values
            public static void DisplayAttacks(Program.BattleAction[] battleActions)
            {
                Console.WriteLine("Pick your move (1-4)");
                Console.WriteLine("");
                
                //check what DefendState the move is to determine whether or not the move gives mana or takes away mana
                if(battleActions[0].M_ActionType == Program.DefendState.Physical)
                {
                    m_ManaPlusMinus = "+";
                }
                else
                {
                    m_ManaPlusMinus = "-";
                }

                for (int i = 0; i < battleActions.Length; i++)
                {
                    Console.Write((i + 1) + ")");
                    Console.Write(battleActions[i].M_MoveName);
                    Console.WriteLine("");

                    //Example of physical move: | Damage: 100, +Mana: 35 Status: None |
                    //Example of other moves  : | Damage: 100, -Mana: -35 Status: Poison |

                    Console.WriteLine("| Damage: {0}, {1}Mana: {2}, Status: {3} |", battleActions[i].M_MoveValue, m_ManaPlusMinus, battleActions[i].M_ManaValue, CheckMoveStatus(battleActions[i]));
                    Console.WriteLine("");
                    Console.WriteLine("");

                }

            }

            //if healing move, targetName will be the same as attackerName
            public static void NarrateActions(string attackerName, string targetName, Program.BattleAction battleAction)
            {
                string HpPlusMinus = "";
                string MpPlusMinus = "";
                if (battleAction.M_MoveValue < 0)
                    HpPlusMinus = "-";
                else
                    HpPlusMinus = "+";
                if (battleAction.M_ManaValue < 0)
                    MpPlusMinus = "-";
                else
                    MpPlusMinus = "+";

                Console.WriteLine(attackerName + " used " + battleAction.M_MoveName);

                if (HpPlusMinus == "+")
                    Console.Write(" on themselves!");

                Console.ReadLine();

                Console.WriteLine(targetName + " " + HpPlusMinus + Math.Abs(battleAction.M_MoveValue) + " Health!");
                Console.WriteLine("");
                Console.WriteLine("");

                Console.WriteLine(attackerName + " " + MpPlusMinus + Math.Abs(battleAction.M_MoveValue) + " Mana!");
                Console.WriteLine("");
                Console.ReadLine();

                Console.Clear();

            }

            public static void NarrateDefense(Living.Character Defender, Living.Character Attacker, bool sucessfulBlock, Program.DefendState blockType)
            {
                if(sucessfulBlock)
                {
                    Console.WriteLine(Defender.M_Name + " Succesfully blocked " + Attacker.M_Name + "'s attack!");
                }
                else if(!sucessfulBlock)
                {
                    Console.WriteLine(Defender.M_Name + " failed to succesfully block " + Attacker.M_Name + "'s attack!");
                }

                if(blockType == Program.DefendState.Physical)
                {
                    //code showing rewards for succesful physical block
                }
                else if(blockType == Program.DefendState.Magical)
                {
                    //code showing rewards for sucessful magical block
                }
                else if(blockType == Program.DefendState.Healing)
                {
                    //Code explaining lack of defensive choice because of healing
                }
                else if(blockType == Program.DefendState.Ultimate)
                {
                    //Code showing rewards for successful ultimate stall
                }
            }
            //                                                               ^^^^^^^^^^^^^
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ LEARN THESE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



            // the idea is to be able to switch on the fly what "char" we want to use to represent hp and MP, and still being able to reuse the code to display stats for both stats.
            private static string m_MPDisplay = "*";
            private static string m_HPDisplay = "|";
            private static string m_ManaPlusMinus = "";

            //These number is the one that is used in the for loop for DisplayStats();, so if we need individual "scaling" for the 2 stats, we can pass or change the value easily.
            //Essentially, it helps make them the same size for esthetic purposes.
            private static int m_HPIterator = 10;
            private static int m_MPIterator = 7;

            //The arrays for both menus for "AttackTypes" and "DefenseOptions"
            private static string[] m_AttackTypes = new string[4] { "Physical", "Magic", "Support", "Ult" };
            private static string[] m_DefenseTypes = new string[3] { "Physical", "Magical", "Ultimate" };

            private static void DisplayStats(int stat, string display, int iterator)
            {
                for (int i = 0; i < stat; i += iterator)
                {
                    Console.Write(display);
                }
            }

            private static void DisplayHP(Living.Character character)
            {
                //Display the actual hp bar using the displaystat function
                Console.Write(character.M_Name + ": ");
                Console.Write("HP: [");
                DisplayStats(character.M_HP, m_HPDisplay, m_HPIterator);               
                Console.Write("]");
                Console.Write(" - {0}  -", character.M_HP);
                

                //Add additional information, such as if the character is at full health,
                // if the character is poisoned, or stunned next to the HP bar in all caps
                if (character.M_HP == character.M_MaxHP && character.M_CurrentStatus == Program.Status.NoEffect)
                {
                    Console.Write(" MAX - ");
                }
                else if (character.M_CurrentStatus != Program.Status.NoEffect)
                { 
                        Console.Write(character.M_CurrentStatus.ToString());
                }
                Console.WriteLine("");
            }
            private static void DisplayMP(Living.Character character)
            {
                Console.Write(AllignStats(character.M_Name) + "MP: ");
                Console.Write("{");
                DisplayStats(character.M_MP, m_MPDisplay, m_MPIterator);
                Console.Write("}");
            }            

            //Print the offensive/defensive arrays, no need to call outside of class
            private static void DisplayOptions(string[] ShownOptions)
            {
                for(int i = 0; i < ShownOptions.Length; i++)
                {
                    Console.Write((i+1) + ") ");
                    Console.Write(ShownOptions[i]);
                    Console.WriteLine();
                }
            }

            //return string to display status effect of move
            private static string CheckMoveStatus(Program.BattleAction battleAction)
            {
                if(battleAction.M_Effect == Program.Status.Stunned)
                {
                    return "Stun";
                }
                if(battleAction.M_Effect == Program.Status.Poisoned)
                {
                    return "Poison";
                }
                if(battleAction.M_Effect == Program.Status.Healthy)
                {
                    return "Clean";
                }
                
                //if none of the above apply, return "None"
                return "None";
            }

            //make the MP stat allign with HP stat for readability
            private static string AllignStats(string name)
            {
                //two spaces to account for symbols
                string space = "  ";
                for(int i = 0; i < name.Length; i++)
                {
                    space += " ";
                }
                return space;
            }

        }
    }
}
