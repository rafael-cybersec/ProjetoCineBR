    namespace CineBR.Models
{
    public class Classification
    {
        public List<ClassificationModel> Classifications { get; set; }
    }

    public class ClassificationModel
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Logo { get; set; }
    }
}
