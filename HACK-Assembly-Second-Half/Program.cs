using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HACK_Assembly_Second_Half
{
    class Program
    {
        private static readonly string tempFilePrep = "./TempPrepFile.txt";
        private static readonly string tempFileBit = "./TempBitFile.txt";

        public static string InputFilePath { get; private set; }
        public static string TempFilePrep { get => tempFilePrep; }
        public static string TempFileBit { get => tempFileBit; }


        static void Main(string[] args)
        {
            StartMenu();

            // preps file ready for use
            PrepFile(new StreamReader(InputFilePath), new StreamWriter(TempFilePrep));

            BitConverter(new StreamReader(TempFilePrep), new StreamWriter(TempFileBit));

            Console.WriteLine("Done!!!!!");
        }

        private static void StartMenu()
        {
            bool validFile = false;

            Console.WriteLine("You are about to enroll on a great journey from your assembler code to binay");

            do
            {
                // user information
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Path to Your File :");
                Console.ForegroundColor = ConsoleColor.White;

                // file path
                string filePath = Console.ReadLine();

                // normalise the string input
                filePath = filePath.Trim();
                filePath = filePath.ToLower();

                // file check
                if (ValidFile(filePath))
                {
                    // end loop
                    validFile = true;
                    InputFilePath = filePath;
                }

            } while (!validFile);
        }



        static bool ValidFile(string path)
        {
            if (path.Contains(".txt") && File.Exists(path))
            {
                return true;
            }
            return false;
        }



        private static void BitConverter(StreamReader reader, StreamWriter writer)
        {
            // TODO: make stream reader read from file one line at a time
            try
            {
                string line;
                int lineNum = 1;

                while ((line = reader.ReadLine()) != null)
                {
                    // if the line contains @ it is a A Instrution
                    // else it is a C instruction
                    if (line.Contains("@"))
                    {
                        line = line.Replace("@", "");
                        line = ConvertToBits(line);
                    }
                    else
                    {
                        StringBuilder bitString = new StringBuilder("111");

                        // remove the jump part of the string
                        string lineWithoutJump = line;
                        if (line.Contains(";"))
                        {
                            lineWithoutJump = line.Remove(line.IndexOf(";"));
                        }

                        if (lineWithoutJump.Contains("="))
                        {
                            string[] temp;

                            temp = lineWithoutJump.Split('=');

                            bitString.Append(DestinationBits(temp[0]));

                            bitString.Append(CompBits(temp[1]));
                        }
                        else
                        {
                            bitString.Append(DestinationBits(lineWithoutJump));
                            bitString.Append(CompBits(""));
                        }

                        if (line.Contains(";"))
                        {
                            string[] temp = line.Split(";");

                            bitString.Append(JumpBits(temp[1]));
                        }
                        else
                        {
                            bitString.Append(JumpBits(""));
                        }

                        // write line
                        line = bitString.ToString();
                    }

                    writer.WriteLine(line);
                    lineNum++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry file could not be read");
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
                writer.Close();
            }


            // TODO: look at the first line and chech for @ 
            // @ = A-instruction
        }



        /// <summary>
        /// removes file whitespace
        /// </summary>
        private static void PrepFile(StreamReader reader, StreamWriter writer)
        {
            try
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Replace(" ", "");
                    writer.WriteLine(line);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry file could not be read");
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
                writer.Close();
            }
        }



        private static string InstructBits(string instruct)
        {
            return instruct;
        }



        private static string CompBits(string comp)
        {
            string aBits = "";
            string cBits = "";

            switch (comp)
            {
                // a=0
                case "0":
                    aBits = "0";
                    cBits = "101010";
                    break;

                case "1":
                    aBits = "0";
                    cBits = "111111";
                    break;

                case "-1":
                    aBits = "0";
                    cBits = "111010";
                    break;

                case "D":
                    aBits = "0";
                    cBits = "001100";
                    break;

                case "A":
                    aBits = "0";
                    cBits = "110000";
                    break;

                case "!D":
                    aBits = "0";
                    cBits = "001101";
                    break;

                case "!A":
                    aBits = "0";
                    cBits = "110001";
                    break;

                case "-D":
                    aBits = "0";
                    cBits = "001111";
                    break;

                case "-A":
                    aBits = "0";
                    cBits = "110011";
                    break;

                case "D+1":
                    aBits = "0";
                    cBits = "011111";
                    break;

                case "A+1":
                    aBits = "0";
                    cBits = "110111";
                    break;

                case "D-1":
                    aBits = "0";
                    cBits = "001110";
                    break;

                case "A-1":
                    aBits = "0";
                    cBits = "110010";
                    break;

                case "D+A":
                    aBits = "0";
                    cBits = "000010";
                    break;

                case "D-A":
                    aBits = "0";
                    cBits = "010011";
                    break;

                case "A-D":

                    aBits = "0";
                    cBits = "000111";
                    break;

                case "D&A":
                    aBits = "0";
                    cBits = "000000";
                    break;

                case "D|A":
                    aBits = "0";
                    cBits = "010101";
                    break;

                    // a=1
                case "M":
                    aBits = "1";
                    cBits = "110000";
                    break;

                case "!M":
                    aBits = "1";
                    cBits = "110001";
                    break;

                case "-M":
                    aBits = "1";
                    cBits = "110011";
                    break;

                case "M+1":
                    aBits = "1";
                    cBits = "110111";
                    break;

                case "M-1":
                    aBits = "1";
                    cBits = "110010";
                    break;

                case "D+M":
                    aBits = "1";
                    cBits = "000010";
                    break;

                case "D-M":
                    aBits = "1";
                    cBits = "010011";
                    break;

                case "M-D":
                    aBits = "1";
                    cBits = "000111";
                    break;

                case "D&M":
                    aBits = "1";
                    cBits = "000000";
                    break;

                case "D|M":
                    aBits = "1";
                    cBits = "010101";
                    break;

                case "":
                    aBits = "1";
                    cBits = "000000";
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error Invalid Comp Input not handeled");
                    Console.WriteLine($"<<<<<<<<<<<{comp}>>>>>>>>>>>");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            return aBits + cBits;
        }



        private static string DestinationBits(string destination)
        {
            switch (destination)
            {
                case "":
                    destination = "000";
                    break;

                case "M":
                    destination = "001";
                    break;

                case "D":
                    destination = "010";
                    break;

                case "MD":
                    destination = "011";
                    break;

                case "A":
                    destination = "100";
                    break;

                case "AM":
                    destination = "101";
                    break;

                case "AD":
                    destination = "110";
                    break;

                case "AMD":
                    destination = "111";
                    break;

                case "0":
                    destination = "000";
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error Invalid Destination Input not handeled");
                    Console.WriteLine($"<<<<<<<<<<<{destination}>>>>>>>>>>>");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            return destination;
        }



        private static string JumpBits(string jump)
        {
            switch (jump)
            {
                case "":
                    jump = "000";
                    break;

                case "JGT":
                    jump = "001";
                    break;

                case "JEQ":
                    jump = "010";
                    break;

                case "JGE":
                    jump = "011";
                    break;

                case "JLT":
                    jump = "100";
                    break;

                case "JNE":
                    jump = "101";
                    break;

                case "JLE":
                    jump = "110";
                    break;

                case "JMP":
                    jump = "111";
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error Invalid Comp Input not handeled");
                    Console.WriteLine($"<<<<<<<<<<<{jump}>>>>>>>>>>>");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            return jump;
        }



        private static string ConvertToBits(string value)
        {
            // temp vars
            int toBase = 2;
            StringBuilder bitString = new StringBuilder();

            string temp = Convert.ToString(Convert.ToUInt16(value), toBase);

            for (int i = 0; i < (16 - temp.Length); i++)
            {
                bitString.Append('0');
            }

            if (temp.Length > 16)
            {
                Console.WriteLine("Nope");
            }
            bitString.Append(temp);

            return bitString.ToString();
        }
    }
}
