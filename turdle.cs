// See https://aka.ms/new-console-template for more information

using System;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.VisualBasic;

namespace Variables
{
    class Program
    {
        static void Main(string[] args)
        {

            Random rnd = new Random();
            string[] SaveData = System.IO.File.ReadAllLines(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "data/savedata.txt"));
            int[] PreviousScores = SaveData.Select(int.Parse).ToArray();
            string[] Lines = System.IO.File.ReadAllLines(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "data/answerlist.txt"));
            string[] ValidAnswers = System.IO.File.ReadAllLines(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "data/valid_wordle_words.txt"));
            string answer = Lines[rnd.Next(Lines.Length)];
            char[] AnswerArray = answer.ToCharArray();
            string UserGuess = "aaaaa";
            char[] UserGuessArray = UserGuess.ToCharArray();
            bool Solved = false;
            int GuessNumber = 1;
            bool ValidGuess = false;
            int[] AnswerSolvedArray = new int[5];
            int[] ScoreBoard = new int[30];
            char[] AnswerGreenArray = AnswerArray;
            bool RunState = true;
            string PlayAgainString = null;
            char PlayAgainChar = 'y';
            char[] AllGuesses = new char[30];
            char[] Keyboard = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int[] KeyUsed = new int[26];
            int CurrentStreak = PreviousScores[0] + PreviousScores[1] + PreviousScores[2] + PreviousScores[3] + PreviousScores[4] + PreviousScores[5];
            int HighScore = PreviousScores[6];

            answer = "tarps";
            AnswerArray = answer.ToCharArray();

            DrawTurdle();
            ResetScoreboard();
            RunGame();
            ResetKeyboard();
            

            void RunGame()
            {
                Console.Clear();               

                do
                {
                    PrintBoard();
                    PlayAgainChar = 'y';
                    GuessReset();
                    PrintKeyboard();
                    getGuess(GuessNumber);
                    CheckGreenYellow();
                    ValidGuess = false;
                    DisplayGuess();
                    EndGame();
                } while (Solved == false && GuessNumber < 7 && RunState == true);
            }

            void DrawTurdle()
            {
                Console.WriteLine("");
                Console.Clear();
                Console.WriteLine("##########  ##.    ##.  #######.  #######.   ##.        ########");
                Console.WriteLine("    ##.     ##.    ##.  ##.   ##. ##.    ##. ##.        ##.");
                Console.WriteLine("    ##.     ##.    ##.  ##.   ##. ##.    ##. ##.        ##.");
                Console.WriteLine("    ##.     ##.    ##.  ##.  ##.  ##.    ##. ##.        ######");
                Console.WriteLine("    ##.     ##.    ##.  ## ##.    ##.    ##. ##.        ##.");
                Console.WriteLine("    ##.     ##.    ##.  ##. ##.   ##.    ##. ##.        ##.");
                Console.WriteLine("    ##.     ##.    ##.  ##.  ##.  ##.    ##. ##.        ##.");
                Console.WriteLine("    ##.     #########.  ##.   ##. #######.   #########  ########");
                Console.WriteLine("");
                WaitForAnyKeyPress();
            }

            void WaitForAnyKeyPress()
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Press any key to begin...");
                Console.ReadKey(true);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            void WelcomeMessage()
            {
                Console.Clear();

                for (int i = 0; i < 4; i++)
                {

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("");

                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("T");

                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("U");


                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("R");

                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("D");

                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("L");

                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("E");

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" ");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("");
                Console.WriteLine("");                    
            }

            void ResetScoreboard()
            {
                for (int i = 0; i < 30; i++)
                {
                    ScoreBoard[i] = 4;
                }
            }

            void ResetKeyboard()
            {
                for (int i = 0; i < 26; i++)
                {
                    KeyUsed[i] = 0;
                }
            }

            int FindKeyboardLocation(char LetterToCheck)
            {
                char upperCase = Char.ToUpper(LetterToCheck);
                int KeyIndex = 0;
                bool KeyGuessed = false;

                // find location of guessed letter on the keyboard
                for (int i = 0; i < 26; i++)
                {
                    if (upperCase == Keyboard[i])
                    {
                        KeyIndex = i;
                        KeyGuessed = true;
                    }
                }
                return KeyIndex;
            }

            // change keyboard index to print as green
            void ChangeToGreen(int Index)
            {
                KeyUsed[Index] = 1;
            }

            // change keyboard index to print as gray
            void ChangeToGray(int Index)
            {
                KeyUsed[Index] = 2;
            }

            // display the keyboard
            void PrintKeyboard()
            {
                Console.WriteLine();

                for (int i = 0; i < 26; i++)
                {
                    // print unused letters
                    if (KeyUsed[i] == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"{Keyboard[i]}");
                    }
                    // print green letters
                    else if (KeyUsed[i] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"{Keyboard[i]}");
                    }
                    // print gray letters
                    else if (KeyUsed[i] == 2)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write($"{Keyboard[i]}");
                    }
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("");
                Console.WriteLine("");

            }

            // get the word to guess from the list
            void GetChallange()
            {
                answer = Lines[rnd.Next(Lines.Length)];
            }


            void GuessReset()
            {
                AnswerArray = answer.ToCharArray();
                UserGuess = "aaaaa";
                UserGuessArray = UserGuess.ToCharArray();

                //  copy correct answer to temporary array to check
                for (int i = 0; i < 5; i++)
                {
                    AnswerGreenArray[i] = AnswerArray[i];
                }
                for (int i = 0; i < 5; i++)
                {
                    AnswerSolvedArray[i] = 0;
                }

            }

            void displayTurns(int i)
            {
                if (i < 6)
                {
                    Console.WriteLine($"You have {7 - i} guesses remaining.");
                }
                else if (i == 6)
                {
                    Console.WriteLine($"You have {7 - i} guess remaining.");
                }
                else
                {
                    Console.WriteLine("You have no guesses remaining.");
                }
            }

            void getGuess(int guess)
            {

                while (ValidGuess == false)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    displayTurns(guess);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Guess? (5 letters): ");
                    UserGuess = Console.ReadLine().ToLower();
                    UserGuessArray = UserGuess.ToCharArray();
                    ValidGuessCheck();
                    Console.WriteLine("");
                }

            }

            // add current guess to the array of all previous gusses
            void appendAllGuesses()
            {
                for (int i = 0; i < 5; i++)
                {
                    AllGuesses[(i + ((GuessNumber - 1) * 5))] = UserGuessArray[i];
                }

            }

            // check if a guess is a valid by comparing to list of valid words
            void ValidGuessCheck()
            {
                for (int k = 0; k < ValidAnswers.Length; k++)
                {
                    if (ValidAnswers[k] == UserGuess)
                    {
                        ValidGuess = true;
                        appendAllGuesses();
                        break;
                    }
                    else
                    {
                        ValidGuess = false;
                    }
                }
                if (ValidGuess == false)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid word.");
                }

            }

            void CheckGreenYellow()
            {
                bool InWord = false;

                // check if green
                for (int i = 0; i < 5; i++)
                {
                    int loc = FindKeyboardLocation(UserGuessArray[i]);

                    if (UserGuessArray[i] == AnswerGreenArray[i])
                    {
                        AnswerSolvedArray[i] = 1;
                        AnswerGreenArray[i] = '-';
                        ScoreBoard[(i + ((GuessNumber - 1) * 5))] = 1;
                        InWord = true;
                        ChangeToGreen(loc);
                    }

                }

                // check if yellow
                for (int i = 0; i < 5; i++)
                {
                    int loc = FindKeyboardLocation(UserGuessArray[i]);

                    for (int j = 0; j < 5; j++)
                    {
                        if (UserGuessArray[i] == AnswerGreenArray[j] && AnswerGreenArray[i] != '-')
                        {
                            AnswerSolvedArray[i] = 2;
                            AnswerGreenArray[j] = '&';
                            ScoreBoard[(i + ((GuessNumber - 1) * 5))] = 2;
                            InWord = true;
                            ChangeToGreen(loc);
                        }
                        else if (ScoreBoard[(i + ((GuessNumber - 1) * 5))] == 4)
                            ScoreBoard[(i + ((GuessNumber - 1) * 5))] = 0;

                    }
                }

                // if the letter was guessed but not in the puzzle, set keyboard to gray
                for (int i = 0; i < 5; i++)
                {
                    int loc = FindKeyboardLocation(UserGuessArray[i]);

                    if (KeyUsed[loc] == 0)
                    {
                        ChangeToGray(loc);
                    }
                }
            }

            void ResetPreviousScores()
            {
                for (int i = 0; i < 6; i++)
                {
                    PreviousScores[i] = 0;
                    SaveData[i] = "0";
                }

            }

            void PrintBoard()
            {
                Console.Clear();
                WelcomeMessage();

                for (int i = 0; i < 30; i++)
                {
                    if (i % 5 == 0)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(" ");
                        }
                    }
                    if (ScoreBoard[i] == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(AllGuesses[i]);

                        if ((i + 1) % 5 == 0 && i > 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("");
                        }

                    }
                    else if (ScoreBoard[i] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(AllGuesses[i]);
                        if ((i + 1) % 5 == 0 && i > 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("");
                        }
                    }
                    else if (ScoreBoard[i] == 2)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(AllGuesses[i]);
                        if ((i + 1) % 5 == 0 && i > 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("");
                        }
                    }
                    else if (ScoreBoard[i] == 4)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("·");
                        if ((i + 1) % 5 == 0 && i > 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("");
                        }

                    }





                }
            }

            void DisplayGuess()
            {
                Console.Clear();
                PrintBoard();

                for (int i = 0; i < (GuessNumber * 5); i++)
                {

                    if (answer == UserGuess)
                    {

                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"You guessed correctly!");
                        Console.WriteLine($"Score this round: {GuessNumber}");
                        GuessDistribution(GuessNumber);
                        Solved = true;
                        PlayAgainYN();
                        break;
                    }



                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                GuessNumber = GuessNumber + 1;

            }

            void GuessDistribution(int Guess)
            {
                PreviousScores[Guess - 1] = PreviousScores[Guess - 1] + 1;
                SaveData[Guess - 1] = PreviousScores[Guess - 1].ToString();

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                CurrentStreak = CurrentStreak + 1;
                Console.WriteLine($"\nCurrent Streak: {CurrentStreak}");
                Console.Write($"Longest Streak: ");
                if (CurrentStreak > HighScore)
                {
                    Console.WriteLine(CurrentStreak);
                    HighScore = CurrentStreak;
                    PreviousScores[6] = CurrentStreak;
                }
                else
                { 
                    Console.WriteLine(HighScore);
                }


                for (int i = 0; i < 6; i++)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" ");
                    Console.Write($"{i + 1}: ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{PreviousScores[i]} ");
                    for (int j = 0; j < PreviousScores[i]; j++)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write(" ");
                    }
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" ");
                Console.WriteLine(" ");


            }

            void EndGame()
            {

                if (answer == UserGuess && GuessNumber < 7)
                {
                    Solved = true;
                    PlayAgainYN();
                }
                else if (answer != UserGuess && GuessNumber == 7)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine($"The word was {answer}.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("");
                    ResetPreviousScores();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.BackgroundColor = ConsoleColor.Black;
                    PlayAgainYN();
                }

            }

            void SaveDataOnQuit()
            {

                Console.WriteLine("Saving Data...");

                for (int i = 0; i < PreviousScores.Length; i++)
                {
                    SaveData[i] = PreviousScores[i].ToString();
                }

                File.WriteAllLines(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "data/savedata.txt"), SaveData);
            }

            bool CheckValidYN(string YN)
            {
                bool IsValid = false;

                if (YN != null)
                {
                    IsValid = true;
                }

                else
                {
                    IsValid = false;
                    PlayAgainYN();
                }

                return IsValid;
            }

            void PlayAgainYN()
            {
                do
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("Play again? [Y]/[N]: ");
                    bool ValidYN = true;

                    PlayAgainString = Console.ReadLine().ToLower();
                    if (PlayAgainString.Length > 0)
                    {
                        ValidYN = CheckValidYN(PlayAgainString);
                    }
                    else
                        PlayAgainYN();

                    if (ValidYN == true)
                    {
                        PlayAgainChar = PlayAgainString[0];
                    }
                    Console.WriteLine("");



                    if (PlayAgainChar == 'y')
                    {
                        RunState = true;
                        Solved = false;
                        GuessNumber = 1;
                        PlayAgainChar = 'y';
                        GuessReset();
                        GetChallange();

                        for (int i = 1; i < 5; i++)
                        {
                            AnswerSolvedArray[i] = 0;
                        }

                        Array.Clear(ScoreBoard, 0, 30);
                        Array.Clear(AllGuesses, 0, 30);

                        ResetKeyboard();
                        ResetScoreboard();
                        RunGame();
                    }
                    else if (PlayAgainChar == 'n')
                    {
                        RunState = false;
                        Solved = true;
                        QuitGame();
                    }
                    else
                        PlayAgainYN();



                } while (PlayAgainChar != 'y' || PlayAgainChar != 'n');



            }

            void QuitGame()
            {
                SaveDataOnQuit();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine($"Thanks for playing.");
                System.Environment.Exit(1);
            }
        }
    }
}
