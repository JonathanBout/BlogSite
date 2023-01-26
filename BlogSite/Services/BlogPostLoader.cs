using BlogSite.Data;
using LiteDB;
using LiteDB.Engine;
using Microsoft.AspNetCore.Components;
namespace BlogSite.Services
{
    public class BlogPostLoader
    {
        ILiteCollection<BlogPost> BlogPosts { get; }
        public BlogPostLoader(Database database)
        {
            BlogPosts = database.GetCollection<BlogPost>();
        }

        public BlogPost[] GetPosts(DateTime? startDate = null, DateTime? endDate = null, string? searchText = null, int maxCount = 0, int authorId = 0)
        {
            var query = BlogPosts.Query().OrderByDescending(x => x.PublishedOn);
            if (!string.IsNullOrEmpty(searchText))
                query = query.Where(item => item.ToString().Contains(searchText));
            if (endDate is not null)
                query = query.Where(x => x.PublishedOn.Date <= endDate.Value.Date);
            if (startDate is not null)
                query = query.Where(x => x.PublishedOn.Date >= startDate.Value.Date);
            if (authorId > 0)
                query = query.Where(x => x.Author.Id == authorId);
            //should be the last as Limit() does not return an ILiteQueryable<T>.
            if (maxCount > 0)
                return query.Limit(maxCount).ToArray();

            return query.ToArray();
        }

        public BlogPost GetBlogPost(int id)
        {
            return BlogPosts.FindById(id);
        }

        public void UploadBlogPost(BlogPost blogPost)
        {
            BlogPosts.Insert(blogPost);
        }

        public void UpdateBlogPost(BlogPost blogPost)
        {
            BlogPosts.Update(blogPost);
        }
    }
}