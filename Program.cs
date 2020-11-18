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
            bool validInput = false;
            bool quitGame = false;
            bool playAgain = true;

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

            // Creates towers and sets MinimumPossibleMoves property.
            Towers myTowers = new Towers(numDiscs);
            myTowers.MinimumPossibleMoves = MinimumMoves(numDiscs);
            Clear();

            do
            {
                myTowers.ToArray();
                TowerUtilities.DisplayTowers(myTowers);

                // checks if game is complete, and if not states the previous move
                if (myTowers.NumberOfMoves > 0)
                {
                    if (!GameComplete(myTowers, numDiscs)) WriteLine($"Move {myTowers.NumberOfMoves} complete. Successfully moved disc from tower {from} to tower {to}");
                    else if (GameComplete(myTowers, numDiscs))
                    {
                        WriteLine($"Congratulations, you completed the puzzle in {myTowers.NumberOfMoves} moves.");
                        if (myTowers.MinimumPossibleMoves == myTowers.NumberOfMoves)
                        {
                            WriteLine($"\nThat's the fewest number of moves possible. I ANOINT YOU THE RULER OF HANOI!");
                        }
                        else
                        {
                            WriteLine($"\nYou complete the puzzle in {myTowers.NumberOfMoves} moves but the fewest possible is {myTowers.MinimumPossibleMoves}");
                            WriteLine("Let's give it another shot. What do you say? ('Y' for yes! or 'x' to quit): ");
                        }
                        string playAgainInput = ReadKey().KeyChar.ToString().ToUpper();
                        switch (playAgainInput)
                        {
                            case "Y":
                                playAgain = true;
                                break;
                            default:
                                playAgain = false;
                                break;
                        }
                    }
                }
                
                do
                {
                    do
                    {
                        validInput = false; // resets validInput

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

                    if (from != -1)
                    {
                        do
                        {
                            // gets 'to' pole
                            try
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
                    else quitGame = true;

                    myTowers.Move(from, to);

                } while (playAgain == true);

            } while (!quitGame);

        } // end Main

        public static int MoveFrom(/*bool displayCtrlY = true*/)
        {
            string validFromInput;
            int validFromInt;

            Write("Enter 'from' tower number or 'x' to quit: ");
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
            Clear();
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
            // will need to reset towers object
            return true;
        }

        // myTowers.IsComplete = GameComplete();
        public static bool GameComplete(Towers myTowers, int numberOfDiscs)
        {
            if (myTowers.poleThree.Count == numberOfDiscs)
            {
                myTowers.IsComplete = true;
                return true;
            }
            myTowers.IsComplete = false;
            return false;
        }

        //int numberOfDiscs;
        //int fromInputInt;
        //int toInputInt = 0;
        //string fromInputChar;
        //string toInputChar;
        //string playAgainInput;
        //string playAgainForFewerTurns;
        //bool playAgain;
        //bool validToInput;
        //bool quitGame;
        //bool playToBeatScore;

        //do
        //{
        //    //Reset from previous game
        //    playAgain = true;
        //    validToInput = false;
        //    quitGame = false;
        //    playToBeatScore = false;

        //    Write("How many disks would you like to start with? Min is 1, Max is 9 --> ");
        //    numberOfDiscs = int.Parse(ReadLine());
        //    if (numberOfDiscs > 9)
        //    {
        //        throw new InvalidHeightException();
        //    }

        //    Towers myTowers = new Towers(numberOfDiscs);
        //    myTowers.ToArray();
        //    TowerUtilities.DisplayTowers(myTowers);
        //    myTowers.MinimumPossibleMoves = (int)Math.Pow(2, numberOfDiscs) - 1;

        //    do //quit game is false
        //    {
        //        WriteLine("\nEnter the number of the source pole (1-3) or 'q' to quit.");
        //        fromInputChar = ReadKey().KeyChar.ToString().ToUpper(); 
        //        int.TryParse(fromInputChar, out fromInputInt);

        //        if (fromInputInt == 1 || fromInputInt == 2 || fromInputInt == 3)
        //        {
        //            //Stack<Stack<int>> j = myTowers.poleArray[fromInputInt - 1];
        //            //myTowers.Move(fromInputInt - 1, toInputInt);
        //            Stack<int> a = j.Peek();

        //            if (a.Count == 0) // checks tower count for disks
        //            {
        //                WriteLine("\nThat tower is empty, select another tower (1-3).");
        //            }
        //            else // if tower is not empty the program continues
        //            {
        //                do // while 'to' input is invalid
        //                {
        //                    WriteLine("\nEnter the number of the target pole (1-3).");
        //                    toInputChar = ReadKey().KeyChar.ToString().ToUpper();
        //                    int.TryParse(toInputChar, out toInputInt);
        //                    int p = a.Pop(); // is used below to compare disk size

        //                    if (toInputInt == 1 || toInputInt == 2 || toInputInt == 3)
        //                    {
        //                        //Stack<Stack<int>> q = myTowers.poleArray[toInputInt - 1];
        //                        //Stack<int> r = q.Peek();
        //                        //r.Push(34);
        //                        //r.Push(22);
        //                        //r.Push(90);
        //                        if (r.Count >= 1)
        //                        {
        //                            int k = r.Pop();
        //                            if (p > k)
        //                            {
        //                                WriteLine("You can't stack a larger disk on top of a smaller disc. Try again.");
        //                                validToInput = false;
        //                            }
        //                            else
        //                            {
        //                                myTowers.Move(fromInputInt, toInputInt);
        //                                validToInput = true;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            myTowers.Move(fromInputInt, toInputInt);
        //                            validToInput = true;
        //                        }
        //                    }
        //                    else // if user entered an invalid 'to' pole 
        //                    {
        //                        WriteLine("That is not a valid tower, please select another tower (1-3).");
        //                        validToInput = false;
        //                    }
        //                } while (!validToInput);
        //            }
        //        }
        //        else if (fromInputChar == "Q")
        //        {
        //            quitGame = true;
        //        }
        //        else
        //        {
        //            WriteLine("\nNot a valid pole, please enter a pole from 1 - 3.");
        //        }

        //        // Endgame authentication
        //        Stack<Stack<int>> endGameTower = myTowers.poleArray[2];
        //        int eGT = endGameTower.Count();

        //        //player completed the puzzle
        //        if (eGT == numberOfDiscs)
        //        {
        //            if (myTowers.NumberOfMoves == myTowers.MinimumPossibleMoves)
        //            {
        //                WriteLine($"Congratulations! You completed the game in only {myTowers.MinimumPossibleMoves} moves, that's the least possible moves for {numberOfDiscs} disks!");
        //            }
        //            else
        //            {
        //                WriteLine($"You completed the puzzel in {myTowers.NumberOfMoves} moves but the fewest moves possible is {myTowers.MinimumPossibleMoves}.");
        //                WriteLine("\nWhat do you say, play again and try to complete the puzzle in fewer turns? (Y or N)");
        //                playAgainForFewerTurns = ReadKey().KeyChar.ToString().ToUpper();
        //                if (playAgainForFewerTurns == "Y")
        //                {
        //                    playToBeatScore = true;
        //                    quitGame = true;
        //                    playAgain = true;
        //                }
        //            }
        //        }
        //        //the puzzle is incomplete
        //        else
        //        {
        //            WriteLine($"Move: {myTowers.NumberOfMoves}");
        //            WriteLine($"\nYou moved from pole {fromInputInt} to pole {toInputInt}.");

        //            TowerUtilities.DisplayTowers(myTowers);
        //        }
        //    } while (!quitGame);

        //    if (playToBeatScore == false)
        //    {
        //        WriteLine("Would you like to play again? (Y or N)");
        //        playAgainInput = ReadKey().KeyChar.ToString().ToUpper();
        //        if (playAgainInput == "Y") playAgain = true;
        //        else playAgain = false;
        //    }

        //} while (playAgain);

    }
}
