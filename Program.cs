using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using static ClassLibrary.Towers;
using static System.Console;

namespace CarsonTowerOfHanoi
{
    class Program
    {
        static void Main(string[] args)
        {
            int numDiscs = 0;
            int from = 0;
            int to = 0;
            bool validInput;
            bool endGame;
            bool playAgain;

            do
            {
                Clear();
                do
                {
                    try
                    {
                        numDiscs = GetGameDiscs();
                    }
                    catch (InvalidHeightException e)
                    {
                        WriteLine(e.Message);
                    }
                } while (numDiscs == 0);

                // Creates towers 
                // sets MinimumPossibleMoves property
                // Displays towers
                // Converts myTowers to array
                Towers myTowers = new Towers(numDiscs);
                myTowers.MinimumPossibleMoves = MinimumMoves(numDiscs);
                Update(myTowers);

                do
                {
                    do
                    {
                        validInput = false;
                        endGame = false;// resets validInput

                        try // Gets 'from' pole
                        {
                            from = MoveFrom();
                            validInput = true;
                        }
                        catch (InvalidMoveException e)
                        {
                            WriteLine(e.Message);
                        }
                    } while (validInput == false);

                    if (from == -1) endGame = true; // checks if user decides to quit
                    else
                    {
                        do
                        {
                            try // gets 'to' pole
                            {
                                to = MoveTo();
                                validInput = true;
                            }
                            catch (InvalidMoveException e)
                            {
                                WriteLine(e.Message);
                                validInput = false;
                            }
                        } while (validInput == false);
                    }
                    if (!endGame) // if the user did not quit
                    {
                        // Makes move
                        // Displays towers
                        // converts to array
                        // Checks if game is complete
                        // Ends game if game is complete
                        myTowers.Move(from, to);
                        Update(myTowers);
                        myTowers.IsComplete = GameComplete(myTowers, numDiscs, from, to);
                        if (myTowers.IsComplete) endGame = true;
                    }

                } while (!endGame);

                playAgain = PlayAgain();

            } while (playAgain);

        } // end Main

        public static int MoveFrom(/*bool displayCtrlY = true*/)
        {
            string validFromInput;
            int validFromInt;

            Write("\nEnter 'from' tower number or 'x' to quit: ");
            validFromInput = ReadKey().KeyChar.ToString().ToUpper();
            if (validFromInput == "X")
            {
                return -1;
            }
            else
            {
                validFromInt = int.Parse(validFromInput);
                if (validFromInt == 0 || validFromInt < 0 || validFromInt > 3) throw new InvalidMoveException("Invalid tower.");
                else return validFromInt;
            }
        }

        public static int MoveTo()
        {
            int validToInput;

            Write("\nEnter 'to' tower number: ");
            int.TryParse(ReadKey().KeyChar.ToString().ToUpper(), out validToInput);
            if (validToInput == 0 || validToInput < 0 || validToInput > 3)
            {
                throw new InvalidMoveException("Invalid tower.");
            }
            return validToInput;
        }

        public static int GetGameDiscs()
        {
            int validInt;

            Write("How many discs would you like? (5 is default, 9 is max): ");
            int.TryParse(ReadLine(), out validInt);
            if (validInt == 0 || validInt < 0 || validInt > 9)
            {
                throw new InvalidHeightException();
            }
            return validInt;
        }


        public static int MinimumMoves(int numDiscs)
        {
            int minMoves = (int)Math.Pow(2, numDiscs) - 1;

            return minMoves;
        }

        public static bool PlayAgain()
        {
            WriteLine("\nWould you like to play again?('y' for yes! or any key to quit): ");
            string playAgainInput = ReadKey().KeyChar.ToString().ToUpper();
            if (playAgainInput == "Y") return true;
            else return false;
        }

        public static bool GameComplete(Towers myTowers, int numberOfDiscs, int from, int to)
        {
            if (myTowers.poleThree.Count == numberOfDiscs)
            {
                myTowers.IsComplete = true;
                WriteLine($"\nCongratulations, you completed the puzzle in {myTowers.NumberOfMoves} moves.");
                if (myTowers.MinimumPossibleMoves == myTowers.NumberOfMoves) WriteLine($"\nThat's the fewest number of moves possible. I ANOINT YOU THE RULER OF HANOI!");
                else
                {
                    WriteLine($"\nYou completed the puzzle in {myTowers.NumberOfMoves} moves but the fewest possible is {myTowers.MinimumPossibleMoves}");
                    WriteLine("\nLet's give it another shot. What do you say?");
                }
                return true;
            }
            else
            {
                WriteLine($"\nMove {myTowers.NumberOfMoves} complete. Successfully moved disc from tower {from} to tower {to}.");
                myTowers.IsComplete = false;
                return false;
            }
        }

        public static void Update(Towers myTowers)
        {
            TowerUtilities.DisplayTowers(myTowers);
            myTowers.ToArray();
        }
    }
}
