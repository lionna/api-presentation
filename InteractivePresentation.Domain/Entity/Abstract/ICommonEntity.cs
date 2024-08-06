using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InteractivePresentation.Domain.Entity.Abstract
{
    public interface ICommonEntity<TKey>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        TKey Id { set; get; }
    }
}
