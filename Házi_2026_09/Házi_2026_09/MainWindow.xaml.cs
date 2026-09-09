using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Házi_2026_09
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
            KPegyenleg.Content = kpegyenleg.ToString("N0") + " Ft";
            EURegyenleg.Content = euregyenleg.ToString("N0") + " EUR";

            //Logolás
            log.AppendText($"[{DateTime.Now:HH:mm:ss}] Indítás kezdete. \n");
        }

        //Alapérték beállítása
        public int bankiegyenleg = 1500000;
        public int kpegyenleg = 300000;
        public int euregyenleg = 0;

        public int eur_to_huf = 360;
        public int vanehitel = 0;
        int tét = 0;

        string tetDeviza = "HUF";
        int tartozas = 0;

        // FELVÉTEL GOMB
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(getkp.Text, out int felvet) && felvet > 0)
            {
                if (bankiegyenleg >= felvet)
                {

                    bankiegyenleg -= felvet;
                    kpegyenleg += felvet;


                    bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
                    KPegyenleg.Content = kpegyenleg.ToString("N0") + " Ft";

                    log.AppendText($"[{DateTime.Now:HH:mm:ss}] Felvétel: {felvet:N0} Ft\n");
                    log.ScrollToEnd();

                    getkp.Clear();
                }

                else
                {
                    MessageBox.Show("Nincs elég pénz a számládon!");
                }
            }
            else if (felvet == 0 || felvet < 0)
            {
                MessageBox.Show("Miért csinálnád ezt?");
            }


        }

        // BEFIZETÉS GOMB
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(addkp.Text, out int befizet) && befizet > 0)
            {
                if (kpegyenleg >= befizet)
                {

                    kpegyenleg -= befizet;
                    bankiegyenleg += befizet;


                    bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
                    KPegyenleg.Content = kpegyenleg.ToString("N0") + " Ft";

                    log.AppendText($"[{DateTime.Now:HH:mm:ss}] Befizet: {befizet:N0} Ft\n");
                    log.ScrollToEnd();

                    addkp.Clear();
                }

                else
                {
                    MessageBox.Show("Nincs elég pénz nálad!");
                }
            }

            else if (befizet == 0 || befizet < 0)
            {
                MessageBox.Show("Miért csinálnád ezt?");
            }


        }

        // DEVIZAVÁLTÁS GOMB
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // HUF - EUR
            if (valuta.SelectedIndex == 0)
            {
                if (int.TryParse(addvaluta.Text, out int valtas) && valtas >= eur_to_huf)
                {

                    if (bankiegyenleg < valtas)
                    {
                        MessageBox.Show("Nincs elég forintod a váltáshoz");
                        return;
                    }
                    int eur = (int)(valtas / eur_to_huf);
                    bankiegyenleg -= valtas;
                    euregyenleg += eur;

                    bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
                    EURegyenleg.Content = euregyenleg.ToString("N0") + " EUR";
                    log.AppendText($"[{DateTime.Now:HH:mm:ss}] Forint átváltása Euróra: {valtas:N0} Ft -> {eur} EUR\n");

                    addvaluta.Clear();
                }
                else
                {
                    MessageBox.Show("Minimum egy eurót tudsz váltani. (360FT)");
                }
            }



            //EUR - HUF

            else if (valuta.SelectedIndex == 1)
            {
                if (int.TryParse(addvaluta.Text, out int eurValtas) && eurValtas > 0)
                {
                    if (euregyenleg < eurValtas)
                    {
                        MessageBox.Show("Nincs ennyi euród az számládon!");
                        return;
                    }

                    int huf = eurValtas * eur_to_huf;

                    euregyenleg -= eurValtas;
                    bankiegyenleg += huf;

                    bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
                    EURegyenleg.Content = euregyenleg.ToString("N0") + " EUR";

                    log.AppendText($"[{DateTime.Now:HH:mm:ss}] Euró átváltása Forintra: {eurValtas:N0} EUR -> {huf:N0} Ft\n");
                    log.ScrollToEnd();

                    addvaluta.Clear();

                }
                else
                {
                    MessageBox.Show("Adj meg legalább 1 EUR-t a visszaváltáshoz!");
                }


            }
            else
            {
                MessageBox.Show("Válassz ki egy opciót");
            }


        }

        //HITEL 
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int ossz = bankiegyenleg + kpegyenleg + (euregyenleg * eur_to_huf);
            int min = 2500000;

            if (vanehitel == 1)
            {
                MessageBox.Show("Már van aktív hiteled, újat csak a jelenlegi törlesztésével tudsz felvenni.");
                return;
            }

            if (ossz >= min)
            {
                if (hitel.SelectedIndex == 0)
                {
                    int hitelOsszeg = 5000000;
                    bankiegyenleg += hitelOsszeg;

                    bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
                    log.AppendText($"[{DateTime.Now:HH:mm:ss}] Hitel felvéve: {hitelOsszeg:N0} Ft\n");
                    log.ScrollToEnd();

                    vanehitel++;
                    tartozas = hitelOsszeg;
                    hiteltorL.Content = $"{hitelOsszeg:N0}";
                }
                else if (hitel.SelectedIndex == 1)
                {
                    int hitelOsszeg = 10000000;
                    bankiegyenleg += hitelOsszeg;

                    bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
                    log.AppendText($"[{DateTime.Now:HH:mm:ss}] Hitel felvéve: {hitelOsszeg:N0} Ft\n");
                    log.ScrollToEnd();

                    vanehitel++;
                    tartozas = hitelOsszeg;
                    hiteltorL.Content = $"{hitelOsszeg:N0}";
                }
                else if (hitel.SelectedIndex == 2)
                {
                    int hitelOsszeg = 15000000;
                    bankiegyenleg += hitelOsszeg;

                    bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
                    log.AppendText($"[{DateTime.Now:HH:mm:ss}] Hitel felvéve: {hitelOsszeg:N0} Ft\n");
                    log.ScrollToEnd();

                    vanehitel++;
                    tartozas = hitelOsszeg;
                    hiteltorL.Content = $"{hitelOsszeg:N0}";
                }
                else if (hitel.SelectedIndex == 3)
                {
                    int hitelOsszeg = 20000000;
                    bankiegyenleg += hitelOsszeg;

                    bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
                    log.AppendText($"[{DateTime.Now:HH:mm:ss}] Hitel felvéve: {hitelOsszeg:N0} Ft\n");
                    log.ScrollToEnd();

                    vanehitel++;
                    tartozas = hitelOsszeg;
                    hiteltorL.Content = $"{hitelOsszeg:N0}";
                }
                else if (hitel.SelectedIndex == 4)
                {
                    int hitelOsszeg = 25000000;
                    bankiegyenleg += hitelOsszeg;

                    bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
                    log.AppendText($"[{DateTime.Now:HH:mm:ss}] Hitel felvéve: {hitelOsszeg:N0} Ft\n");
                    log.ScrollToEnd();

                    vanehitel++;
                    tartozas = hitelOsszeg;
                    hiteltorL.Content = $"{hitelOsszeg:N0}";
                }
                else if (hitel.SelectedIndex == 5)
                {
                    int hitelOsszeg = 30000000;
                    bankiegyenleg += hitelOsszeg;

                    bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
                    log.AppendText($"[{DateTime.Now:HH:mm:ss}] Hitel felvéve: {hitelOsszeg:N0} Ft\n");
                    log.ScrollToEnd();

                    vanehitel++;
                    tartozas = hitelOsszeg;
                    hiteltorL.Content = $"{hitelOsszeg:N0}";
                }
                else
                {
                    MessageBox.Show("Válassz ki egy opciót!");
                }
            }
            else
            {
                MessageBox.Show($"A hitelfelvételhez minimum {min} FT kell");
            }
        }

        
      

        //Tipp játék
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (tét <= 0)
            {
                MessageBox.Show("Előbb fogadj egy összegben.", "Hiányzó tét", MessageBoxButton.OK);
                return;
            }

            if (!int.TryParse(tippeltszam.Text, out int tipp) || tipp < 0 || tipp > 100)
            {
                MessageBox.Show("Kérlek 0-100 között adj meg egy számot!");
                return;
            }

            if (tetDeviza == "HUF")
            {
                if (bankiegyenleg < tét)
                {
                    MessageBox.Show($"Nincs elegendő forintod ehhez a pörgetéshez! Jelenlegi forint egyenleged: {bankiegyenleg:N0} Ft", "Kevés Forint egyenleg", MessageBoxButton.OK);
                    return;
                }
                bankiegyenleg -= tét;
            }
            else if (tetDeviza == "EUR")
            {
                if (euregyenleg < tét)
                {
                    MessageBox.Show($"Nincs elegendő euród ehhez a pörgetéshez! Jelenlegi euró egyenleged: {euregyenleg:N0} EUR", "Kevés EUR egyenleg", MessageBoxButton.OK);
                    return;
                }
                euregyenleg -= tét;
            }

            Random rnd = new Random();
            int sorsolt = rnd.Next(0, 101);

            kapottszamL.Content = sorsolt.ToString();

            int kulonbseg = Math.Abs(tipp - sorsolt);
            int szorzo = 0;

            if (kulonbseg == 0)
            {
                szorzo = 30; 
            }
            else if (kulonbseg <= 5)
            {
                szorzo = 7; 
            }
            else if (kulonbseg <= 10)
            {
                szorzo = 3;  
            }

            if (szorzo > 0)
            {
                int nyeremeny = tét * szorzo;

                if (tetDeviza == "HUF")
                {
                    bankiegyenleg += nyeremeny;
                }
                else
                {
                    euregyenleg += nyeremeny;
                }

                string uzenet = kulonbseg == 0
                    ? $"TELITALÁLAT! ({sorsolt})"
                    : $"KÖZEL VOLTÁL! A sorsolt szám {sorsolt} volt (eltérés: {kulonbseg}).";

                MessageBox.Show($"{uzenet}\nNyereményed ({szorzo}x): {nyeremeny:N0} {tetDeviza}", "Nyertél!", MessageBoxButton.OK);
                log.AppendText($"[{DateTime.Now:HH:mm:ss}] Szerencsejáték NYEREMÉNY ({szorzo}x): +{nyeremeny:N0} {tetDeviza} (Tétel: {tét:N0} {tetDeviza}, Tipp: {tipp}, Sorsolt: {sorsolt})\n");
            }
            else
            {
                MessageBox.Show($"Sajnos nem talált. A kisorsolt szám: {sorsolt}\nVeszítettél {tét:N0} {tetDeviza}-t.", "Veszítettél", MessageBoxButton.OK);
                log.AppendText($"[{DateTime.Now:HH:mm:ss}] Szerencsejáték VERESÉG: -{tét:N0} {tetDeviza} (Tipp: {tipp}, Sorsolt: {sorsolt})\n");
            }

            bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
            EURegyenleg.Content = euregyenleg.ToString("N0") + " EUR";

            tét = 0;

            log.ScrollToEnd();
            tippeltszam.Clear();
            kapottszamL.Content = "";
        }

        // TÉT megrakása
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (tét > 0)
            {
                MessageBox.Show($"Már van egy aktív fogadásod ({tét:N0} {tetDeviza})! Előbb pörgesd le a kört.", "Már fogadtál", MessageBoxButton.OK);
                return;
            }

            if (fogaddeviza.SelectedIndex == -1)
            {
                MessageBox.Show("Kérlek előbb válaszd ki a devizát a legördülőből!", "Nincs deviza kiválasztva", MessageBoxButton.OK);
                return;
            }

            if (int.TryParse(fogadas.Text, out int megadottTet) && megadottTet > 0)
            {
                               
                if (fogaddeviza.SelectedIndex == 0)
                {
                    if (megadottTet < 1000)
                    {
                        MessageBox.Show("Forint esetén a minimum beszálló 1000 Ft!", "Hibás tét", MessageBoxButton.OK);
                        return;
                    }

                    if (bankiegyenleg >= megadottTet)
                    {
                        tét = megadottTet;
                        tetDeviza = "HUF";
                        MessageBox.Show($"A tét sikeresen beállítva: {tét:N0} Ft", "Tét beállítva", MessageBoxButton.OK);
                        log.AppendText($"[{DateTime.Now:HH:mm:ss}] Új tét beállítva: {tét:N0} Ft\n");
                        log.ScrollToEnd();
                        fogadas.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Nincs ennyi forintod a számládon!", "Kevés forint egyenleg", MessageBoxButton.OK);
                    }
                }
                else if (fogaddeviza.SelectedIndex == 1)
                {
                    if (megadottTet < 1)
                    {
                        MessageBox.Show("Euró esetén a minimum beszálló 1 EUR!", "Hibás tét", MessageBoxButton.OK);
                        return;
                    }

                    if (euregyenleg >= megadottTet)
                    {
                        tét = megadottTet;
                        tetDeviza = "EUR";
                        MessageBox.Show($"A tét sikeresen beállítva: {tét:N0} EUR", "Tét beállítva", MessageBoxButton.OK);
                        log.AppendText($"[{DateTime.Now:HH:mm:ss}] Új tét beállítva: {tét:N0} EUR\n");
                        log.ScrollToEnd();
                        fogadas.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Nincs ennyi euród a számládon!", "Kevés EUR egyenleg", MessageBoxButton.OK);
                    }
                }
            }
            else
            {
                MessageBox.Show("Adj meg egy érvényes számot tétnek!", "Hibás tét", MessageBoxButton.OK);
            }
        }

        // HITEL Törlesztés
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (vanehitel == 0 || tartozas <= 0)
            {
                MessageBox.Show("Jelenleg nincs aktív hiteltartozásod!");
                return;
            }

            if (!int.TryParse(torlesztosszeg.Text, out int torlesztendo) || torlesztendo <= 0)
            {
                MessageBox.Show("Kérlek, adj meg egy érvényes törlesztési összeget!");
                return;
            }

            int minTorleszto = (int)(tartozas * 0.10); 

            if (torlesztendo < minTorleszto && torlesztendo < tartozas)
            {
                MessageBox.Show($"A minimálisan törlesztendő összeg a hitel 10%-a: {minTorleszto:N0} Ft!");
                return;
            }

            if (torlesztendo > tartozas)
            {
                torlesztendo = tartozas;
            }

            int osszVagyon = bankiegyenleg + kpegyenleg + (int)(euregyenleg * eur_to_huf);
            if (osszVagyon < torlesztendo)
            {
                MessageBox.Show("Nincs elegendő pénzed a kívánt törlesztőrészlet kifizetésére!");
                return;
            }

            int levonando = torlesztendo;

            if (bankiegyenleg >= levonando)
            {
                bankiegyenleg -= levonando;
                levonando = 0;
            }
            else
            {
                levonando -= bankiegyenleg;
                bankiegyenleg = 0;

                if (kpegyenleg >= levonando)
                {
                    kpegyenleg -= levonando;
                    levonando = 0;
                }
                else
                {
                    levonando -= kpegyenleg;
                    kpegyenleg = 0;

                    int szuksegesEur = (int)levonando / eur_to_huf;
                    euregyenleg -= szuksegesEur;
                    levonando = 0;
                }
            }

            tartozas -= torlesztendo;

            log.AppendText($"[{DateTime.Now:HH:mm:ss}] Hitel törlesztve: {torlesztendo:N0} Ft (Maradék: {tartozas:N0} Ft)\n");

            if (tartozas <= 0)
            {
                vanehitel = 0;
                tartozas = 0;
                hiteltorL.Content = "0 Ft";

            }
            else
            {
                hiteltorL.Content = $"{tartozas:N0} Ft";
            }

            bankegyenleg.Content = bankiegyenleg.ToString("N0") + " Ft";
            KPegyenleg.Content = kpegyenleg.ToString("N0") + " Ft";
            EURegyenleg.Content = euregyenleg.ToString("N0") + " €";
            log.ScrollToEnd();
            torlesztosszeg.Clear();
        }

    }

}
