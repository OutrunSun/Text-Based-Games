using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Text_Based_Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.Write("HANGMAN - C# CONSOLE EDITION");
            Console.Write("\nBy Noah Ramdhony");
            Config();
            Console.Write("\nThanks for playing");
        }

        static void Config()
        {
            Console.Write("\nHow many players: ");
            int playerCount = Convert.ToInt32(Console.ReadLine());

            string selectedWord = "";

            if (playerCount > 1)
            {
                Console.Write("Input a word: ");
                selectedWord = Console.ReadLine().ToUpper();
            } 
            else 
            {
                selectedWord = WordGeneration();
            }

            GameLoop(selectedWord);
        }

        static string WordGeneration()
        {
            // Initialises the Variables needed for random line
            Random index = new Random();
            string word = "";

            using (var reader = new StreamReader(@"C:\Users\Njbra\Documents\@ Programming\C#\Text Based Hangman\words.txt"))
            {
                // Cycles through all lines before the one selected from the random
                for (int i = 1; i < index.Next(1, 10000); i++)
                {
                    reader.ReadLine();
                }

                // Sets the selected word to a variable
                word = reader.ReadLine().ToUpper();
            }
            return word;
        }

        static void GameLoop(string word)
        {
            // Defining Variables used
            int lives = 9;
            List<char> alphabet = new List<char>();
            List<string> wordsGuessed = new List<string>();
            int winStatus = 0;

            do
            {  
                Renderer(word, lives, alphabet, wordsGuessed, winStatus);
                if (lives == 0)
                {
                    winStatus = -1;
                }
                else
                {
                    Console.Write("LIVES:   " + lives);

                    Console.Write("\nInput Guess: ");
                    string guess = Console.ReadLine().ToUpper();
                    char selection;
                    if (guess.Length == 1)
                    {
                        selection = 'L';
                    }
                    else
                    {
                        selection = 'W';
                    }

                    switch (selection)
                    {
                        case 'W':
                            if (guess == word)
                            {
                                winStatus = 1; // 1 = Win, -1 = Loss, 0 = Playing
                            }
                            else
                            {
                                lives--;
                                wordsGuessed.Add(guess);
                            }

                            break;

                        case 'L':
                            if (word.IndexOf(guess) == -1)
                            {
                            lives--;
                            }
                            alphabet.Add(Convert.ToChar(guess));
                            break;
                    }
                }

            }while (winStatus == 0);
            Renderer(word, lives, alphabet, wordsGuessed, winStatus);
            
        }

        static void Renderer(string word, int lives, List<char> lettersGuesses, List<string> wordGuesses, int winStatus)
        {
            Console.Clear();

            // The Title:
            Console.Write("HANGMAN - C# Console\n");

            // The Drawing itself
            if (lives < 8)
            {
                Console.Write("    +---+\n");
            }
            else
            {
                Console.Write("\t\n");
            }
            switch (lives)
            {
                case 0:
                    Console.Write("    |   |\n    O   |\n   /|\\  |\n   / \\  |\n        |\n");
                    break;

                case 1:
                    Console.Write("    |   |\n    O   |\n   /|\\  |\n   /    |\n        |\n");
                    break;
                
                case 2:
                    Console.Write("    |   |\n    O   |\n   /|\\  |\n        |\n        |\n");
                    break;
                
                case 3:
                    Console.Write("    |   |\n    O   |\n   /|   |\n        |\n        |\n");
                    break;

                case 4:
                    Console.Write("    |   |\n    O   |\n    |   |\n        |\n        |\n");
                    break;
                
                case 5:
                    Console.Write("    |   |\n    O   |\n        |\n        |\n        |\n");
                    break;
                
                case 7:
                    Console.Write("        |\n        |\n        |\n        |\n        |\n");
                    break;
                
                case 6:
                    Console.Write("    |   |\n        |\n        |\n        |\n        |\n");
                    break;
                
                case 8:
                    Console.Write("        |\n        |\n        |\n        |\n        |\n");
                    break;
                
                case 9:
                    Console.Write("\t \t\n\t \t\n\t \t\n\t \t\n\t \t\n");
                    break;
            }
            Console.Write("   ======\n\n ");
            
            // The Word display:
            if (winStatus == 1)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    Console.Write(word[i] + " ");
                }
            }
            else
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (lettersGuesses.Contains(word[i]))
                    {
                        Console.Write(word[i] + " ");
                    }
                    else
                    {
                        Console.Write("_ ");
                    }
                }
            }
            Console.Write("\n");

            // Letters Guessed Display:
            Console.Write("\nLETTERS: ");
            foreach (char letter in lettersGuesses)
            {
                Console.Write(letter);
            }
            Console.Write("\n");

            // Words Guessed Display
            Console.Write("WORDS:   ");
            foreach (string currentWord in wordGuesses)
            {
                Console.Write(currentWord);
            }
            Console.Write("\n");

            // Win Display:
            switch (winStatus)
            {
                case 1:
                    Console.Write("\n! Y O U   W O N !");
                    break;
                
                case -1:
                    Console.Write("\n! Y O U   L O S T !");
                    Console.Write("\nThe word was: " + word);
                    break;
            }
        }
    }
}
