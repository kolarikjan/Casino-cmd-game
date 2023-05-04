using Casino;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
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

        public int currentBet;

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
                this.FileError();
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
            this.RefreshData();
            Ui.MenuLine();
            int i = 0;
            foreach (List<string> profile in this.profilesData)
            {
                Console.Write(string.Format("{0} - {1} (Peněženka: ${2})", i + 1, profile[0], profile[1]));
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
            catch(Exception)
            {
                this.FileError();
            }
            return result;
        }
        public void SwitchToNewAccount()
        {
            int count = this.profilesData.Count - 1;
            this.currentId = count;
            this.currentName = this.profilesData[count][0];
            this.currentWallet = int.Parse(this.profilesData[count][1]);
        }
        public void DeleteAccounts()
        {
            try
            {
                if (File.Exists(this.path))
                {
                    File.Delete(this.path);
                    this.profilesData.Clear();
                }
            }
            catch (Exception)
            {
                this.FileError();
            }
            

        }
        private void RefreshData()
        {
            this.profilesData.Clear();
            this.GetProfilesData();
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
                            this.GetProfilesData();
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
                this.FileError();
            }
        }
        public bool AcceptBet()
        {
            Console.WriteLine(string.Format("Aktuálně na účtě máte: ${0}", this.currentWallet));
            Ui.MenuLine();
            Console.Write("Kolik peněz chcete vsadit: ");
            bool result = false;
            try
            {
                string? input = Console.ReadLine();
                if (input != null)
                {
                    int bet = int.Parse(input);
                    if (bet > 0 && bet <= this.currentWallet)
                    {
                        Bet(bet);
                        result = true;
                    }
                }
            }
            catch (Exception)
            {
                this.FileError();
            }
            return result;
            
        }
        public void Bet(int bet)
        {
            this.currentBet = bet;
            string text = File.ReadAllText(this.path);
            int currentWalletNew = this.currentWallet - bet;
            text = text.Replace(string.Format("{0},{1}", this.currentName, this.currentWallet), string.Format("{0},{1}", this.currentName, currentWalletNew));
            this.currentWallet = currentWalletNew;
            File.WriteAllText(this.path, text);
        }
        public void CalculateWinnings(int x = 2)
        {
            try
            {
                string text = File.ReadAllText(this.path);
                int currentWalletNew = this.currentWallet + (this.currentBet * x);
                text = text.Replace(string.Format("{0},{1}", this.currentName, this.currentWallet), string.Format("{0},{1}", this.currentName, currentWalletNew));
                this.currentWallet = currentWalletNew;
                File.WriteAllText(this.path, text);
            }
            catch (Exception)
            {
                this.FileError();
            }
        }
        private void FileError()
        {
            Console.WriteLine("Během práce se souborem došlo k chybě, zkuste program restartovat a vypnout soubory, do kterých se data ukládají!");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
