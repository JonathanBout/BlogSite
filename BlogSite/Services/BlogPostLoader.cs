using BlogSite.Data;
using LiteDB;
using Microsoft.AspNetCore.Components;
namespace BlogSite.Services
{
    public class BlogPostLoader
    {
        ILiteCollection<BlogPost> BlogPosts { get; }
        public BlogPostLoader(Database database)
        {
            BlogPosts = database.GetCollection<BlogPost>();
#if DEBUG
            BlogPosts.DeleteAll();
            var generatedPosts = new BlogPost[Random.Shared.Next(11, 100)];
            for (int i = 0; i < generatedPosts.Length; i++)
            {
                generatedPosts[i] = new BlogPost(
                    Random.Shared.Paragraph(3, 7) + " [TEST]",
                    DateTime.Now - TimeSpan.FromTicks(Random.Shared.NextInt64(1, (long)1e14 * 4)),
                    Random.Shared.Paragraph(),
                    Random.Shared.Paragraphs());
            }
            BlogPosts.InsertBulk(generatedPosts);
#endif
        }

        public BlogPost[] GetPosts(DateTime? startDate = null, DateTime? endDate = null, string? searchText = null, int maxCount = 0)
        {
            var query = BlogPosts.Query().OrderByDescending(x => x.PublishedOn);
            if (!string.IsNullOrEmpty(searchText))
                query = query.Where(item => item.ToString().Contains(searchText));
            if (endDate is not null)
                query = query.Where(x => x.PublishedOn.Date <= endDate.Value.Date);
            if (startDate is not null)
                query = query.Where(x => x.PublishedOn.Date >= startDate.Value.Date);

            //should be the last as Limit() does not return an ILiteQueryable<T>.
            if (maxCount > 0)
                return query.Limit(maxCount).ToArray();

            return query.ToArray();
        }

        public BlogPost GetBlogPost(int id)
        {
            return BlogPosts.FindById(id);
        }
    }
}
