using Casino;
using System.IO;
using System.Text.RegularExpressions;

namespace Casino
{
    internal class Profiles
    {
        public List<List<string>> profilesData = new List<List<string>>();
        private string path = @"profiles.txt";

        public int currentId;
        public int currentWallet;
        public string currentName = "";

        private void GetProfilesData()
        {
            try
            {
                if (!File.Exists(this.path))
                {
                    FileStream fileStream = File.Create(this.path);
                    fileStream.Close();
                }
                StreamReader sr = new StreamReader(this.path);
                while (!sr.EndOfStream)
                {
                    string? line = sr.ReadLine();
                    if (line != "")
                    {
                        string[] words = line.Split(",");
                        this.profilesData.Add(new List<string> { words[0], words[1] });
                    }
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Během práce se souborem došlo k chybě, zkuste program restartovat a vypnout soubory, do kterých se data ukládají!");
            }
        }
        public bool AnyProfileExists()
        {
            bool result = false;
            this.GetProfilesData();
            foreach (List<string> profile in this.profilesData)
            {
                result = true;
                break;
            }
            return result;
        }
        private bool CertainProfileExist(string username)
        {
            bool result = false;
            foreach (List<string> profile in this.profilesData)
            {
                if (profile[0].ToLower() == username.ToLower()) return true;
            }
            return result;
            
        }
        public bool ProfileSelect()
        {
            bool result = false;
            Ui.MenuLine();
            int i = 0;
            foreach (List<string> profile in this.profilesData)
            {
                Console.Write(string.Format("{0} - {1} (Peněženka: ${2})", i, profile[0], profile[1]));
                if (i + 1 != this.profilesData.Count) Console.Write("\n\n");
                i++;
            }
            Console.WriteLine();
            Ui.MenuLine();
            Console.Write("Vyberte účet, na který se chcete přihlásit (číslo): ");
            try
            {
                string? userInput = Console.ReadLine();
                int action = 0;
                if (userInput != null)
                {
                    action = int.Parse(userInput);
                    if (action > 0 && action <= i)
                    {
                        int RealId = action - 1;
                        this.currentId = RealId;
                        this.currentName = this.profilesData[RealId][0];
                        this.currentWallet = int.Parse(this.profilesData[RealId][1]);
                        result = true;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Během práce se souborem došlo k chybě, zkuste program restartovat a vypnout soubory, do kterých se data ukládají!");
            }
            return result;
        }
        public int ProfileCreate()
        {
            int result = 1;
            Console.Write("Zadejte název nového profilu: ");
            string? username = Console.ReadLine();
            if (username != null) 
            {
                Regex pattern = new Regex("^[a-zA-Z0-9]*$");
                if (pattern.IsMatch(username) && username.Length >= 5)
                {
                    if (File.Exists(this.path))
                    {
                        if(!this.CertainProfileExist(username))
                        {
                            this.ProfileNewSave(username);
                            result = 0;
                        }
                        else
                        {
                            result = 2;
                        }
                    }
                }
            }
            return result;
        }
        public void ProfileNewSave(string username)
        {
            try
            {
                StreamWriter sw = new StreamWriter(this.path, true);
                sw.WriteLine("{0},5000", username);
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Během práce se souborem došlo k chybě, zkuste program restartovat a vypnout soubory, do kterých se data ukládají!");
                return;
            }
        }
    }
}
