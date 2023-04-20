using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp
{
    public class EditorSaveResult
    {
        public EditorSaveResultTypeEnum EditorSaveResultType { get; set; }        
    }
}