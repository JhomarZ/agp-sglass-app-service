namespace AGPSnowden.Service.TestPDF
{
    public class TestPDFService
    {
        public FileStream GetFileStrem(string file)
        {
            string fullPath = Path.GetFullPath(file);
            FileStream fs = File.Open(fullPath, FileMode.Open);
            //FileStream fs = File.Open("C:\\Users\\rzenteno\\OneDrive - AGP GROUP\\5.0 TI\\Proyectos\\17.0 Snowden\\AGPSnowden\\ConsolePresentation\\logo.jpg", FileMode.Open);
            return fs;
        }
    }
}
