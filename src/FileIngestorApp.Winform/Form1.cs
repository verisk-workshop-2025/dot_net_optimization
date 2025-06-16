using Bogus;
using FileIngestorApp.Core.Models;
using Newtonsoft.Json;

namespace FileIngestorApp.Winform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCreateFile_Click(object sender, EventArgs e)
        {
            try
            {
                int size = (int)numericUpDownNumberOfLines.Value;
                FileInfo file = new(textBoxFilePath.Text);
                if (file.Directory?.Exists == false)
                {
                    file.Directory.Create();
                    return;
                }
                File.WriteAllLines(file.FullName, GeneratePersonData(size).Select(x => JsonConvert.SerializeObject(x)));
                MessageBox.Show("File created successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to create file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private IEnumerable<Employee> GeneratePersonData(int size)
        {
            int[] salaries = { 1000, 2000, 3000, 5000, 10000 };

            List<Employee> persons = new Faker<Employee>()
                .RuleFor(x => x.Id, f => f.IndexGlobal)
                .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                .RuleFor(x => x.LastName, f => f.Name.LastName())
                .RuleFor(x => x.Salary, f => f.PickRandom(salaries))
                .Generate(size).ToList();

            return persons;
        }
    }
}
