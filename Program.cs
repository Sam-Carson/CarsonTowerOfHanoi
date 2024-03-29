﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using static ClassLibrary.Towers;
using static System.Console;

namespace CarsonTowerOfHanoi
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool playAgain;
            do
            {
                int numDiscs = 0;
                int from = 0;
                int to = 0;
                bool validInput;
                bool endGame;
                bool askDisplayCtrlZ = false;
                bool askDisplayCtrlY = false;

                Queue<MoveRecord> recordedMovesQ = new Queue<MoveRecord>();
                Stack<MoveRecord> undoStack = new Stack<MoveRecord>();
                Stack<MoveRecord> redoStack = new Stack<MoveRecord>();

                Clear();
                do
                {
                    try
                    {
                        numDiscs = GetGameDiscs();
                        Clear();
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
                HowToPlay();

                do
                {
                    do
                    {
                        validInput = false;
                        endGame = false;// resets validInput
                        if (myTowers.NumberOfMoves > 0) askDisplayCtrlZ = true;
                        if (redoStack.Count > 0)
                        {
                            askDisplayCtrlY = true;
                        }

                        try // Gets 'from' pole
                        {
                            from = MoveFrom(askDisplayCtrlZ, askDisplayCtrlY, undoStack, redoStack);
                            if (from == -3 && undoStack.Count == 0)
                            {
                                WriteLine("\nCan't undo!");
                                validInput = false;
                            }
                            else if (from == -2 && redoStack.Count == 0)
                            {
                                WriteLine("\nCan't redo!");
                                validInput = false;
                            }
                            else validInput = true;
                        }
                        catch (InvalidMoveException e)
                        {
                            WriteLine(e.Message);
                        }
                    } while (validInput == false);

                    if (from < 0)
                    {
                        switch (from)
                        {
                            case -1: // quit
                                endGame = true;
                                break;
                            case -2: // redo
                                MoveRecord postRedoMove = Redo(undoStack, redoStack, myTowers, recordedMovesQ);
                                break;
                            case -3: // undo
                                MoveRecord postUndoMove = Undo(undoStack, redoStack, myTowers, recordedMovesQ);
                                break;
                        }
                    }
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
                        // Makes move an returns recorded move
                        // Pushes recordedMove to undoStack
                        // Adds recordedMove to recordedMovesQ
                        // Clears redoStack becuase user made a regular move
                        // Displays towers
                        // converts to array
                        // Checks if game is complete
                        // Ends game if game is complete

                        if (from > 0) // if the user made a regular move
                        {
                            MoveRecord recordedMove = myTowers.Move(from, to);
                            undoStack.Push(recordedMove);
                            redoStack.Clear();
                            recordedMovesQ.Enqueue(recordedMove);
                        }

                        Update(myTowers);
                        GameComplete(myTowers, from, to, undoStack, redoStack);
                        if (myTowers.IsComplete) endGame = true;
                    }

                } while (!endGame);

                // list turns
                DisplayMoves(recordedMovesQ);
                playAgain = PlayAgain();

            } while (playAgain);

        } // end Main

        public static int MoveFrom(bool askCtrlZ, bool askCtrlY, Stack<MoveRecord> pUndoStack, Stack<MoveRecord> pRedoStack)
        {
            // if the user has undone or redone the maximum number of turns, the below writeline updates
            if (pUndoStack.Count == 0) askCtrlZ = false;
            if (pRedoStack.Count == 0) askCtrlY = false;

            string ctrlOptionsY = askCtrlY == true ? " 'Ctrl+y' to redo," : "";
            string ctrlOptionsZ = askCtrlZ == true ? ", 'Ctrl+z' to undo," : "";


            Write($"\nEnter 'from' tower number{ctrlOptionsZ}{ctrlOptionsY} or 'x' to quit: ");
            ConsoleKeyInfo validFromInput = ReadKey();
            if (validFromInput.Key == ConsoleKey.X) // user inputs x
            {
                return -1;
            }
            else if (validFromInput.Modifiers == ConsoleModifiers.Control && validFromInput.Key == ConsoleKey.Y) // user inputs ctrl+y to redo
            {
                return -2;
            }
            else if (validFromInput.Modifiers == ConsoleModifiers.Control && validFromInput.Key == ConsoleKey.Z) // user inputs ctrl+z to undo
            {
                return -3;
            }
            else
            {
                int.TryParse(validFromInput.KeyChar.ToString(), out int validFromInt);
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

        public static void GameComplete(Towers pMyTowers, int pFrom, int pTo, Stack<MoveRecord> pUndoStack, Stack<MoveRecord> pRedoStack)
        {
            if (pMyTowers.IsComplete)
            {
                WriteLine($"\nCongratulations, you completed the puzzle in {pMyTowers.NumberOfMoves} moves.");
                if (pMyTowers.MinimumPossibleMoves == pMyTowers.NumberOfMoves) WriteLine($"\nThat's the fewest number of moves possible. I ANOINT YOU THE RULER OF HANOI!");
                else
                {
                    WriteLine($"\nYou completed the puzzle in {pMyTowers.NumberOfMoves} moves but the fewest possible is {pMyTowers.MinimumPossibleMoves}");
                    WriteLine("\nLet's give it another shot. What do you say?");
                }
            }
            else if (pFrom == -2) // redo
            {
                MoveRecord redoMoveDetails = pUndoStack.Peek();
                WriteLine($"\nMove {pMyTowers.NumberOfMoves} complete by redo of move {pMyTowers.NumberOfMoves - 1}. Disc {redoMoveDetails.Disc} restored to tower {redoMoveDetails.To} from tower {redoMoveDetails.From}.");
            }
            else if (pFrom == -3) // undo
            {
                MoveRecord undoMoveDetails = pRedoStack.Peek();
                WriteLine($"\nMove {pMyTowers.NumberOfMoves} complete by undo of move {undoMoveDetails.MoveNumber}. Disc {undoMoveDetails.Disc} restored to tower {undoMoveDetails.From} from tower {undoMoveDetails.To}.");
            }
            else
            {
                WriteLine($"\nMove {pMyTowers.NumberOfMoves} complete. Successfully moved disc {pUndoStack.Peek().Disc} from tower {pFrom} to tower {pTo}.");
            }
        }

        public static void Update(Towers pMyTowers)
        {
            TowerUtilities.DisplayTowers(pMyTowers);
            pMyTowers.ToArray();
        }

        public static void DisplayMoves(Queue<MoveRecord> pRecordedMoves)
        {
            Write("\nWould you like to see a list of moves? ('y' for yes): ");
            string recordedMovesInput = ReadKey().KeyChar.ToString().ToUpper();

            if (recordedMovesInput == "Y")
            {
                foreach (MoveRecord item in pRecordedMoves)
                {
                    WriteLine($"\nMove {item.MoveNumber}: You moved disc {item.Disc} from tower {item.From} to tower {item.To}");
                }
            }
        }

        public static MoveRecord Undo(Stack<MoveRecord> pUndoStack, Stack<MoveRecord> pRedoStack, Towers pMyTowers, Queue<MoveRecord> pRecordedMovesQ)
        {
            MoveRecord undoMoveRecord = pUndoStack.Pop();
            pRedoStack.Push(undoMoveRecord);
            MoveRecord postUndoMove = pMyTowers.Move(undoMoveRecord.To, undoMoveRecord.From);
            pRecordedMovesQ.Enqueue(postUndoMove);
            return postUndoMove;
        }

        public static MoveRecord Redo(Stack<MoveRecord> pUndoStack, Stack<MoveRecord> pRedoStack, Towers pMyTowers, Queue<MoveRecord> pRecordedMovesQ)
        {
            MoveRecord redoMoveRecord = pRedoStack.Pop();
            pUndoStack.Push(redoMoveRecord);
            MoveRecord postRedoMove = pMyTowers.Move(redoMoveRecord.From, redoMoveRecord.To);
            pRecordedMovesQ.Enqueue(postRedoMove);

            return postRedoMove;
        }

        public static void HowToPlay()
        {
            string hTPInput;
            bool validHTPInput;

            WriteLine("Options: ");
            WriteLine("- M - Solve the puzzle manually");
            WriteLine("- A - Auto-solve");

            do
            {
                Write("\nChoose an approach: ");
                hTPInput = ReadKey().KeyChar.ToString().ToUpper();

                if (hTPInput == "M" || hTPInput == "A") validHTPInput = true;
                else
                {
                    WriteLine("\nNot a valid Input!");
                    validHTPInput = false;
                }
            } while (!validHTPInput);
        }
    }
}
