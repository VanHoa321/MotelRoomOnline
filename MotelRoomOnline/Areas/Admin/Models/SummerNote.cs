namespace MotelRoomOnline.Areas.Admin.Models
{
    public class SummerNote
    {
        public SummerNote(string idEdittor, bool loadLibrary = true)
        {
            IDEditor = idEdittor;
            LoadLibrary = loadLibrary;
        }
        public string IDEditor { get; set; }
        public bool LoadLibrary { get; set; }
        public int Height { get; set; } = 700;
        public string toolBar { get; set; } = @"
             [
             ['style', ['style']],
             ['font', ['bool','underline','clear']],
             ['color', ['color']],
             ['para', ['ul','ol','paragraph']],
             ['table', ['table']],
             ['insert', ['link','elfinderFiles','video','elfinder']],
             ['view', ['fullscreen','codeview','help']]
             ]
        ";
    }
}

