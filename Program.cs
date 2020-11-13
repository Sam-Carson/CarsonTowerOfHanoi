using ClassLibrary;
using System;
using static ClassLibrary.Towers;
using static System.Console;

namespace CarsonTowerOfHanoi
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfDiscs;
            string fromInput;
            string toInput;
            bool playAgain = true;
            bool validFromInput = false;
            bool validToInput = false;

            do
            {
                Write("How many disks would you like to start with? Min is 1, Max is 9 --> ");
                numberOfDiscs = int.Parse(ReadLine());
                while (numberOfDiscs > 9)
                {
                    throw new InvalidHeightException();
                }


                Towers myTowers = new Towers(numberOfDiscs);
                myTowers.ToArray();
                TowerUtilities.DisplayTowers(myTowers);

                do
                {
                    WriteLine("Enter the number of the source pole (1-3) or 'q' to quit.");
                    fromInput = ReadKey().KeyChar.ToString().ToUpper();
                    if (fromInput == "1" || fromInput == "2" || fromInput == "3")
                    {
                        do
                        {
                            WriteLine("Enter the number of the source pole (1-3) or 'q' to quit.");
                            toInput = ReadKey().KeyChar.ToString().ToUpper();
                            if (toInput == "1" || toInput == "2" || toInput == "3")
                            {

                            }

                        } while (!validToInput);
                        validFromInput = true;
                    }
                    else if (fromInput == "Q") validFromInput = true;
                    else validFromInput = false;


                } while (!validFromInput);

            } while (playAgain);

        }
    }
}
