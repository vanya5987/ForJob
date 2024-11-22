using Microsoft.EntityFrameworkCore;
using ShareYourText.Database;
using ShareYourText.Interfaces.UIManager;

namespace ShareYourText.UIManager
{
    public sealed class UI : IShowUI
    {
        public async Task ShowPopularityLinksAsync(ListView listView)
        {
            using (LinkDbContext context = new LinkDbContext())
            {
                var baseLinks = await context.BaseLinks
                    .OrderByDescending(link => link.PopularityPoint)
                    .Take(10)
                    .Select(link => new
                    {
                        link.LinkFormat,
                        link.ExpirationDate,
                        link.PopularityPoint
                    })
                    .ToListAsync();

                listView.Items.Clear();

                foreach (var link in baseLinks)
                {
                    ListViewItem item = new ListViewItem(link.LinkFormat);

                    item.SubItems.Add(link.ExpirationDate.ToString("g"));
                    item.SubItems.Add(link.PopularityPoint.ToString());
                    listView.Items.Add(item);
                }
            }
        }
    }
}

