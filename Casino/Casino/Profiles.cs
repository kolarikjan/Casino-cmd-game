using Casino;
using System.IO;
using System.Text.RegularExpressions;

namespace Casino
{
    internal class Profiles
    {
        private string path = @"profiles.txt";

        public bool ProfileExist()
        {
            bool result = false;
            try
            {
                if (File.Exists(this.path))
                {
                    StreamReader sr = new StreamReader(this.path);

                    string data = sr.ReadToEnd();

                    if (data != string.Empty)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Během práce se souborem došlo k chybě, zkuste program restartovat!");
            }
            return result;
            
        }
        public void ProfileCreate()
        { 
            while (true)
            {
                Console.Write("Váš username: ");
                string? username = Console.ReadLine();
                if (username != null) 
                {
                    Regex pattern = new Regex("^[a-zA-Z0-9]*$");
                    if (pattern.IsMatch(username) && username.Length > 5)
                    {
                        if (File.Exists(this.path))
                        {
                            StreamReader sr = new StreamReader(this.path);

                            string data = sr.ReadToEnd();

                            if (data != string.Empty)
                            {
                                Console.WriteLine(data);

                                while(!sr.EndOfStream)
                                {
                                    string? line = sr.ReadLine();
                                    Console.WriteLine(line);
                                }

                            }
                            Console.ReadLine();
                        }
                        else
                        {
                            FileStream fileStream = File.Create(this.path);
                            fileStream.Close();
                        }
                    }
                }
            }
        }
        public void ProfileNewSave(string username)
        {
            try
            {
                StreamWriter sw = new StreamWriter(this.path, true); 
                sw.WriteLine("{0}, 500", username);
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Během práce se souborem došlo k chybě, zkuste program restartovat!");
                return;
            }
        }
    }
}
