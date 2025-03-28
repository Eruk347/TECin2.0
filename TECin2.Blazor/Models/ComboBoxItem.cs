using Microsoft.AspNetCore.Mvc.Rendering;

namespace TECin2.Blazor.Models
{
    public class ComboBoxItem(int _id)
    {
        public string Id { get; set; } = _id.ToString();
        public List<SelectListItem> SelectList { get; set; } = [];
    }
}
