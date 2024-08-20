    using System.IO;
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

    namespace WpfApp1
    {
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public partial class MainWindow : Window
        {
            private string filePath = "FilmListe.csv";
            private string visningFilePath = "OversigtVisninger.csv";

        public MainWindow()
        {
            //Loadfilms og Loadvisninger tjekkes ved opstart så ændringer i dokument er opdateret
            InitializeComponent();
            LoadFilmsFromFile();
            LoadVisningerFromFile();
        }


        //Metode der håndterer når bruger skal vælge biograf
        private void cbBiograf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbSal.IsEnabled = true;
            cbSal.Items.Clear();

            // Alle biografer bliver tildelt 3 sale. 
            if (cbBiograf.SelectedItem != null)
            {
                cbSal.Items.Add("Sal 1");
                cbSal.Items.Add("Sal 2");
                cbSal.Items.Add("Sal 3");
            }

            cbSpilletid.IsEnabled = true;
            cbFilm.IsEnabled = true;
        }

        //
        private void Planlæg_Click(object sender, RoutedEventArgs e)
        {   
            //laver streng ud fra det der bliver valgt i comboboxxxx
            string biograf = cbBiograf.Text;
            string sal = cbSal.Text;
            string spilletid = cbSpilletid.Text;
            string film = cbFilm.Text;

            //Samler info til enkelt streng
            string visningInfo = $"Biograf: {biograf}; Sal: {sal}; Tid: {spilletid}; Film: {film}";
            
            //tilføj til fanen "Oversigt"
            oversigtListBox.Items.Add(visningInfo);

            //gem visning til fil
            SaveVisningToFile(visningInfo);

            // Bekræftelse
            MessageBox.Show($"Planlagt visning: \n{visningInfo}");
        }


        //metode der indlæser liste af film fra csv og filføjer til listbox
        private void LoadFilmsFromFile()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                //for løkke der springer overskrift over og læser derefter
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split(';');

                    //tilføjer som streng og tilføjer så de kan ses i listbox
                    string filmInfo = $"{data[0]} - {data[1]} minutter - {data[2]}";
                    filmListBox.Items.Add(filmInfo);

                    // Tilføj filmene til "Planlægning" fanen
                    cbFilm.Items.Add(data[0]);
                }
            }
        }

        //bruger visninginfo streng fra "Planlæg_Click" til at skrive til fil
        private void SaveVisningToFile(string visningInfo)
        {
            using (StreamWriter sw = new StreamWriter(visningFilePath, true))
            {
                sw.WriteLine(visningInfo);
            }
        }


        //Indlæser hvad der er af planlagte film fra filen - og tilføjer til listbox
        private void LoadVisningerFromFile()
        { 
            if (File.Exists(visningFilePath))
            {
                string[] lines = File.ReadAllLines(visningFilePath);

                foreach (string line in lines)
                {
                    oversigtListBox.Items.Add(line);
                }
            } 
        }


        //Metode til at oprette ny film
        private void TilføjFilm_Click(object sender, RoutedEventArgs e)
        {
            string titel = tbTitel.Text;
            string varighed = tbVarighed.Text;
            string genre = tbGenre.Text;

            //samler til streng og tilføjer til listbox
            string filmInfo = $"{titel} - {varighed} minutter - {genre}";
            filmListBox.Items.Add(filmInfo);

            //rydder felter efter film er tilføjet
            tbTitel.Clear();
            tbVarighed.Clear();
            tbGenre.Clear();

            //hvis ikke fil eksisterer oprettes den, med overskrif.Hvis fil eksisterer tilføjes den nye film 
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                if (new FileInfo(filePath).Length == 0)
                {
                    sw.WriteLine("Titel,Varighed (Minutter),Genre");
                }
                sw.WriteLine($"{titel};{varighed};{genre}");
            }

            cbFilm.Items.Add(titel);
        }

        //metode til ryd knap 
        private void Ryd_Click(object sender, RoutedEventArgs e)
        {
            tbTitel.Clear();
            tbVarighed.Clear();
            tbGenre.Clear();
        }
    }
}