using System;
using static System.Console;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class Towers
    {
        public int NumberOfDiscs { get; set; }
        public int NumberOfMoves { get; set; }
        public int MinimumPossibleMoves { get; set; } // 2^n -1
        public bool IsComplete { get; set; }

        private Stack<int> poleOne = new Stack<int>();
        private Stack<int> poleTwo = new Stack<int>();
        private Stack<int> poleThree = new Stack<int>();
        private Stack<Stack<int>>[] poleArray = new Stack<Stack<int>>[] { new Stack<Stack<int>>(), new Stack<Stack<int>>(), new Stack<Stack<int>>() };
  

        public Towers(int numberOfDiscs)
        {
            NumberOfDiscs = numberOfDiscs;

            for (int i = numberOfDiscs; i > 0; i--)
            {
                poleOne.Push(i);
            }
            
            poleArray[0].Push(poleOne);
            poleArray[1].Push(poleTwo);
            poleArray[2].Push(poleThree);
        }


        public void Move(int from, int to)
        {
            Stack<int> movedDisk;

            //for (int i = numberOfTowers; i < numberOfTowers; i++)
            //{
            //    Stack<int> pole = poleArray[i].Pop();
            //}


            if (from < 1 || from > 3 || to < 1 || to > 3) // validates input for poles
            {
                WriteLine("Not a valid tower! Towers are from left to right (1, 2, 3)");
            }
            else if (from == to) // prevents from choosing same pole to move from --> to
            {
                WriteLine("Move Cancelled");
            }
            else if (poleArray[from - 1].Count == 0) // Checks if pull is empty
            {
                WriteLine($"Tower {from - 1} is empty");
            }
            else if (Convert.ToInt32(poleArray[from - 1].Pop()) > Convert.ToInt32(poleArray[to - 1].Pop())) // compares 'from' disk to 'to' size
            {
                WriteLine($"Top disk of {from} is larger than the top disk on tower {to}.");
            }
            else
            {
                movedDisk = poleArray[from - 1].Pop();
                poleArray[to - 1].Push(movedDisk);
                NumberOfMoves++;
                if (poleArray[2].Count == NumberOfDiscs)
                {
                    WriteLine("Winner");
                    WriteLine($"Total number of moves {NumberOfMoves}.");
                }
            }
        }


        public int[][] ToArray()
        {
            int[][] jaggedArray = new int[3][];

            // because poleArray is of type Stack<Stack<int>> you must convert the first stack to an array and then do the same thing again that stack.

            for (int i = 0; i < poleArray.Length; i++)
            {
                Stack<int>[] s = poleArray[i].ToArray();
                jaggedArray[i] = s[0].ToArray();
            }

            return jaggedArray;
        }

        public class InvalidHeightException : Exception
        {
            public InvalidHeightException()
            {
                
            }

            public InvalidHeightException(string message) : base(message)
            {

            }

            public InvalidHeightException(string message, Exception inner) : base(message, inner)
            {

            }
        }

        public class InvalidMoveException : Exception
        {
            public InvalidMoveException()
            {

            }
            public InvalidMoveException(string message) : base(message)
            {

            }
            public InvalidMoveException(string message, Exception inner) : base(message, inner)
            {

            }
        }

    }
}

