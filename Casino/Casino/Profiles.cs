using Casino;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Casino
{
    internal class Profiles
    {
        //
        // trida pro obstaravani zalezitosti kolem profilu, zapis a cteni dat o profilech, spravujici penezenku apod
        //

        public List<List<string>> profilesData = new List<List<string>>();
        private string path = @"profiles.txt";

        public int currentId;
        public int currentWallet;
        public string currentName = "";

        public int currentBet;

        private void GetProfilesData()
        {
            //
            // metoda ktera provadi cteni ze souboru
            // prectena data uklada do atributu profilesData
            //
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
                    if (line != null)
                    {
                        string[] words = line.Split(",");
                        this.profilesData.Add(new List<string> { words[0], words[1] });
                    }
                }
                sr.Close();
            }
            catch (Exception)
            {
                this.FileError();
            }
        }
        public bool AnyProfileExists()
        {
            //
            // metoda vracejici bool podle toho, jestli jsou v souboru nejaka data
            //
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
            //
            // metoda overujici, zda existuje uz nejaky profil se stejnym username - pouziva se napr pri vytvareni noveho profilu, kdy se overuje, zda username existuje a muze ho uzivatel mit
            //
            // username = retezec, ktery ma reprezentovat nove vyvareny username, ten ktery chceme overit
            //
            bool result = false;
            foreach (List<string> profile in this.profilesData)
            {
                if (profile[0].ToLower() == username.ToLower()) return true;
            }
            return result;
            
        }
        public bool ProfileSelect()
        {
            //
            // metoda, ktera nejdrive vypise vsechna ulozena data o profilech a da na vyber uzivateli z techto profilu - na jaky profil se chce prihlasit
            //
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
                        // po vyberu na jaky profil se chce uzivatel prihlasit, se data ulozi do atributu tridy, abychom nadale vedeli, jaky profil aktualne aplikaci pouziva
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
            //
            // metoda, ktera se spusti pri vytvoreni noveho profilu a zvoleni moznosti, kdy se uzivatel chce prihlasit na tento novy profil
            //
            int count = this.profilesData.Count - 1;
            this.currentId = count;
            this.currentName = this.profilesData[count][0];
            this.currentWallet = int.Parse(this.profilesData[count][1]);
        }
        public void DeleteAccounts()
        {
            //
            // metoda mazajici celou databazi profilu
            //
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
            //
            // metoda, ktera obnovuje data v tom smyslu, ze vymaze ulozena data a ze souboru je nacte znovu
            //
            this.profilesData.Clear();
            this.GetProfilesData();
        }
        public int ProfileCreate()
        {
            //
            // metoda, ktera se stara o vyvoreni noveho profilu
            // overuje validitu vstupu na zaklade pravidel predlozenych uzivateli
            //
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
                            // vse je ok = profil se ulozi
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
            //
            // metoda, ukladajici data o novem profilu
            //
            try
            {
                StreamWriter sw = new StreamWriter(this.path, true);
                sw.WriteLine("{0},5000", username);
                sw.Close();
            }
            catch (Exception)
            {
                this.FileError();
            }
        }
        public bool AcceptBet()
        {
            //
            // metoda, ktera prijima sazky od uzivatele
            // nejdrive se zepta kolik chce vsadit
            //
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
                        // zde je overeni, zda sazka neni nula, a ze uzivatel takovou sumu ma opravdu na ucte
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
            //
            // metoda ukladajici data o sazce (jak do souboru tak do lokalnich atributu)
            //
            this.currentBet = bet;
            string text = File.ReadAllText(this.path);
            int currentWalletNew = this.currentWallet - bet;
            text = text.Replace(string.Format("{0},{1}", this.currentName, this.currentWallet), string.Format("{0},{1}", this.currentName, currentWalletNew));
            this.currentWallet = currentWalletNew;
            File.WriteAllText(this.path, text);
        }
        public void CalculateWinnings(int x = 2)
        {
            //
            // metoda, ktera vypocitava, jak jednotlive hry dopadly pro uzivatele = kolik vyhral
            //
            // x = jakym cislem chceme vyhru nasobit - pri vetsine sazek se dvojnasobi, proto je to jako defaultni hodnota, ale napr pri sazce na zelenou v rulete se 14x 
            //
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
            //
            // kosmeticka metoda informujici o chybe pri praci se souborem
            //
            Console.WriteLine("Během práce se souborem došlo k chybě, zkuste program restartovat a vypnout soubory, do kterých se data ukládají!");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
