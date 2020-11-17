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
            int numberOfDiscs;
            int fromInputInt;
            int toInputInt = 0;
            string fromInputChar;
            string toInputChar;
            string playAgainInput;
            string playAgainForFewerTurns;
            bool playAgain;
            bool validToInput;
            bool quitGame;
            bool playToBeatScore;

            do
            {
                //Reset from previous game
                playAgain = true;
                validToInput = false;
                quitGame = false;
                playToBeatScore = false;

                Write("How many disks would you like to start with? Min is 1, Max is 9 --> ");
                numberOfDiscs = int.Parse(ReadLine());
                if (numberOfDiscs > 9)
                {
                    throw new InvalidHeightException();
                }

                Towers myTowers = new Towers(numberOfDiscs);
                myTowers.ToArray();
                TowerUtilities.DisplayTowers(myTowers);
                myTowers.MinimumPossibleMoves = (int)Math.Pow(2, numberOfDiscs) - 1;

                do //quit game is false
                {
                    WriteLine("\nEnter the number of the source pole (1-3) or 'q' to quit.");
                    fromInputChar = ReadKey().KeyChar.ToString().ToUpper(); 
                    int.TryParse(fromInputChar, out fromInputInt);

                    if (fromInputInt == 1 || fromInputInt == 2 || fromInputInt == 3)
                    {
                        Stack<Stack<int>> j = myTowers.poleArray[fromInputInt - 1];
                        Stack<int> a = j.Peek();

                        if (a.Count == 0) // checks tower count for disks
                        {
                            WriteLine("\nThat tower is empty, select another tower (1-3).");
                        }
                        else // if tower is not empty the program continues
                        {
                            do // while 'to' input is invalid
                            {
                                WriteLine("\nEnter the number of the target pole (1-3).");
                                toInputChar = ReadKey().KeyChar.ToString().ToUpper();
                                int.TryParse(toInputChar, out toInputInt);
                                int p = a.Pop(); // is used below to compare disk size

                                if (toInputInt == 1 || toInputInt == 2 || toInputInt == 3)
                                {
                                    Stack<Stack<int>> q = myTowers.poleArray[toInputInt - 1];
                                    Stack<int> r = q.Peek();
                                    r.Push(34);
                                    r.Push(22);
                                    r.Push(90);
                                    if (r.Count >= 1)
                                    {
                                        int k = r.Pop();
                                        if (p > k)
                                        {
                                            WriteLine("You can't stack a larger disk on top of a smaller disc. Try again.");
                                            validToInput = false;
                                        }
                                        else
                                        {
                                            myTowers.Move(fromInputInt, toInputInt);
                                            validToInput = true;
                                        }
                                    }
                                    else
                                    {
                                        myTowers.Move(fromInputInt, toInputInt);
                                        validToInput = true;
                                    }
                                }
                                else // if user entered an invalid 'to' pole 
                                {
                                    WriteLine("That is not a valid tower, please select another tower (1-3).");
                                    validToInput = false;
                                }
                            } while (!validToInput);
                        }
                    }
                    else if (fromInputChar == "Q")
                    {
                        quitGame = true;
                    }
                    else
                    {
                        WriteLine("\nNot a valid pole, please enter a pole from 1 - 3.");
                    }

                    // Endgame authentication
                    Stack<Stack<int>> endGameTower = myTowers.poleArray[2];
                    int eGT = endGameTower.Count();

                    //player completed the puzzle
                    if (eGT == numberOfDiscs)
                    {
                        if (myTowers.NumberOfMoves == myTowers.MinimumPossibleMoves)
                        {
                            WriteLine($"Congratulations! You completed the game in only {myTowers.MinimumPossibleMoves} moves, that's the least possible moves for {numberOfDiscs} disks!");
                        }
                        else
                        {
                            WriteLine($"You completed the puzzel in {myTowers.NumberOfMoves} moves but the fewest moves possible is {myTowers.MinimumPossibleMoves}.");
                            WriteLine("\nWhat do you say, play again and try to complete the puzzle in fewer turns? (Y or N)");
                            playAgainForFewerTurns = ReadKey().KeyChar.ToString().ToUpper();
                            if (playAgainForFewerTurns == "Y")
                            {
                                playToBeatScore = true;
                                quitGame = true;
                                playAgain = true;
                            }
                        }
                    }
                    //the puzzle is incomplete
                    else
                    {
                        WriteLine($"Move: {myTowers.NumberOfMoves}");
                        WriteLine($"\nYou moved from pole {fromInputInt} to pole {toInputInt}.");

                        TowerUtilities.DisplayTowers(myTowers);
                    }
                } while (!quitGame);

                if (playToBeatScore == false)
                {
                    WriteLine("Would you like to play again? (Y or N)");
                    playAgainInput = ReadKey().KeyChar.ToString().ToUpper();
                    if (playAgainInput == "Y") playAgain = true;
                    else playAgain = false;
                }

            } while (playAgain);

        }
    }
}
