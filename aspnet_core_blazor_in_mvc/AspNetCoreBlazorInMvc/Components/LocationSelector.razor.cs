using AspNetCoreBlazorInMvc.Data;
using AspNetCoreBlazorInMvc.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreBlazorInMvc.Components
{
    public partial class LocationSelector : ComponentBase
    {
        [Inject]
        private AppDbContext Context { get; set; } = null!;

        private List<Prefecture> prefectures = new();
        private List<City> cities = new();

        private int? selectedPrefectureId;
        private int? selectedCityId;

        private string SelectionLabel => GetSelectionLabel();

        protected override async Task OnInitializedAsync()
        {
            prefectures = await Context.Prefectures.OrderBy(p => p.Id).ToListAsync();
        }

        private async Task OnPrefectureChange(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var prefId))
            {
                selectedPrefectureId = prefId;
                selectedCityId = null;
                cities = await Context.Cities
                    .Where(c => c.PrefectureId == prefId)
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
            else
            {
                selectedPrefectureId = null;
                selectedCityId = null;
                cities = new();
            }
        }

        private void OnCityChange(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var cityId))
            {
                selectedCityId = cityId;
            }
            else
            {
                selectedCityId = null;
            }
        }

        private string GetSelectionLabel()
        {
            if (selectedPrefectureId == null) return "未選択";
            
            var pref = prefectures.FirstOrDefault(p => p.Id == selectedPrefectureId);
            if (pref == null) return "未選択";

            if (selectedCityId == null) return "未選択";
            
            var city = cities.FirstOrDefault(c => c.Id == selectedCityId);
            if (city == null) return "未選択";

            return $"{pref.Name} {city.Name}";
        }
    }
}
