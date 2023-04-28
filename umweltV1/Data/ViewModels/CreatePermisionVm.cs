namespace umweltV1.Data.ViewModels
{
    public class CreatePermisionVm
    {
        public string Title { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; }
        public IList<string> ParentTitle { get; set; }
    }
}
