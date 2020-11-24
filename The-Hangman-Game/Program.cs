using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace The_Hangman_Game
{

    class Program
    {


        static void Main(string[] args)
        {

            hangMan();

           void hangMan() {

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                //introduction

                Console.Title = ("HangMan Game");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Welcome To HangMan Game\n\n");
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("The rules are simple. Try to guess the state capitals.\nYou can guess a whole word or a letter.");
                Console.WriteLine("If you don't guess the letter you will lose one life.\nIf you don't guess the word you will lose two.\nGood luck!\n\n");

                string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Seweryn\source\repos\The-Hangman-Game\countries_and_capitals.txt.txt");

                Random random = new Random();

                string guess = lines[random.Next(lines.Length)];

                //separation of the downloaded words

                string[] wordToGuess = guess.Split(new string[] { "| " }, StringSplitOptions.None);

                wordToGuess[1] = wordToGuess[1].ToUpper();

                //assigning characters to arrays

                int lives = 5;
                int counter = -1;
                int wordLength = wordToGuess[1].Length;
                char[] secretLetter = wordToGuess[1].ToCharArray();
                char[] printLetter = new char[wordLength];
                char[] guessedLetters = new char[100];
                int number = 0;
                bool victory = false;

                foreach (char letter in printLetter)
                {
                    counter++;
                    printLetter[counter] = '-';
                }

                //creating an application model

                while (lives > 0)
                {
                    counter = -1;
                    string printProgress = String.Concat(printLetter);
                    bool letterFound = false;
                    int multiples = 0;

                    if (printProgress == wordToGuess[1])
                    {
                        victory = true;
                        break;
                    }

                    if (lives > 1)
                    {
                        Console.WriteLine("You have {0} lives!\n", lives);

                    }
                    else
                    {
                        Console.WriteLine("You only have {0} life left!\n", lives);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("HINT: The capital of {0}\n", wordToGuess[0]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    //getting a word or a letter from the user

                    Console.WriteLine("current progress: " + printProgress);
                    Console.Write("\n\n\n");
                    Console.Write("guess a whole word or a letter: ");
                    string playerGuess = Console.ReadLine();

                    //testing the length of the entered character

                    while (playerGuess.Length != wordToGuess[1].Length && playerGuess.Length != 1)
                    {
                        Console.WriteLine("Please enter a whole word or only a single letter!");
                        Console.Write("Guess a letter: ");
                        playerGuess = Console.ReadLine();
                    }


                    //testing the correctness of guessing words

                    playerGuess = playerGuess.ToUpper();
                    Boolean correct = String.Equals(wordToGuess[1], playerGuess);

                    if (correct == true)
                    {
                        victory = true;
                        break;
                    }
                    else if (correct == false && playerGuess.Length == wordToGuess[1].Length)
                    {
                        Console.WriteLine("That's wrong word!");
                        lives--;
                        lives--;
                        Console.WriteLine(drawHnagMan(lives));
                        continue;

                    }
                    else if (guessedLetters.Contains(Convert.ToChar(playerGuess)) == false)
                    {
                        guessedLetters[number] = Convert.ToChar(playerGuess);
                        number++;

                        foreach (char letter in secretLetter)
                        {
                            counter++;
                            if (letter == Convert.ToChar(playerGuess))
                            {
                                printLetter[counter] = Convert.ToChar(playerGuess);
                                letterFound = true;
                                multiples++;
                            }
                        }
                        if (letterFound)
                        {
                            Console.WriteLine("Found {0} letter {1}!", multiples, Convert.ToChar(playerGuess));
                        }
                        else
                        {
                            Console.WriteLine("No letter {0}!", Convert.ToChar(playerGuess));
                            lives--;
                        }
                        Console.WriteLine(drawHnagMan(lives));

                    }
                    else
                    {
                        Console.WriteLine("You already guessed {0}!!", Convert.ToChar(playerGuess));
                    }

                    // letters used

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Used letters: ");
                   for(int i = 0; i < number; i++)
                    {
                        Console.WriteLine(guessedLetters[i]);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }

                //win and lose

                if (victory)
                {
                    Console.WriteLine("\n\nThe word was: {0}", wordToGuess[1]);
                    Console.WriteLine("\n\nYOU WIN!");

                    //creating data and saving to file

                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}",  ts.Minutes, ts.Seconds);

                    Console.WriteLine("You guessed after " + number + " letters" + " and " + elapsedTime + " sec");

                    string path = @"High_Score.txt";
                    StreamWriter sw;
                    if (!File.Exists(path))
                    {
                        sw = File.CreateText(path);
                    }
                    else
                    {
                        sw = new StreamWriter(path, true);
                    }
                    DateTime thisDay = DateTime.Today;
                    Console.WriteLine("name: ");
                    string name = Console.ReadLine();


                    sw.WriteLine("Name: " + name + " | " + "Data: "  + thisDay.ToString("d") + " | " + "Guessing time[min]: " + elapsedTime 
                        + " | " + "Guessing tries: " + number  + " | " + "Word to guess: " + wordToGuess[1]);

                    sw.Close();

                    StreamReader sr = File.OpenText(path);
                    string s = "";
                    int i = 1;
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(i++ + ". " + s);
                    }
                    sr.Close();
                    end();

                }
                else
                {
                    Console.WriteLine("\n\nThe word was: {0}", wordToGuess[1]);
                    Console.WriteLine("\n\nYOU LOSE!");
                    end();
                }
            }


            //application restart

             void end()
            {
                Console.WriteLine("Do you want to play one more time? Y/N?");
                string game = Console.ReadLine();
                char game1 = Convert.ToChar(game);
                if (game1 == 'y' || game1 == 'Y')
                    hangMan();
                else if (game1 == 'n' || game1 == 'N') { }
                else
                {
                    Console.WriteLine("Wrong char, try again!"); 
                    end();
                }
            }
          
        }

        //drawing of a hangman

        private static string drawHnagMan(int livesLeft)
        {
           

            string drawHangman = "";

            if (livesLeft < 5)
            {
                drawHangman += "--------\n";
            }

            if (livesLeft < 4)
            {
                drawHangman += "       |\n";
            }

            if (livesLeft < 3)
            {
                drawHangman += "       O\n";
            }

            if (livesLeft < 2)
            {
                drawHangman += "      /|\\ \n";
            }

            if (livesLeft == 0)
            {
                drawHangman += "      / \\ \n";
            }

            return drawHangman;

        }

        


    }
}