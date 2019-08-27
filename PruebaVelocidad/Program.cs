using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaVelocidad
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Random number = new Random();
            int columns = 5;
            int rows = 3;
            String word;
            String line = "";
            String final = "";
            int[] sizes = new int[columns];
            int length;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    length = (sizes[j] == 0) ? number.Next(5, 20) : sizes[j];
                    word = p.GetWord(length, number);
                    line += (j != columns - 1) ? word + "," : word;
                    if (i == 0) sizes[j] = length;
                }
                final += (i + 1 == rows) ? line : line + "\n";
                line = "";
            }
            String path = @"C:\Users\Danilo Giron\Desktop\Practicas 2019\Registros.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(final);
                }
            }
            else if (File.Exists(path))
            {
                File.Delete(path);
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(final);
                }
            }

            Console.WriteLine("Archivo generado");

            string[] lines = p.lines(path);
            List<string[]> data = new List<string[]>();
            string actualLine;

            for (int i = 0; i < lines.Length; i++) {
                actualLine = lines[i];
                string[] linesValues = actualLine.Split(',').Select(val => "'" + val + "'").ToArray();
                data.Add(linesValues);
            }

            String campos = "";
            for(int i = 1; i <= columns; i++) {
                campos += (i == columns)? "'campo" + i + "'": "'campo" + i + "', ";
            }

            int times = 9;
            int total = data.Count;
            int count2 = 1;
            int cantidad = (total % times == 0) ? total / times : total / times + 1;
            int salto = 0;
            int sueltas = (total % times == 0)? 0: times - total % times;
            do
            {

                string start = "INSERT INTO (" + campos + ") VALUES ";

                string end = "";
                string single = "";
                string[] inf;
                for (int i = 0; i < times; i++){
                    int position = salto + i;
                    if (i + sueltas == times && count2 == cantidad && total % times != 0) break;
                    inf = data[position];
                    single = "(";
                    for (int j = 0; j < inf.Length; j++)
                    {
                        single += (j + 1 == inf.Length) ? inf[j] + ")" : inf[j] + ", ";
                    }
                    if(count2 == cantidad){
                        if (i + 1 + sueltas == times) end += single;
                        else end += single + ", ";
                    }else{
                        if (i + 1 == times) end += single;
                        else end += single + ", ";
                    }
                    single = "(";
                }
                salto += times;
                String finalSentence = start + end;

                Console.WriteLine("Sentencia No. " + count2 + "\n" + finalSentence + "\n");

                count2++;
            } while (cantidad >= count2);


            Console.Read();

        }

        public string GetWord(int length, Random random)
        {
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        public string[] lines(string path)
        {
            return File.ReadAllLines(path);
        }

    }
}
